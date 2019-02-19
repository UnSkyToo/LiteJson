using System;
using System.IO;
using LiteJson;

namespace LiteJsonTest
{
    class Program
    {
        static void Main(string[] Args)
        {
            //var text = File.ReadAllText("F:\\test.json");

            /*var jobj = JsonConvert.DeserializeJsonObject(@"{
               'CPU': 'Intel',
               'Core': 4,
               'Drives': [
                 'DVD read/writer',
                 '500 gigabyte hard drive',
                 10
               ]
             }");*/
             
            var text = @"{'key' : -1254.5455, 'a':1, 'b':2, 'c':{'c1':'asd'}}";

            var jobj = JsonConvert.DeserializeJsonObject(text);

            if (jobj == null)
            {
                Console.WriteLine(JsonConvert.GetErrorMsg());
            }
            else
            {
                foreach (var obj in jobj)
                {
                    Console.WriteLine(obj);
                }
                
                Console.WriteLine((double) jobj["key"]);
            }

            Console.ReadKey();
        }
    }
}
