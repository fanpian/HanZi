using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionaries.Model.Entities
{
    /// <summary>
    /// 汉字基类
    /// </summary>
    public class HanZiEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

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
    }
}
