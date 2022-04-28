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
    public interface IPinYinJianSuoAnalySis: IHtmlAnalySis
    {
        /// <summary>
        /// 从拼音Url页面解析
        /// </summary>
        /// <returns>得到所有的拼音和url地址</returns>
        List<PinYinJianSuoPageModel> Analysis();
    }
}
