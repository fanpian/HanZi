using System.Diagnostics.CodeAnalysis;

namespace Dictionaries.Model
{
    /// <summary>
    /// 拼音检索模型对象
    /// </summary>
    public class PinYinJianSuoPageModel
    {

        /// <summary>
        /// 拼音字母
        /// </summary>
        public string PinYin { get; set; } = string.Empty;

        /// <summary>
        /// 拼音详细页面Url地址
        /// </summary>
        public string? Url { get; set; }
    }

    /// <summary>
    /// 汉字简单页面的模型
    /// </summary>
    public class HanZiSimplePageModel:PinYinJianSuoPageModel
    {
        /// <summary>
        /// 汉字
        /// </summary>
        public string HanZi { get; set; } = string.Empty;
    }
}