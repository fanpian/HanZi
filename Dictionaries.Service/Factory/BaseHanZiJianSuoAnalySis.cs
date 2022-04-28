using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dictionaries.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionaries.Service.Factory
{
    /// <summary>
    /// 常用汉字检索解析类
    /// </summary>
    public class BaseHanZiJianSuoAnalySis: BaseHtmlAnalySis, IHanZiJianSuoAnalySis
    {
        private readonly BlockingCollection<HanZiSimplePageModel> _bc;
        private readonly ConcurrentQueue<PinYinJianSuoPageModel> _queue;
        public BaseHanZiJianSuoAnalySis()
        {
            _queue = new ConcurrentQueue<PinYinJianSuoPageModel>();
            _bc = new BlockingCollection<HanZiSimplePageModel>();
        }

        /// <summary>
        /// 解析得到每一个汉字的Url地址
        /// </summary>
        /// <param name="threadNum">使用的线程数量</param>
        /// <returns></returns>
        public List<HanZiSimplePageModel> Analysis(IEnumerable<PinYinJianSuoPageModel> pinYinJianSuos, int threadNum = 50)
        {
            try
            {
                Console.WriteLine("将拼音放入堆栈中");
                foreach (PinYinJianSuoPageModel model in pinYinJianSuos)
                {
                    _queue.Enqueue(model);
                }
                Console.WriteLine($"根据拼音检索汉字的线程共 {threadNum}个");
                Task[] tasks = new Task[threadNum];

                for (int i = 0; i < threadNum; i++)
                {
                    tasks[i] = Task.Run(() => AnalySisHanZiPage());
                }
                Task.WaitAll(tasks);
                return _bc.OrderBy(o => o.PinYin).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                _bc.Dispose();
            }
            return new List<HanZiSimplePageModel>();
        }

        /// <summary>
        /// 解析Html文档得到C#对象结果
        /// </summary>
        /// <returns></returns>
        public void AnalySisHanZiPage()
        {
            while (_queue.Count > 0)
            {
                PinYinJianSuoPageModel? pinYinJianSuoPage = null;
                try
                {
                    if (!_queue.TryDequeue(out pinYinJianSuoPage))
                    {
                        continue;
                    }
                    Console.WriteLine($"正在检索拼音：{pinYinJianSuoPage.PinYin}");
                    Monitor.Enter(pinYinJianSuoPage);
                    // Console.WriteLine($"**线程 - ({thread}) 正在处理：{pinYinJianSuoPage.PinYin}");
                    if (string.IsNullOrWhiteSpace(pinYinJianSuoPage.Url))
                    {
                        continue;
                    }
                    IDocument documnet = base.RequestDocument(pinYinJianSuoPage.Url).Result;
                    IHtmlElement? pinYinEl = documnet.QuerySelector<IHtmlElement>("div.panel > div.mcon > div.jjj > div.bthh > h1 > strong");
                    IEnumerable<IHtmlAnchorElement> elements = documnet.QuerySelectorAll<IHtmlAnchorElement>("div.panel > div.mcon > div.jjj > ul.lst6:not(:last-of-type) > li > a");
                    foreach (IHtmlAnchorElement element in elements)
                    {
                        _bc.Add(new HanZiSimplePageModel
                        {
                            PinYin = pinYinEl?.Text()?.Trim() ?? string.Empty,
                            HanZi = element.Text().Trim(),
                            Url = element.Href?.Trim()
                        });
                    }
                    Console.WriteLine($"完成检索拼音：{pinYinJianSuoPage.PinYin}");
                    Console.WriteLine($"剩余拼音：{_queue.Count} 个");
                }
                catch (Exception ex)
                {
                    // Console.WriteLine(ex.ToString());
                    if (pinYinJianSuoPage != null)
                    {
                        Console.WriteLine($"线程处理异常,拼音：{pinYinJianSuoPage.PinYin},重新添加到队列末尾");
                        _queue.Enqueue(pinYinJianSuoPage);
                    }
                }
                finally
                {
                    if (pinYinJianSuoPage != null)
                    {
                        Monitor.Exit(pinYinJianSuoPage);
                    }
                }
            }
        }
    }
}
