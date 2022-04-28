using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionaries.Model.Entities
{
    /// <summary>
    /// 汉字的拼音
    /// </summary>
    public class PinYinEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 拼音
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
