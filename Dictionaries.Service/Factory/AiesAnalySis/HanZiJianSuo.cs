using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dictionaries.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionaries.Service.Factory.AiesAnalySis
{
    /// <summary>
    /// 所有汉字的检索
    /// </summary>
    public class HanZiJianSuo
    {
        private BlockingCollection<HanZiSimplePageModel> _bc;
        private ConcurrentQueue<PinYinJianSuoPageModel> _queue;
        private int _total = 0;

        public HanZiJianSuo(IEnumerable<PinYinJianSuoPageModel> pinYinPage)
        {
            _bc = new BlockingCollection<HanZiSimplePageModel>();
            _queue = new ConcurrentQueue<PinYinJianSuoPageModel>(pinYinPage);
        }

        /// <summary>
        /// 解析Html文档得到C#对象结果
        /// </summary>
        /// <returns></returns>
        public List<HanZiSimplePageModel> Analysis()
        {
            try
            {
                int theadNum = 50;
                Task[] tasks = new Task[theadNum];

                for (int i = 0; i < theadNum; i++)
                {
                    int j = i;
                    tasks[i] = Task.Run(() => AnalySisHanZiPage(j));
                }
                Task.WaitAll(tasks);
                return _bc.OrderBy(o=>o.PinYin).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally {
                _bc.Dispose();
            }
            return new List<HanZiSimplePageModel>();
        }

        /// <summary>
        /// 解析Html文档得到C#对象结果
        /// </summary>
        /// <returns></returns>
        public void AnalySisHanZiPage(int thread)
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
                    Monitor.Enter(pinYinJianSuoPage);
                    _total += 1;
                    Console.WriteLine($"**线程 - ({thread}) 正在处理：{pinYinJianSuoPage.PinYin}");
                    if (string.IsNullOrWhiteSpace(pinYinJianSuoPage.Url))
                    {
                        continue;
                    }
                    IConfiguration config = Configuration.Default.WithDefaultLoader();
                    IBrowsingContext context = BrowsingContext.New(config);
                    IDocument documnet = context.OpenAsync(pinYinJianSuoPage.Url).Result;
                    IHtmlElement? pinYinEl = documnet.QuerySelector<IHtmlElement>("div.panel > div.mcon > div.jjj > div.bthh > h1 > strong");
                    IEnumerable<IHtmlAnchorElement> elements = documnet.QuerySelectorAll<IHtmlAnchorElement>("div.panel > div.mcon > div.jjj > ul.lst6:not(:last-of-type) > li > a");
                    foreach (IHtmlAnchorElement element in elements)
                    {
                        _total += 1;
                        _bc.Add(new HanZiSimplePageModel {
                            PinYin = pinYinEl?.Text()?.Trim() ?? string.Empty,
                            HanZi = element.Text().Trim(),
                            Url = element.Href?.Trim()
                        });
                    }
                    Console.WriteLine($"##线程 - ({thread}) 处理完成：{pinYinJianSuoPage.PinYin}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    if (pinYinJianSuoPage != null)
                    {
                        Console.WriteLine("线程处理异常,重新写入队列");
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
