using Dictionaries.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionaries.Service.Factory
{
    /// <summary>
    /// 汉字检索接口
    /// 得到所有拼音后，根据拼音查字
    /// </summary>
    public interface IHanZiJianSuoAnalySis: IHtmlAnalySis
    {
        /// <summary>
        /// 解析得到每一个汉字的Url地址
        /// </summary>
        /// <param name="pinYinJianSuos">所有的拼音集合</param>
        /// <param name="threadNum">使用的线程数量</param>
        /// <returns></returns>
        List<HanZiSimplePageModel> Analysis(IEnumerable<PinYinJianSuoPageModel> pinYinJianSuos, int threadNum);
    }
}
