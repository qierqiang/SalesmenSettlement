using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SalesmenSettlement.Utility
{
    public class LocalUserProfile
    {
        /*
            目录结构：
            ~\Users\
            ~\Users\exampleUser\
            ~\Users\exampleUser\password.dat
            ~\Users\exampleUser\clientform.dat
            ......
        */

        public static readonly string UserProfileDirecotry = AppDomain.CurrentDomain.BaseDirectory + "Users\\";

        public static void Save(string userName, string profileName, object profile)
        {
            if (profile == null)
            {
                throw new ArgumentNullException(nameof(profile));
            }

            string content = profile.ToJson();
            //保存
            SaveContent(userName, profileName, content);
        }
        public static void SaveContent(string userName, string profileName, string content)
        {
            if (userName.IsNullOrWhiteSpace())
            {
                throw new ArgumentException("", nameof(userName));
            }
            if (profileName.IsNullOrWhiteSpace())
            {
                throw new ArgumentException("", nameof(profileName));
            }

            //创建文件夹
            string dir = UserProfileDirecotry + userName + "\\";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            //删除已有文件
            string fileName = dir + profileName + ".dat";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            //保存
            File.WriteAllText(fileName, content.Encrypt(userName));
        }

        public static T GetAnonymousTypeProfile<T>(string userName, string profileName, T example)
        {
            string content = GetProfileContent(userName, profileName);
            if (content.IsNullOrWhiteSpace())
            {
                return default(T);
            }
            //cast
            return JsonConvert.DeserializeAnonymousType<T>(content, example);
        }
        public static T GetProfile<T>(string userName, string profileName)
        {
            string content = GetProfileContent(userName, profileName);
            if (content.IsNullOrWhiteSpace())
            {
                return default(T);
            }
            //cast
            return JsonConvert.DeserializeObject<T>(content);
        }
        public static string GetProfileContent(string userName, string profileName)
        {
            if (userName.IsNullOrWhiteSpace())
            {
                throw new ArgumentException("", nameof(userName));
            }
            if (profileName.IsNullOrWhiteSpace())
            {
                throw new ArgumentException("", nameof(profileName));
            }

            //找文件
            string fileName = UserProfileDirecotry + userName + "\\" + profileName + ".dat";
            if (!File.Exists(fileName))
            {
                return null;
            }

            return File.ReadAllText(fileName).Decrypt(userName);
        }

        public static void Delete(string userName, string profileName)
        {
            if (userName.IsNullOrWhiteSpace())
            {
                throw new ArgumentException("", nameof(userName));
            }
            if (profileName.IsNullOrWhiteSpace())
            {
                throw new ArgumentException("", nameof(profileName));
            }

            string fileName = UserProfileDirecotry + userName + "\\" + profileName + ".dat";
            File.Delete(fileName);
        }
    }
}
