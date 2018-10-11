using System.Collections.Generic;
using LiteJson.Common;
using LiteJson.Stream;

namespace LiteJson.Analyzer
{
    internal class LexicalAnalyzer : IAnalyzer
    {
        internal enum LexerStateType
        {
            Begin = 0,
            Number = 1,
            Float = 2,
            String = 3,
            Identity = 4,
            End = 255,
        }

        private readonly CharacterStream CharsStream_;
        private TokenStream TokensStream_;

        private LexerStateType CurrentType_ = LexerStateType.End;
        private readonly StringStream CurrentText_;
        private int CurrentLine_ = 1;
        private int CurrentLineIndex_ = 0;

        public LexicalAnalyzer(CharacterStream CharsStream)
        {
            CharsStream_ = CharsStream;
            CurrentText_ = new StringStream();
        }

        public TokenStream GetTokenStream()
        {
            return TokensStream_;
        }

        public bool Analyzing()
        {
            CharsStream_.Reset();
            CurrentLine_ = 1;
            CurrentLineIndex_ = 0;
            var Tokens = new List<Token>();

            while (!CharsStream_.IsEnd())
            {
                var Tok = ParseToken();
                if (Tok.Type_ == TokenType.None)
                {
                    continue;
                }
                if (Tok.Type_ == TokenType.Error)
                {
                    return false;
                }
                Tokens.Add(Tok);
            }

            TokensStream_ = new TokenStream(Tokens.ToArray());
            return true;
        }

        private Token ParseToken()
        {
            var TokType = TokenType.Error;
            CurrentType_ = LexerStateType.Begin;
            CurrentText_.Reset();

            var TokenLine = CurrentLine_;
            var TokenLineIndex = CurrentLineIndex_;

            while (!CharsStream_.IsEnd() && CurrentType_ != LexerStateType.End)
            {
                var Ch = CharsStream_.Take();
                CurrentText_.Push(Ch);
                CurrentLineIndex_++;

                switch (CurrentType_)
                {
                    #region Begin
                    case LexerStateType.Begin:
                        if (LanguageDescriptor.IsWhitespaceChar(Ch))
                        {
                            TokType = TokenType.None;
                            CurrentType_ = LexerStateType.End;
                        }
                        else if (LanguageDescriptor.IsEndOfLineChar(Ch))
                        {
                            CurrentLine_++;
                            CurrentLineIndex_ = 0;

                            TokType = TokenType.None;
                            CurrentType_ = LexerStateType.End;
                        }
                        else if (LanguageDescriptor.IsDelimiterChar(Ch))
                        {
                            TokType = TokenType.Delimiter;
                            CurrentType_ = LexerStateType.End;
                        }
                        else if (LanguageDescriptor.IsDigitChar(Ch))
                        {
                            CurrentType_ = LexerStateType.Number;
                        }
                        else if (LanguageDescriptor.IsQuoteChar(Ch))
                        {
                            CurrentType_ = LexerStateType.String;
                        }
                        else if (LanguageDescriptor.IsIdentityChar(Ch))
                        {
                            CurrentType_ = LexerStateType.Identity;
                        }
                        break;
                    #endregion
                    #region Number
                    case LexerStateType.Number:
                        if (LanguageDescriptor.IsPointChar(Ch))
                        {
                            CurrentType_ = LexerStateType.Float;
                        }
                        else if (!LanguageDescriptor.IsDigitChar(Ch))
                        {
                            CurrentText_.Pop();
                            CharsStream_.Back();
                            
                            if (LanguageDescriptor.IsWhitespaceChar(Ch) ||
                                LanguageDescriptor.IsDelimiterChar(Ch) ||
                                LanguageDescriptor.IsEndOfLineChar(Ch))
                            {
                                TokType = TokenType.Numeric;
                            }
                            else
                            {
                                TokType = TokenType.Error;
                                Logger.Add(CurrentLine_, CurrentLineIndex_, $"unexpected character '{Ch}' in {CurrentText_}");
                            }

                            CurrentType_ = LexerStateType.End;
                        }
                        break;
                    #endregion
                    #region Float
                    case LexerStateType.Float:
                        if (!LanguageDescriptor.IsDigitChar(Ch))
                        {
                            CurrentText_.Pop();
                            CharsStream_.Back();

                            if (LanguageDescriptor.IsWhitespaceChar(Ch) ||
                                LanguageDescriptor.IsDelimiterChar(Ch) ||
                                LanguageDescriptor.IsEndOfLineChar(Ch))
                            {
                                TokType = TokenType.Numeric;
                            }
                            else
                            {
                                TokType = TokenType.Error;
                                Logger.Add(CurrentLine_, CurrentLineIndex_, $"unexpected character '{Ch}' in {CurrentText_}");
                            }

                            CurrentType_ = LexerStateType.End;
                        }
                        break;
                    #endregion
                    #region String
                    case LexerStateType.String:
                        if (LanguageDescriptor.IsEscapeChar(Ch))
                        {
                            CurrentText_.Pop();
                            CurrentText_.Push(LanguageDescriptor.EscapeChar(CharsStream_.Take()));
                        }
                        else if (LanguageDescriptor.IsQuoteChar(Ch) && CurrentText_.Index(0) == Ch)
                        {
                            CurrentText_.Pop();
                            CurrentText_.Remove(0);

                            TokType = TokenType.String;
                            CurrentType_ = LexerStateType.End;
                        }
                        else if (LanguageDescriptor.IsEndOfLineChar(Ch))
                        {
                            CurrentText_.Pop();
                            CharsStream_.Back();

                            TokType = TokenType.Error;
                            CurrentType_ = LexerStateType.End;
                            Logger.Add(CurrentLine_, CurrentLineIndex_, $"unexpected <eof> in {CurrentText_}");
                        }
                        break;
                    #endregion
                    #region Identity
                    case LexerStateType.Identity:
                        if (!LanguageDescriptor.IsIdentityChar(Ch) && !LanguageDescriptor.IsDigitChar(Ch))
                        {
                            CurrentText_.Pop();
                            CharsStream_.Back();
                            TokType = TokenType.Identity;
                            CurrentType_ = LexerStateType.End;
                        }
                        break;
                        #endregion
                }
            }

            var TokCode = CurrentText_.ToString();
            if (LanguageDescriptor.IsBooleanString(TokCode))
            {
                TokType = TokenType.Boolean;
            }
            else if (LanguageDescriptor.IsNullString(TokCode))
            {
                TokType = TokenType.Null;
            }

            return new Token(TokType, TokCode, TokenLine, TokenLineIndex);
        }

        public void Display()
        {
            TokensStream_.Reset();

            while (!TokensStream_.IsEnd())
            {
                var Tok = TokensStream_.Take();
                System.Console.WriteLine("[{0}:{1}] {2} : {3}", Tok.Line_, Tok.Index_, Tok.Type_, Tok.Code_);
            }

            TokensStream_.Reset();
        }
    }
}