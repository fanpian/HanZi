using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionaries.Service.Factory
{
    /// <summary>
    /// Html解析接口
    /// </summary>
    public interface IHtmlAnalySis
    {
        /// <summary>
        /// 从Url获取Html文档
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<IDocument> RequestDocument(string url);
    }
}
