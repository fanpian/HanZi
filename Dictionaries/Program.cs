// See https://aka.ms/new-console-template for more information
using Dictionaries.Service;
using Dictionaries.Service.Factory.AiesAnalySis;
using System.Text;

Console.WriteLine("汉字拼音生成系统：");

CheckDBTableService.Check();

Console.WriteLine("开始从GB18030-2005标准生成汉字,符号或者其他语言不生成.");
GB18030_2005Service.Insert();

AiesAnalySisFactory aiesAnalySisFactory = new AiesAnalySisFactory(50, 100);
aiesAnalySisFactory.AnalySis();
Console.WriteLine("执行导出成Json");
ExportService.ExportJson();
Console.WriteLine("完成导出成Json");

Console.WriteLine("执行导出成CS");
ExportService.ExportCS();
Console.WriteLine("完成导出成CS");

Console.ReadLine();