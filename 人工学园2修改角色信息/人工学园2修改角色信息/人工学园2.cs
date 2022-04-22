using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace 人工学园2修改角色信息
{
    class 人工学园2
    {
        byte 字节0 = 0;
        byte 字节1 = 1;
        public string 文件路径;
        Encoding 文件编码 = Encoding.GetEncoding("gb2312");
        Random 随机 = new Random();
        static string 数据目录 = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "Data" + Path.DirectorySeparatorChar;
        string[] 中式姓 = File.ReadAllLines(数据目录 + "中式姓.txt");
        string[] 中式中性字 = File.ReadAllLines(数据目录 + "中式中性字.txt");
        string[] 中式男名 = File.ReadAllLines(数据目录 + "中式男名.txt");
        string[] 中式女名 = File.ReadAllLines(数据目录 + "中式女名.txt");
        string[] 日式姓 = File.ReadAllLines(数据目录 + "日式姓.txt");
        string[] 日式男名 = File.ReadAllLines(数据目录 + "日式男名.txt");
        string[] 日式女名 = File.ReadAllLines(数据目录 + "日式女名.txt");
        string []持有物_爱情数据 = File.ReadAllLines(数据目录 + "持有物-爱情.txt");
        string []持有物_友情数据= File.ReadAllLines(数据目录 + "持有物-友情.txt");
        string [] 持有物_色情数据 = File.ReadAllLines(数据目录 + "持有物-色情.txt");
        List<byte[]> 图片 = new List<byte[]>();
        byte[] 图片结束 = { 0x49, 0x45, 0x4E, 0x44 };
        List<byte> 游戏数据 = new List<byte>();
        int 游戏数据长度 = 3015;
        byte[] 性别;
        int 性别位置 = 20;
        int 性别长度 = 1;
        byte[] 姓;
        int 姓位置 = 21;
        int 姓长度 = 12;
        byte[] 名;
        int 名位置 = 281;
        int 名长度 = 12;
        List<bool> 个性;
        int 个性位置 = 1698;
        int 个性长度 = 38;
        byte[] 持有物_爱情;
        int 持有物_爱情位置 = 1867;
        int 持有物_爱情长度 = 26;
        byte[] 持有物_友情;
        int 持有物_友情位置 = 2127;
        int 持有物_友情长度 = 26;
        byte[] 持有物_色情;
        int 持有物_色情位置 = 2387;
        int 持有物_色情长度 = 26;
        private int 获取连续字节位置(byte[] 源字节, byte[] 搜索字节)
        {
            int 位置 = -1;
            for (int i = 0; i < 源字节.Length; i++)
            {
                if (源字节[i] == 搜索字节[0])//第一个符合,检测与搜索字节剩下的是否匹配
                {
                    bool 相等 = true;
                    for (int j = 1; j < 搜索字节.Length; j++)
                    {
                        if (i + j < 源字节.Length && 源字节[i + j] != 搜索字节[j])
                        {
                            相等 = false;
                            break;
                        }
                    }
                    if (相等)
                    {
                        位置 = i;
                        break;
                    }
                }
            }
            return 位置;
        }

        private string 文件字节转字符串(byte[] 字节)
        {
            for (int i = 0; i < 字节.Length; i++)
            {
                字节[i] = (byte)(0xFF - (int)字节[i]);
            }
            return 文件编码.GetString(字节).TrimEnd('\0');//去除
        }

        private byte[] 字符串转文件字节(string 字符串, int 长度)
        {
            List<byte> 字节 = new List<byte>();
            文件编码.GetBytes(字符串);
            byte[] 原字节 = 文件编码.GetBytes(字符串);
            for (int i = 0; i < 原字节.Length; i++)
            {
                字节.Add((byte)(0XFF - (int)原字节[i]));
            }
            int 补充长度 = 长度 - 字节.Count;
            if (补充长度 > 0)
            {
                for (int i = 0; i < 补充长度; i++)
                {
                    字节.Add(0XFF);
                }
            }
            return 字节.ToArray();
        }
        public string 获取随机姓(string 风格)
        {
            if (风格 == "中式")
            {
                return 中式姓[随机.Next(0, 中式姓.Length)];
            }
            else
            {
                return 日式姓[随机.Next(0, 日式姓.Length)];
            }
        }
        public string 获取随机名(string 风格, string 性别)
        {
            string[] 名字数据;
            if (风格 == "中式")
            {
                名字数据 = 性别 == "男" ? 中式男名 : 中式女名;
            }
            else
            {
                名字数据 = 性别 == "男" ? 日式男名 : 日式女名;
            }
            string 名字 = 名字数据[随机.Next(0, 名字数据.Length)];
            if (风格 == "中式")
            {
                //由于考虑到性别对名字的偏向性,名字会始终带有本性别的字而不是只有中性字的名
                if (随机.Next(1, 3) > 1)//常见中式名字1-2个字
                {
                    int[] 模式;
                    if (性别 == "男")
                    {
                        模式 = new int[] { 1, 2 };//男性不使用叠字
                    }
                    else
                    {
                        模式 = new int[] { 0, 1, 2 };
                    }
                    switch (模式[随机.Next(0, 模式.Length)])
                    {
                        case 0://叠字,类似倩倩
                            名字 = 名字 + 名字;
                            break;
                        case 1://当前性别名里查找一个新字,类似倩雪,顺序什么的因为都在一个性别内其实无所谓
                            名字 += 名字数据[随机.Next(0, 名字数据.Length)];
                            break;
                        case 2://查找一个中性字放在前面,类似文倩
                            名字 = 中式中性字[随机.Next(0, 中式中性字.Length)] + 名字;
                            break;
                    }
                }
            }
            return 名字;
        }
        private void 初始化角色数据结构()
        {
            //角色数据块似乎是固定的3015字节,根据同个妹子多次重新保存的结果,每次都会生成不同数据,倒数第4字节会改变,第1676,1677,1678字节改变,还有一些,大概是时间相关
            //据分析字节发现,最前面是【エディット】(最初没有头绪的我想了各种办法,换了n多编码一个个试出来的),编码shift-jis,长度14字节,加上6字节不明数据块(0x00,0x00,0x67,0x00,0x00,0x00)
            //后面一字节据分析应该是性别,0x00男0x01女
            //姓和名最长6个,据测试发现为标准ascii字符时(比如111111和222222),姓是上面的后面6个字节,为多字节字符时(比如张张张张张张),姓是上面的后面12字节
            //1在字节里值为0xCE(206),按照一般来说因为需要兼容的关系任何编码对ascii标准字符的编码是一致的,1在ascii里值是0x31(49),很容易看到两者之和为0xff(255)
            //所以根据猜测应该是0xff减去实际的编码值,2的ascii编码值0x32(50),相应减去的结果为0xcd,与后续测试文件的值符合
            //张在文件里的字节值为0x2a(42),0x3a(58),所以编码原值是0xd5(213),0xc5(197),ascii的结果为??应该不对,utf8据查不会用双字节保存中文排除,shift-jis结果为ﾕﾅ也应该不对,unciode结果为엕也应该不对
            //还剩下的中文字符集有gb18030,gbk,gb2312,用这3个都能得到张的正确结果,为了更具体判断是什么字符集,我使用gb2312不支持的繁体字保存成功但读取错误,而其它两种编码都是支持繁体所以应该是gb2312
            //根据后续分析,个性部分为1698-1735,共38个,0x00代表没有此个性,0x01表示有此个性
            性别 = 游戏数据.Skip(性别位置).Take(性别长度).ToArray();
            姓 = 游戏数据.Skip(姓位置).Take(姓长度).ToArray();
            个性 = new List<bool>();
            foreach (var item in 游戏数据.Skip(个性位置).Take(个性长度))
            {
                个性.Add(item== 字节1);
            }
            名 = 游戏数据.Skip(名位置).Take(名长度).ToArray();
            持有物_爱情 = 游戏数据.Skip(持有物_爱情位置).Take(持有物_爱情长度).ToArray();
            持有物_友情 = 游戏数据.Skip(持有物_友情位置).Take(持有物_友情长度).ToArray();
            持有物_色情 = 游戏数据.Skip(持有物_色情位置).Take(持有物_色情长度).ToArray();
        }
        public void 读取(string 路径)
        {
            //清空原值
            图片.Clear();
            游戏数据.Clear();
            文件路径 = 路径;
            //读取数据
            byte[] 数据 = File.ReadAllBytes(文件路径);
            //读取图片数据
            int 图片结束位置 = 获取连续字节位置(数据, 图片结束) + 8;//加上后面固定的8字节,通过计算png文件头应该可以直接获取位置来提高性能
            图片.Add(数据.Take(图片结束位置).ToArray());
            游戏数据.AddRange(数据.Skip(图片结束位置).Take(游戏数据长度));
            图片.Add(数据.Skip(图片结束位置 + 游戏数据长度).ToArray());
            初始化角色数据结构();
        }

        public Image 获取图片(int 索引)
        {
            return Image.FromStream(new MemoryStream(图片[索引]));
        }

        public byte[] 获取游戏数据字节()
        {
            return 游戏数据.ToArray();
        }

        public string 获取性别()
        {
            return 性别[0] == 0 ? "男" : "女";
        }

        public string 获取姓()
        {
            return 文件字节转字符串(姓);
        }

        public void 设置姓(string 值)
        {
            姓 = 字符串转文件字节(值, 姓长度);
            游戏数据.RemoveRange(姓位置, 姓长度);
            游戏数据.InsertRange(姓位置, 姓);
        }

        public string 获取名()
        {
            return 文件字节转字符串(名);
        }

        public void 设置名(string 值)
        {
            名 = 字符串转文件字节(值, 名长度);
            游戏数据.RemoveRange(名位置, 名长度);
            游戏数据.InsertRange(名位置, 名);
        }

        public bool 获取个性(int 个性索引)
        {
            return 游戏数据[个性位置 + 个性索引]== 字节1;
        }

        public void 设置个性(int 个性索引,bool 拥有)
        {
            游戏数据[个性位置 + 个性索引] = 拥有? 字节1: 字节0;
        }

        public string 获取持有物_爱情()
        {
            return 文件字节转字符串(持有物_爱情);
        }

        public void 设置持有物_爱情(string 值)
        {
            持有物_爱情 = 字符串转文件字节(值, 持有物_爱情长度);
            游戏数据.RemoveRange(持有物_爱情位置, 持有物_爱情长度);
            游戏数据.InsertRange(持有物_爱情位置, 持有物_爱情);
        }
        public string 获取持有物_友情()
        {
            return 文件字节转字符串(持有物_友情);
        }

        public void 设置持有物_友情(string 值)
        {
            持有物_友情 = 字符串转文件字节(值, 持有物_友情长度);
            游戏数据.RemoveRange(持有物_友情位置, 持有物_友情长度);
            游戏数据.InsertRange(持有物_友情位置, 持有物_友情);
        }
        public string 获取持有物_色情()
        {
            return 文件字节转字符串(持有物_色情);
        }

        public string 获取随机持有物(string 类型)
        {
            string[] 持有物数据;
            switch (类型)
            {
                case "爱情" :
                    持有物数据 = 持有物_爱情数据;
                    break; case "友情" :
                    持有物数据 = 持有物_友情数据;
                    break; case "色情" :
                    持有物数据 = 持有物_色情数据;
                    break;
                default:
                    return "";
            }
            return 持有物数据[随机.Next(0, 持有物数据.Length)];
        }

        public void 设置持有物_色情(string 值)
        {
            持有物_色情 = 字符串转文件字节(值, 持有物_色情长度);
            游戏数据.RemoveRange(持有物_色情位置, 持有物_色情长度);
            游戏数据.InsertRange(持有物_色情位置, 持有物_色情);
        }
        public void 保存(string 路径)
        {
            //合成数据
            List<byte> 文件数据 = new List<byte>();
            文件数据.AddRange(图片[0]);
            文件数据.AddRange(游戏数据);
            文件数据.AddRange(图片[1]);
            File.WriteAllBytes(路径, 文件数据.ToArray());
        }
    }
}
