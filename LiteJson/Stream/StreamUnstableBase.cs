using System;

namespace LiteJson.Stream
{
    internal class StreamUnstableBase<T>
    {
        private const int DefaultCapacity = 16;

        protected T[] Buffer_ = null;
        protected int Index_ = 0;

        protected StreamUnstableBase()
            : this(DefaultCapacity)
        {
        }

        protected StreamUnstableBase(int Capacity)
        {
            Buffer_ = new T[Capacity];
            Array.Clear(Buffer_, 0, Buffer_.Length);
        }

        private void Resize(int Capacity)
        {
            Array.Resize(ref Buffer_, Capacity);
        }

        public void Reset()
        {
            Index_ = 0;
        }

        public int Length()
        {
            return Index_;
        }

        public void Push(T Value)
        {
            if (Index_ >= Buffer_.Length)
            {
                Resize(Buffer_.Length * 2);
            }

            Buffer_[Index_++] = Value;
        }

        public T Pop()
        {
            return Index_ > 0 ? Buffer_[--Index_] : default(T);
        }

        public T Index(int Index)
        {
            return Index < Index_ ? Buffer_[Index] : default(T);
        }

        public void Remove(int Index)
        {
            for (var Offset = Index; Offset < Index_ - 1; ++Offset)
            {
                Buffer_[Offset] = Buffer_[Offset + 1];
            }
            Index_--;
        }
    }
}