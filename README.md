# LiteJson
A simple JSON parser

simple usage:
```
var jobj = JsonConvert.DeserializeJsonObject(@"{
   'CPU': 'Intel',
   'Core': 4,
   'Drives': [
     'DVD read/writer',
     '500 gigabyte hard drive',
     10
   ]
 }");
 
text = @"{'key' : -1254.5455}";

var jobj = JsonConvert.DeserializeJsonObject(text);

if (jobj == null)
{
    Console.WriteLine(JsonConvert.GetErrorMsg());
}
else
{
    Console.WriteLine((double) jobj["key"]);
}