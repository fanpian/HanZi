using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionaries.Model.Entities
{
    /// <summary>
    /// 汉字与拼音关联表
    /// </summary>
    public class HanZiPinYinEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 汉字
        /// </summary>
        public string HanZi { get; set; } = string.Empty;

        /// <summary>
        /// 汉字
        /// </summary>
        public int HanZiId { get; set; }

        /// <summary>
        /// GB18030编码
        /// 从高位到低位
        /// </summary>
        public string GB18030 { get; set; } = string.Empty;

        /// <summary>
        /// unicode编码
        /// 从高位到低位
        /// </summary>
        public string Unicode { get; set; } = string.Empty;

        /// <summary>
        /// 拼音主键
        /// </summary>
        public int PinYinId { get; set; }

        /// <summary>
        /// 拼音
        /// </summary>
        public string PinYin { get; set; } = string.Empty;

        /// <summary>
        /// 带音标的拼音
        /// </summary>
        public string? YinBiao { get; set; }

        /// <summary>
        /// 首字母
        /// 如果是Zh则是两个字符
        /// </summary>
        public string ShouZiMu { get; set; } = string.Empty;

        /// <summary>
        /// 汉字的部首
        /// </summary>
        public string BuShou { get; set; } = string.Empty;

        /// <summary>
        /// 汉字的繁体
        /// </summary>
        public string? ZhHant { get; set; }
    }
}
