using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dictionaries.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionaries.Service.Factory
{
    /// <summary>
    /// 拼音单页面检索解析
    /// </summary>
    public class BasePinYinJianSuoAnalySis : BaseHtmlAnalySis, IPinYinJianSuoAnalySis
    {
        private readonly string _url;
        private readonly string _selectors;
        public BasePinYinJianSuoAnalySis(string url, string selector)
        {
            _url = url;
            _selectors = selector;
        }

        /// <summary>
        /// 解析得到所有的拼音
        /// </summary>
        /// <param name="url">url地址</param>
        /// <returns></returns>
        public List<PinYinJianSuoPageModel> Analysis()
        {
            IDocument documnet = base.RequestDocument(_url).Result;
            IEnumerable<IHtmlAnchorElement> elements = documnet.QuerySelectorAll<IHtmlAnchorElement>(_selectors);
            return elements.Select(s => new PinYinJianSuoPageModel
            {
                PinYin = s.Text().Trim(),
                Url = s.Href?.Trim(),
            }).ToList();
        }
    }
}
