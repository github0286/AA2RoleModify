using System;
using System.Collections.Generic;
using System.IO;
namespace github0286
{
    class Config
    {
        static Dictionary<string, string> ConfigCache = new Dictionary<string, string>();
        static string ConfigName = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar+"Data"+ Path.DirectorySeparatorChar + "Config.txt";
        static char ConfigDelimiter = '=';
        static bool Change = false;
        public static string Get(string Key)
        {
            return ConfigCache.ContainsKey(Key)?ConfigCache[Key]:"";
        }
        public static void Set(string Key, string Value)
        {
            //禁止使用分隔符
            if (Key.IndexOf(ConfigDelimiter) > -1 || Value.IndexOf(ConfigDelimiter) > -1)
            {
                throw new Exception("不能使用" + ConfigDelimiter);
            }
            else
            {
                if (ConfigCache.ContainsKey(Key))
                {

                    ConfigCache[Key] = Value;
                }
                else
                {
                    ConfigCache.Add(Key, Value);
                }
                Change = true;
            }
        }

        public static void Save()
        {
            List<string> ConfigLine = new List<string>();
            foreach (var item in ConfigCache)
            {
                ConfigLine.Add(item.Key + ConfigDelimiter + item.Value);
            }

            File.WriteAllLines(ConfigName, ConfigLine.ToArray());
        }
        /// <summary>
        /// 从本地读取配置,配置文件不存在会自动创建
        /// </summary>
        public static void Read()
        {
            if (File.Exists(ConfigName))
            {
                string[] TemporaryConfig = File.ReadAllLines(ConfigName);
                foreach (var item in TemporaryConfig)
                {
                    string[] ConfigLine = item.Split(ConfigDelimiter);
                    ConfigCache[ConfigLine[0]] = ConfigLine[1];
                }
            }
            else
            {
                File.Create(ConfigName);
            }
        }
        /// <summary>
        /// 配置是否改变!
        /// </summary>
        public static bool IsChange()
        {
            return Change;
        }
    }
}
