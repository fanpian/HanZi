using Dictionaries.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionaries.Service.Factory
{
    /// <summary>
    /// 汉字详细页面解析
    /// </summary>
    public interface IHanZiDetailPageAnalySis: IHtmlAnalySis
    {
        /// <summary>
        /// 解析Html文档得到C#对象结果
        /// </summary>
        /// <param name="url">汉字的url地址</param>
        /// <param name="pinYin">汉字的拼音</param>
        /// <returns></returns>
        HanZiModel Analysis(string url, string pinYin);
    }
}
