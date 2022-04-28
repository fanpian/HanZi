using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionaries.Service.Factory.AiesAnalySis
{
    /// <summary>
    /// 汉字详细页面解析
    /// </summary>
    public class HanZiAnalySis
    {

        private readonly string _baseUrl;
        private readonly string _pinYin;

        public HanZiAnalySis(string baseUrl, string pinYin)
        {
            _baseUrl = baseUrl;
            _pinYin = pinYin;
        }

        /// <summary>
        /// 解析Html文档得到C#对象结果
        /// </summary>
        /// <returns></returns>
        public Model.HanZiModel Analysis()
        {
            IConfiguration config = Configuration.Default.WithDefaultLoader();
            IBrowsingContext context = BrowsingContext.New(config);
            IDocument documnet = context.OpenAsync(_baseUrl).Result;
            IHtmlElement? hanZiEl = documnet.QuerySelector<IHtmlElement>("div.panel > div.mcon > h1.zdbt");
            IEnumerable<IHtmlSpanElement> elements = documnet.QuerySelectorAll<IHtmlSpanElement>("div.panel > div.mcon span");
            Model.HanZiModel hanZi = new Model.HanZiModel
            {
                PinYin = _pinYin,
                Name = hanZiEl?.Text()?.Trim() ?? string.Empty,
                PinYinSoundmark = GetNextSiblingText(elements, "拼音"),
                ZhuYin = GetNextSiblingText(elements, "注音"),
                BuShou = GetNextSiblingText(elements, "部首"),
                BuWaiBiHua = GetNextSiblingText(elements, "部外笔画"),
                ZongBiHua = GetNextSiblingText(elements, "总笔画"),
                ComplexFont = GetNextSiblingText(elements, "繁体字"),
                ComplexFontForBuShou = GetNextSiblingText(elements, "繁体部首"),
                ComplexFontForBuWaiBiHua = GetNextSiblingText(elements, "繁体部外笔画"),
                ComplexFontFromZongBiHua = GetNextSiblingText(elements, "繁体总笔画"),
                WuBi86 = GetNextSiblingText(elements, "五笔86"),
                WuBi98 = GetNextSiblingText(elements, "五笔98"),
                CangJie = GetNextSiblingText(elements, "仓颉"),
                ZhengMa = GetNextSiblingText(elements, "郑码"),
                DianMa = GetNextSiblingText(elements, "电码"),
                BiShunBianHao = GetNextSiblingText(elements, "笔顺编号"),
                SiJiaoHaoMa = GetNextSiblingText(elements, "四角号码"),
                UniCode = GetNextSiblingText(elements, "UNICODE")
            };
            if (string.IsNullOrWhiteSpace(hanZi.WuBi98))
            {
                hanZi.WuBi98 = hanZi.WuBi86;
            }
            if (string.IsNullOrWhiteSpace(hanZi.ComplexFont))
            {
                hanZi.ComplexFont = hanZi.Name;
            }
            return hanZi;
        }

        /// <summary>
        /// 获取Span紧邻下一个元素的文本
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="startWith"></param>
        /// <returns></returns>
        private string GetNextSiblingText(IEnumerable<IHtmlSpanElement> elements, string startWith)
        {
            IHtmlSpanElement? spanEl = elements.SingleOrDefault(s => s.Text()?.StartsWith(startWith) ?? false);
            if (spanEl == null) return String.Empty;
            INode? node = spanEl.NextSibling;
            StringBuilder sb = new StringBuilder();
            while (node != null)
            {
                if (node is IText || node is IHtmlAnchorElement)
                {
                    sb.Append(node.Text());
                    node = node.NextSibling;
                }
                else if (node is IHtmlScriptElement)
                {
                    node = node.NextSibling;
                }
                else
                {
                    node = null;
                }
            }
            return sb.ToString().Trim();
        }

    }
}
