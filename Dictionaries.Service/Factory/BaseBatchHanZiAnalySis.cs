using Dictionaries.Model;
using Dictionaries.Service.Factory.AiesAnalySis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionaries.Service.Factory
{
    public class BaseBatchHanZiAnalySis:BaseHtmlAnalySis, IBatchHanZiAnalySis
    {
        private readonly BlockingCollection<HanZiModel> _bc;
        private readonly ConcurrentQueue<HanZiSimplePageModel> _queue;

        private readonly IHanZiDetailPageAnalySis _iHanZiDetailPageAnalySis;
        public BaseBatchHanZiAnalySis(IHanZiDetailPageAnalySis hanZiDetailPageAnalySis)
        {
            _iHanZiDetailPageAnalySis = hanZiDetailPageAnalySis;
            _queue = new ConcurrentQueue<HanZiSimplePageModel>();
            _bc = new BlockingCollection<HanZiModel>();
        }

        /// <summary>
        /// 汉字解析
        /// </summary>
        /// <param name="threadNum">使用的线程数量</param>
        /// <returns></returns>
        public List<HanZiModel> AnalySis(IEnumerable<HanZiSimplePageModel> yinJianSuoPageModels, int threadNum = 500) 
        {
            List<HanZiModel> result = new List<HanZiModel>();
            try
            {
                Console.WriteLine("将汉字放入堆栈中");
                foreach (HanZiSimplePageModel model in yinJianSuoPageModels)
                {
                    _queue.Enqueue(model);
                }
                Task[] tasks = new Task[threadNum];
                Console.WriteLine($"根据拼音检索汉字的线程共 {threadNum}个");
                
                for (int i = 0; i < threadNum; i++)
                {
                    tasks[i] = Task.Run(() => AnalySisHanZiPage());
                }
                Task.WaitAll(tasks);
                result = _bc.OrderBy(o => o.PinYin).ToList();
            }
            catch (Exception ex)
            {
            }
            finally { 
                _bc.Dispose();
            }
            return result;
        }

        /// <summary>
        /// 解析汉字页面
        /// </summary>
        private void AnalySisHanZiPage()
        {
            while (_queue.Count > 0)
            {
                HanZiSimplePageModel? hanZiPage = null;
                try
                {
                    if (!_queue.TryDequeue(out hanZiPage))
                    {
                        continue;
                    }
                    Monitor.Enter(hanZiPage);
                    Console.WriteLine($"正在检索汉字：{hanZiPage.HanZi}");
                    if (string.IsNullOrWhiteSpace(hanZiPage.Url))
                    {
                        continue;
                    }
                    IHanZiDetailPageAnalySis hanZiDetailPageAnalySis = new AiesHanZiDetailPageAnalySis();
                    HanZiModel hanZi = hanZiDetailPageAnalySis.Analysis(hanZiPage.Url, hanZiPage.PinYin);
                    // HanZiModel hanZi = _iHanZiDetailPageAnalySis.Analysis(hanZiPage.Url, hanZiPage.PinYin);
                    _bc.Add(hanZi);
                    Console.WriteLine($"完成检索汉字：{hanZiPage.HanZi}");
                    Console.WriteLine($"剩余汉字：{_queue.Count} 个");
                }
                catch (Exception ex)
                {
                    if (hanZiPage != null)
                    {
                        Console.WriteLine($"线程处理异常,汉字：{hanZiPage.HanZi},重新添加到队列末尾");
                        _queue.Enqueue(hanZiPage);
                    }
                }
                finally
                {
                    if (hanZiPage != null)
                    {
                        Monitor.Exit(hanZiPage);
                    }
                }
            }
        }
    }
}
