using Dictionaries.Model.Entities;
using Dictionaries.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionaries.Service
{
    /// <summary>
    /// Gb18030-2005中的汉字
    /// </summary>
    public static class GB18030_2005Service
    {
        private static readonly Encoding _gb18030;
        private static readonly Encoding _unicode;

        static GB18030_2005Service()
        {
            Encoding? gb18030 = CodePagesEncodingProvider.Instance.GetEncoding("GB18030");
            if (gb18030 == null)
            {
                throw new Exception("GB18030-2005中的汉字编码不存在");
            }
            _gb18030 = gb18030;
            _unicode = Encoding.Unicode;
        }

        public static void Insert()
        {
            try
            {
                using (HanZiContext db = new HanZiContext())
                {
                    Console.WriteLine("单字节是符号区域(0x00 - 0x7F)，没有意义所以不插入");
                    // List<HanZiEntity> singleByteHanZis = InsertSingleByte();
                    // db.AddRange(singleByteHanZis);
                    Console.WriteLine("准备插入双字节汉字");
                    List<HanZiEntity> doubleByteHanZis = InsertDoubleByte();
                    db.AddRange(doubleByteHanZis);
                    Console.WriteLine($"双字节汉字共{doubleByteHanZis.Count}");
                    db.SaveChanges();
                    Console.WriteLine("准备插入四字节汉字");
                    List<HanZiEntity> fourByteHanZis = InsertFourByte();
                    db.AddRange(fourByteHanZis);
                    Console.WriteLine($"四字节汉字共{fourByteHanZis.Count}");
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
            }
        }
        
        /// <summary>
        /// 单字节是符号区域,对汉字来说，没有意义所以不插入
        /// 插入单字节数据
        /// 0x00 - 0x7F
        /// </summary>
        private static List<HanZiEntity> InsertSingleByte()
        {
            List<HanZiEntity> hanZis = new List<HanZiEntity>();
            for (int i = 0x00; i <= 0x7F; i++) {
                // 目前有些数据是没有意义的
                if (i <= 0x20)
                {
                    continue;
                }
                if (i == 0x7F) // 删除符
                {
                    continue;
                }
                string hexStr = i.ToString("X2");
                byte[] gb = new byte[] {
                    byte.Parse(hexStr, System.Globalization.NumberStyles.HexNumber)
                };
                string hanZi = _gb18030.GetString(gb);
                string unicode = GetUnicodeHex(hanZi);
                hanZis.Add(new HanZiEntity()
                {
                    Name = hanZi,
                    Unicode = unicode,
                    GB18030 = hexStr
                });
            }
            return hanZis;
        }

        /// <summary>
        /// 插入双字节汉字
        /// 第一字节：0x81 - 0xFE
        /// 第二字节：0x40 - 0x7E, 0x80 - 0xFE
        /// </summary>
        private static List<HanZiEntity> InsertDoubleByte()
        {
            List<HanZiEntity> hanZis = new List<HanZiEntity>();
            for (int i = 0x81; i <= 0xFE; i++)
            {
                for (int m = 0x40; m <= 0x7E; m++)
                {

                    // 双字节5区 是符号区域,没有意义,不插入
                    if ((0xA8 <= i && i <= 0xA9) && (0x40 <= m && m <= 0x7E))
                    {
                        continue;
                    }
                    // 双字节用户3区 是保留区域,没有意义,不插入
                    if ((0xA1 <= i && i <= 0xA7) && (0x40 <= m && m <= 0x7E))
                    {
                        continue;
                    }
                    string hexStr = i.ToString("X2") + m.ToString("X2");
                    byte[] gb = new byte[] {
                        byte.Parse(hexStr.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
                        byte.Parse(hexStr.Substring(2, 2), System.Globalization.NumberStyles.HexNumber)
                    };
                    string hanZi = _gb18030.GetString(gb);
                    string unicode = GetUnicodeHex(hanZi);
                    hanZis.Add(new HanZiEntity()
                    {
                        Name = hanZi,
                        Unicode = unicode,
                        GB18030 = hexStr
                    });
                }
                for (int n = 0x80; n <= 0xFE; n++)
                {
                    // 双字节1区 是符号区域,没有意义,不插入
                    if ((0xA1 <= i && i <= 0xA9) && (0xA1 <= n && n <= 0xFE))
                    {
                        continue;
                    }
                    // 双字节5区 是符号区域,没有意义,不插入
                    if ((0xA8 <= i && i <= 0xA9) && (0x80 <= n && n <= 0xA0))
                    {
                        continue;
                    }
                    // 双字节用户1区 是保留区域,没有意义,不插入
                    if ((0xAA <= i && i <= 0xAF) && (0xA1 <= n && n <= 0xFE))
                    {
                        continue;
                    }
                    // 双字节用户2区 是保留区域,没有意义,不插入
                    if ((0xF8 <= i && i <= 0xFE) && (0xA1 <= n && n <= 0xFE))
                    {
                        continue;
                    }
                    // 双字节用户3区 是保留区域,没有意义,不插入
                    if ((0xA1 <= i && i <= 0xA7) && (0x80 <= n && n <= 0xA0))
                    {
                        continue;
                    }
                    string hexStr = i.ToString("X2") + n.ToString("X2");
                    byte[] gb = new byte[] {
                        byte.Parse(hexStr.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
                        byte.Parse(hexStr.Substring(2, 2), System.Globalization.NumberStyles.HexNumber)
                    };
                    string hanZi = _gb18030.GetString(gb);
                    string unicode = GetUnicodeHex(hanZi);
                    hanZis.Add(new HanZiEntity()
                    {
                        Name = hanZi,
                        Unicode = unicode,
                        GB18030 = hexStr
                    });
                }
            }
            return hanZis;
        }

        /// <summary>
        /// 插入四字节汉字
        /// 其他不属于汉字的，就没插入
        /// 0x8139EE39~0x82358738：CJK统一汉字扩充A
        /// 0x95328236~0x9835F336：CJK统一汉字扩充B
        /// </summary>
        /// <returns></returns>
        private static List<HanZiEntity> InsertFourByte()
        {
            List<HanZiEntity> hanZis = new List<HanZiEntity>();
            List<int> firstBytes = new List<int> 
            {
                0x81,
                0x82,
                0x95,
                0x96,
                0x97,
                0x98
            };
            for (int first = 0;first < firstBytes.Count; first++)
            {
                int i = firstBytes[first];
                for (int j = 0x30; j <= 0x39; j++)
                {
                    for (int m = 0x81; m <= 0xFE; m++)
                    {
                        for (int n = 0x30; n <= 0x39; n++)
                        {
                            string hex = $"{i.ToString("X2")}{j.ToString("X2")}{m.ToString("X2")}{n.ToString("X2")}";
                            uint current = uint.Parse(hex, NumberStyles.HexNumber);
                            bool a = (current >= 0x8139EE39 && current <= 0x82358738);
                            bool b = (current >= 0x95328236 && current <= 0x9835F336);
                            if (!a && !b)
                            {
                                continue;
                            }
                            byte[] gb = new byte[] {
                                        byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber),
                                        byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber),
                                        byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber),
                                        byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber)
                                    };
                            string hanZi = _gb18030.GetString(gb);
                            string unicode = GetUnicodeHex(hanZi);
                            hanZis.Add(new HanZiEntity()
                            {
                                Name = hanZi,
                                Unicode = unicode,
                                GB18030 = hex
                            });
                        }
                    }
                }
            }
            return hanZis;
        }

        /// <summary>
        /// 将汉字转为UniCode编码
        /// 从高位到低位
        /// </summary>
        /// <param name="hanZi"></param>
        /// <returns></returns>
        public static string GetUnicodeHex(string hanZi) 
        {
            byte[] bytes = _unicode.GetBytes(hanZi);
            return BitConverter.ToString(bytes.Reverse().ToArray());
        }

        /// <summary>
        /// 将UniCode的16进制编码转为汉字
        /// </summary>
        /// <param name="hex">16进制编码,从高位到低位</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string GetUniCodeChar(string hex)
        {
            if (hex.Length % 2 != 0)
            {
                throw new ArgumentException("参数长度不正确，hex必须是偶数!");
            }
            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = byte.Parse(hex.Substring(i * 2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }
            return _unicode.GetString(bytes.Reverse().ToArray());
        }
    }
}
