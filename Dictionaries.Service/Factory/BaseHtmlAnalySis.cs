using AngleSharp;
using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionaries.Service.Factory
{
    public class BaseHtmlAnalySis : IHtmlAnalySis
    {
        /// <summary>
        /// 从Url获取Html文档
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Task<IDocument> RequestDocument(string url)
        {
            IConfiguration config = Configuration.Default.WithDefaultLoader();
            IBrowsingContext context = BrowsingContext.New(config);
            return context.OpenAsync(url);
        }
    }
}
