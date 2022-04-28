using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dictionaries.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionaries.Service.Factory.AiesAnalySis
{
    /// <summary>
    /// 拼音检索
    /// </summary>
    public class PinYinJianSuoAnalySis : BaseHtmlAnalySis, IPinYinJianSuoAnalySis
    {
        /// <summary>
        /// 解析得到所有的拼音
        /// </summary>
        /// <param name="url">url地址</param>
        /// <returns></returns>
        public IEnumerable<PinYinJianSuoPageModel> Analysis(string url)
        {
            IDocument documnet = base.RequestDocument(url).Result;
            IEnumerable <IHtmlAnchorElement> elements = documnet.QuerySelectorAll<IHtmlAnchorElement>("div.panel > div.mcon > div.jjj > ul.lst6:not(:first-of-type) > li > a");
            return elements.Select(s => new PinYinJianSuoPageModel
            {
                PinYin = s.Text().Trim(),
                Url = s.Href?.Trim(),
            });
        }
    }
}
