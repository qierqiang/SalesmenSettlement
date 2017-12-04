using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SalesmenSettlement.Utility
{
    public static class AppConfig
    {
        private static string _appConfigFile = AppDomain.CurrentDomain.BaseDirectory + "appconfig.txt";
        private static Dictionary<string, string> _configs;

        public static string DatabaseConnectionString;

        public static string AppName;

        public static string Company;

        public static string Version;

        static AppConfig()
        {
            DatabaseConnectionString = Read("连接字符串");
            AppName = Read("系统名称");
            Company = Read("公司名称");
            Version = Read("版本号");
        }

        static string Read(string key)
        {
            if (_configs == null)
            {
                string[] lines = File.ReadAllLines(_appConfigFile);
                _configs = new Dictionary<string, string>();
                foreach (string l in lines)
                {
                    if (l.IsNullOrBlank()) continue;//空行，丢弃
                    int i = l.IndexOf('=');
                    if (i < 1) continue;//空key丢弃
                    string k = l.Substring(0, i);
                    if (k.Trim().Length == 0) continue;
                    string v = l.Substring(i + 1).Trim();
                    _configs[k] = v;
                }
            }

            if (!_configs.ContainsKey(key)) return string.Empty;
            string tmp = _configs[key];
            if (tmp == null) return string.Empty;
            return tmp;
        }
    }
}
