using Dictionaries.Model;
using Dictionaries.Model.Entities;
using Dictionaries.Repository;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionaries.Service.Factory.AiesAnalySis
{
    /// <summary>
    /// aies.cn网站解析工厂
    /// https://zidian.aies.cn/
    /// </summary>
    public class AiesAnalySisFactory
    {
        private readonly IPinYinJianSuoAnalySis _iPinYinJianSuo;
        private readonly IHanZiJianSuoAnalySis _iHanZiJianSuo;
        private readonly IBatchHanZiAnalySis _iBatchHanZi;
        private readonly IHanZiDetailPageAnalySis _iHanZiDetial;

        /// <summary>
        /// 汉字检索时的线程数量
        /// </summary>
        private readonly int _hanZiJianSuoThreadNum;

        /// <summary>
        /// 汉字批量解析时线程数量
        /// </summary>
        private readonly int _hanZiDetialThreadNum;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="hanZiJianSuoThreadNum">汉字检索使用的线程数量</param>
        /// <param name="hanZiDetialThreadNum">汉字详细页面解析使用的线程数量;这里限制100是一般网站并发访问就100差不多了，再多线程，访问网站就空白了</param>
        public AiesAnalySisFactory(int hanZiJianSuoThreadNum = 50, int hanZiDetialThreadNum = 100)
        {
            _hanZiJianSuoThreadNum = hanZiJianSuoThreadNum;
            if (_hanZiJianSuoThreadNum > 100 || _hanZiJianSuoThreadNum < 1)
            {
                _hanZiJianSuoThreadNum = 50;
            }

            _hanZiDetialThreadNum = hanZiDetialThreadNum;
            if (_hanZiDetialThreadNum > 100 || _hanZiDetialThreadNum < 1)
            {
                _hanZiDetialThreadNum = 50;
            }

            _iPinYinJianSuo = new BasePinYinJianSuoAnalySis("https://zidian.aies.cn/pinyin_index.htm", "div.panel > div.mcon > div.jjj > ul.lst6:not(:first-of-type) > li > a");
            _iHanZiJianSuo = new BaseHanZiJianSuoAnalySis();
            _iHanZiDetial = new AiesHanZiDetailPageAnalySis();
            _iBatchHanZi = new BaseBatchHanZiAnalySis(_iHanZiDetial);
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <returns></returns>
        public void AnalySis()
        {
            try
            {
                List<PinYinJianSuoPageModel> pinYins = _iPinYinJianSuo.Analysis();
                Console.WriteLine($"从网站共检索拼音({pinYins.Count})个.");
                List<PinYinEntity> pinYinEntities = InsertPinYin(pinYins);
                Console.WriteLine("将拼音插入数据库");

                List<HanZiSimplePageModel> hanZiPages = _iHanZiJianSuo.Analysis(pinYins, _hanZiJianSuoThreadNum);
                Console.WriteLine($"从网站通过拼音检索汉字共({hanZiPages.Count})个.");

                List<HanZiModel> hanZis = _iBatchHanZi.AnalySis(hanZiPages, _hanZiDetialThreadNum);
                Console.WriteLine("汉字解析完成");
                InsertHanZiPinYin(hanZis, pinYinEntities);
                Console.WriteLine("数据库插入完成");
            }
            catch (Exception ex)
            {
            }
            //BasePinYinJianSuoAnalySis pinYinJianSuoAnalySis = new BasePinYinJianSuoAnalySis("https://zidian.aies.cn/pinyin_index.htm");
            //IEnumerable<PinYinJianSuoPageModel> pinYinPage = pinYinJianSuoAnalySis.Analysis();
            //// 得到所有汉字
            //HanZiJianSuo hanZiJianSuo = new HanZiJianSuo(pinYinPage);
            //List<HanZiSimplePageModel> hanZiPages = hanZiJianSuo.Analysis();
            //hanZiPages.ForEach(f=> _queue.Enqueue(f));

            //int theadNum = 500;
            //Task[] tasks = new Task[theadNum];
            //for (int i = 0; i < theadNum; i++)
            //{
            //    int j = i;
            //    tasks[i] = Task.Run(() => AnalySisHanZiPage(j));
            //}
            //Task.WaitAll(tasks);
            //List<HanZiModel> temp = hanZis.OrderBy(o => o.PinYin).ToList();
            //hanZis.Dispose();
            //return temp;
        }

        public List<PinYinEntity> InsertPinYin(List<PinYinJianSuoPageModel> pinYins)
        {
            using (HanZiContext db = new HanZiContext())
            {
                db.PinYinEntities.AddRange(pinYins.Select(s=> new Model.Entities.PinYinEntity {
                    Name = s.PinYin
                }));
                db.SaveChanges();
                return db.PinYinEntities.ToList();
            };
        }

        public void InsertHanZiPinYin(List<HanZiModel> hanZis, List<PinYinEntity> pinYinEntities)
        {
            using (HanZiContext db = new HanZiContext())
            {
                IEnumerable<HanZiPinYinEntity> entities = hanZis.Select(s => new HanZiPinYinEntity {
                    HanZi = s.Name,
                    HanZiId = 0,
                    GB18030 = "",
                    Unicode = "",
                    PinYin = s.PinYin,
                    PinYinId = 0,
                    YinBiao = s.PinYinSoundmark,
                    ShouZiMu = "",
                    BuShou = s.BuShou,
                    ZhHant = s.ComplexFont?.Replace(")", "") ?? string.Empty
                });
                List<string> hanZiNames = entities.Select(s => s.HanZi).ToList();
                List<HanZiEntity> source = db.HanZiEntities.Where(w => hanZiNames.Contains(w.Name)).ToList();
                List<HanZiPinYinEntity> excess = new List<HanZiPinYinEntity>();
                foreach (HanZiPinYinEntity entity in entities)
                {
                    HanZiEntity? hanZi = source.FirstOrDefault(f => f.Name == entity.HanZi);
                    if (hanZi == null)
                    {
                        excess.Add(entity);
                        continue;
                    }
                    entity.GB18030 = hanZi.GB18030;
                    entity.Unicode = hanZi.Unicode;
                    entity.HanZiId = hanZi.Id;
                    string startsWith = string.Empty;
                    if (entity.PinYin?.StartsWith("Zh", StringComparison.OrdinalIgnoreCase) ?? false)
                    {
                        startsWith = "Zh";
                    }
                    else if (entity.PinYin?.StartsWith("Ch", StringComparison.OrdinalIgnoreCase) ?? false)
                    {
                        startsWith = "Ch";
                    }
                    else if (entity.PinYin?.StartsWith("Sh", StringComparison.OrdinalIgnoreCase) ?? false)
                    {
                        startsWith = "Sh";
                    }
                    else {
                        startsWith = (entity.PinYin?.Substring(0, 1) ?? string.Empty).ToUpper();
                    }
                    entity.ShouZiMu = startsWith;
                    PinYinEntity? pinYin = pinYinEntities.FirstOrDefault(f => f.Name == entity.PinYin);
                    if (pinYin != null)
                    {
                        entity.PinYinId = pinYin.Id;
                    }
                    db.HanZiPinYinEntities.Add(entity);
                }
                db.SaveChanges();
            };
        }
    }
}
