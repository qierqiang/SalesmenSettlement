using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using SalesmenSettlement.Utility;

namespace SalesmenSettlement.LocalService
{
    public class AppConfig
    {
        private static AppConfig _instance;
        private string _appConfigFile = AppDomain.CurrentDomain.BaseDirectory + "appconfig.txt";
        private Dictionary<string, string> _configs;

        public string DatabaseConnectionString { get; set; }

        public string AppName { get; set; }

        public string Company { get; set; }

        public string Version { get; set; }

        public string DefaultPwdMD5 { get; set; }

        private AppConfig() { }

        private string Read(string key)
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

        public void ReadAll()
        {
            DatabaseConnectionString = Read("连接字符串");
            AppName = Read("系统名称");
            Company = Read("公司名称");
            Version = Read("版本号");
            DefaultPwdMD5 = Read("默认密码MD5");
        }

        public static AppConfig GetInstance()
        {
            if (_instance == null)
            {
                _instance = new AppConfig();
                _instance.ReadAll();
            }
            return _instance;
        }
    }
}
