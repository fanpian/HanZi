using Dictionaries.Model.Entities;
using Dictionaries.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionaries.Service
{
    /// <summary>
    /// 导出成Json或者*.cs文件
    /// </summary>
    public static class ExportService
    {
        public static void ExportJson()
        {
            using (HanZiContext db = new HanZiContext())
            {
                List<HanZiPinYinEntity> hanZiPinYinEntities = db.HanZiPinYinEntities.ToList();
                var obj = hanZiPinYinEntities.Select(s => new {
                    h = s.HanZi,
                    p = s.PinYin,
                    u = s.Unicode,
                    szm = s.ShouZiMu,
                    bs = s.BuShou,
                    hant = s.ZhHant == s.HanZi ? "" : s.ZhHant
                });
                string json = JsonConvert.SerializeObject(obj);
                string filePath = Path.Combine(@"D:\fanpian\privateJob\Dictionaries\Dictionaries\App_Data\", "json.json");
                File.WriteAllText(filePath, json);
            }
        }

        public static void ExportCS()
        { 
            string hanZiModelFilePath = Path.Combine(@"D:\fanpian\privateJob\Dictionaries\Dictionaries\App_Data\", "HanZiServiceTemplate.txt");
            string hanZiModelText = File.ReadAllText(hanZiModelFilePath);
            StringBuilder sb = new StringBuilder();
            using (HanZiContext db = new HanZiContext())
            {
                List<HanZiPinYinEntity> hanZiPinYinEntities = db.HanZiPinYinEntities.ToList();
                hanZiPinYinEntities.ForEach(f => {
                    string param = $" HanZi = \"{f.HanZi}\", PinYin = \"{f.PinYin}\" , Unicode = \"{f.Unicode}\", ShouZiMu = \"{f.ShouZiMu}\", BuShou = \"{f.BuShou}\", ZhHant = \"{f.ZhHant}\"";
                    sb.AppendLine($"            _hanZis.Add(new HanZiPinYinEntity {{ {param} }});");
                });
            }
            string cs = hanZiModelText.Replace("**汉字拼音数据**", sb.ToString());
            string filePath = Path.Combine(@"D:\fanpian\privateJob\Dictionaries\Dictionaries\App_Data\", "HanZiService.cs");
            File.WriteAllText(filePath, cs);
        }
    }
}
