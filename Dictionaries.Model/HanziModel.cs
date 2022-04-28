using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionaries.Model
{
    /// <summary>
    /// 汉字模型对象
    /// </summary>
    public class HanZiModel
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
        /// 注音
        /// </summary>
        public string ZhuYin { get; set; } = string.Empty;

        /// <summary>
        /// 简体部首
        /// 仅做参考
        /// </summary>
        public string BuShou { get;set; } = string.Empty;

        /// <summary>
        /// 简体部外笔画
        /// 仅做参考
        /// </summary>
        public string BuWaiBiHua { get; set;} = string.Empty;

        /// <summary>
        /// 简体总笔画
        /// 仅做参考
        /// </summary>
        public string ZongBiHua { get;set;} = string.Empty;

        /// <summary>
        /// 繁体字
        /// </summary>
        public string ComplexFont { get; set; } = string.Empty;

        /// <summary>
        /// 繁体部首
        /// 仅做参考
        /// </summary>
        public string ComplexFontForBuShou { get; set; } = string.Empty;

        /// <summary>
        /// 繁体部外笔画
        /// 仅做参考
        /// </summary>
        public string ComplexFontForBuWaiBiHua { get; set;} = string.Empty;

        /// <summary>
        /// 繁体总笔画
        /// 仅做参考
        /// </summary>
        public string ComplexFontFromZongBiHua { get; set;} = string.Empty;

        /// <summary>
        /// 五笔86简码
        /// </summary>
        public string WuBi86 { get; set;} = string.Empty;

        /// <summary>
        /// 五笔98简码
        /// </summary>
        public string WuBi98 { get; set;} = string.Empty;

        /// <summary>
        /// 仓颉码
        /// </summary>
        public string CangJie { get; set;} = string.Empty;

        /// <summary>
        /// 郑码
        /// </summary>
        public string ZhengMa { get; set;} = string.Empty;

        /// <summary>
        /// 电码
        /// </summary>
        public string DianMa { get; set;} = string.Empty;

        /// <summary>
        /// 笔顺编号
        /// </summary>
        public string BiShunBianHao { get; set;} = string.Empty;

        /// <summary>
        /// 四角号码
        /// </summary>
        public string SiJiaoHaoMa { get; set;} = string.Empty;

        /// <summary>
        /// UniCode编码
        /// </summary>
        public string UniCode { get; set;} = string.Empty;
    }
}
