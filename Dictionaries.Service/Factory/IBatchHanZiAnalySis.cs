using Dictionaries.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionaries.Service.Factory
{
    /// <summary>
    /// 汉字批量解析
    /// 将所有的汉字Url批量解析
    /// </summary>
    public interface IBatchHanZiAnalySis:IHtmlAnalySis
    {
        /// <summary>
        /// 汉字解析
        /// </summary>
        /// <param name="yinJianSuoPageModels">所有的汉字Url集合</param>
        /// <param name="threadNum">使用的线程数量</param>
        /// <returns></returns>
        List<HanZiModel> AnalySis(IEnumerable<HanZiSimplePageModel> yinJianSuoPageModels, int threadNum);
    }
}
