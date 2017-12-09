using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace SalesmenSettlement
{
    public static class CodeHelper
    {
        //string
        public static string FormatWith(this string source, params object[] args) => string.Format(source, args);

        public static string LowerFirstLetter(this string source) => source.Substring(0, 1).ToLower() + source.Substring(1);

        public static bool IsNullOrEmpty(this string source) => string.IsNullOrEmpty(source);

        public static bool IsNullOrBlank(this string source) => source.IsNullOrWhiteSpace();

        public static bool IsNullOrWhiteSpace(this string source) => string.IsNullOrEmpty(source) || source.Trim().Length == 0;

        public static string GetMD5(this string source)
        {
            source = source == null ? string.Empty : source;
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(Encoding.Default.GetBytes(source));// Encoding.UTF8.GetBytes(source));
            return BitConverter.ToString(result).Replace("-", "").ToUpper();//Encoding.UTF8.GetString(result).ToUpper();
        }

        public static string Encrypt(this string source, string encryptKey)
        {
            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();   //实例化加/解密类对象   

            byte[] key = new byte[8], iv = new byte[8]; //参数1，长度24，参数2长度8
            //用0填充
            for (int i = 0; i < 8; i++)
            {
                key[i] = 0;
                iv[i] = 0;
            }
            //take24
            for (int i = 0; i < 8 && i < encryptKey.Length; i++)
            {
                key[i] = Convert.ToByte(encryptKey[i]);
            }
            //reserve take 8
            char[] tmp = encryptKey.ToCharArray().Reverse().ToArray();
            for (int i = 0; i < 8 && i < tmp.Length; i++)
            {
                iv[i] = Convert.ToByte(tmp[i]);
            }

            byte[] data = Encoding.Unicode.GetBytes(source);//定义字节数组，用来存储要加密的字符串  

            MemoryStream mStream = new MemoryStream(); //实例化内存流对象      

            //使用内存流实例化加密流对象   

            CryptoStream cStream = new CryptoStream(mStream, descsp.CreateEncryptor(key, iv), CryptoStreamMode.Write);

            cStream.Write(data, 0, data.Length);  //向加密流中写入数据      

            cStream.FlushFinalBlock();              //释放加密流      

            return Convert.ToBase64String(mStream.ToArray());//返回加密后的字符串  
        }

        public static string Decrypt(this string source, string encryptKey)
        {
            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();   //实例化加/解密类对象    

            byte[] key = new byte[8], iv = new byte[8]; //参数1，长度24，参数2长度8
            //用0填充
            for (int i = 0; i < 8; i++)
            {
                key[i] = 0;
                iv[i] = 0;
            }
            //take24
            for (int i = 0; i < 8 && i < encryptKey.Length; i++)
            {
                key[i] = Convert.ToByte(encryptKey[i]);
            }
            //reserve take 8
            char[] tmp = encryptKey.ToCharArray().Reverse().ToArray();
            for (int i = 0; i < 8 && i < tmp.Length; i++)
            {
                iv[i] = Convert.ToByte(tmp[i]);
            }

            byte[] data = Convert.FromBase64String(source);//定义字节数组，用来存储要解密的字符串  

            MemoryStream MStream = new MemoryStream(); //实例化内存流对象      

            //使用内存流实例化解密流对象       
            CryptoStream CStream = new CryptoStream(MStream, descsp.CreateDecryptor(key, iv), CryptoStreamMode.Write);

            CStream.Write(data, 0, data.Length);      //向解密流中写入数据     

            CStream.FlushFinalBlock();               //释放解密流      

            return Encoding.Unicode.GetString(MStream.ToArray());       //返回解密后的字符串  
        }

        //<T>
        public static string ToJson<T>(this T source) where T : class => JsonConvert.SerializeObject(source);

        public static bool In<T>(this T source, params T[] collection) => collection.Contains(source);

        //object
        public static bool IsNullOrDbNull(this object source) => source == null || source == DBNull.Value;

        public static bool In(this object source, params object[] collection) => collection.Contains(source);

        //control
        public static void ShowError(this ErrorProvider provider, Control ctrl, string error)
        {
            provider.SetError(ctrl, error);
            ctrl.Focus();
            EventHandler action = null;
            action = (object sender, EventArgs e) =>
             {
                 provider.Clear();
                 ctrl.TextChanged -= action;
             };
            ctrl.TextChanged += action;
        }

        public static Control FindFirstChildControl(this Control container, Func<Control, bool> filter)
        {
            foreach (Control c in container.Controls)
            {
                if (filter(c))
                    return c;

                Control result = FindFirstChildControl(c, filter);
                if (result != null)
                    return result;
            }

            return null;
        }
    }
}
