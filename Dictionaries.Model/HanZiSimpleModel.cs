using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionaries.Model
{
    /// <summary>
    /// 汉字简单模型
    /// </summary>
    public class HanZiSimpleModel
    { 
        /// <summary>
        /// 拼音写法
        /// 不带音标
        /// </summary>
        public string PinYin { get; set; } = string.Empty;

        /// <summary>
        /// 汉字
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 拼音写法
        /// 带音标
        /// 多个用,号分割
        /// </summary>
        public string PinYinSoundmark { set; get; } = string.Empty;

        /// <summary>
        /// 简体部首
        /// 仅做参考
        /// </summary>
        public string BuShou { get; set; } = string.Empty;

        /// <summary>
        /// 繁体字
        /// </summary>
        public string ComplexFont { get; set; } = string.Empty;

        /// <summary>
        /// UniCode编码
        /// </summary>
        public string UniCode { get; set; } = string.Empty;
    }
}
