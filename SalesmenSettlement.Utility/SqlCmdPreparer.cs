using System;
using ZOC.Client;
using System.Collections.Generic;

namespace ZOC.IO
{
    /// <summary>准备Sql语句</summary>
    public class SqlCmdPreparer
    {
        /// <summary>生成Insert语句</summary>
        public static string Insert(string tableName, Dictionary<string, object> fieldCollection)
        {
            if (string.IsNullOrEmpty(tableName) || tableName.Trim().Length == 0)
                throw new ArgumentNullException("tableName");

            if (fieldCollection == null || fieldCollection.Count < 1)
                throw new ArgumentNullException("fieldCollection");

            string result = string.Format("INSERT INTO [{0}] ", tableName);
            string fields = string.Empty;
            string values = string.Empty;

            foreach (KeyValuePair<string, object> item in fieldCollection)
            {
                if (IsNullOrBlank(item.Key))
                    throw new Exception("存在空名称的字段");

                string val = GetValueString(item.Value);
                fields += "[" + item.Key + "], ";
                values += val + ", ";
            }

            fields = fields.Remove(fields.Length - 2);
            values = values.Remove(values.Length - 2);
            result += "(" + fields + ") VALUES (" + values + ")";
            return result;
        }

        /// <summary>生成Update语句</summary>
        public static string Update(string tableName, Dictionary<string, object> fieldCollection)
        {
            if (string.IsNullOrEmpty(tableName) || tableName.Trim().Length == 0)
                throw new ArgumentNullException("tableName");

            if (fieldCollection == null || fieldCollection.Count < 1)
                throw new ArgumentNullException("fieldCollection");

            string result = string.Format("UPDATE [{0}] SET ", tableName);

            foreach (KeyValuePair<string, object> item in fieldCollection)
            {
                string value = GetValueString(item.Value);
                result += "[" + item.Key + "]=" + value + ", ";
            }

            result = result.Remove(result.Length - 2);
            return result;
        }

        internal static bool IsNullOrBlank(string source)
        {
            if (string.IsNullOrEmpty(source))
                return true;

            if (source.Trim().Length == 0)
                return true;

            return false;
        }
        internal static string ToString(object source)
        {
            if (source == null)
                return string.Empty;
            else
                return source.ToString();
        }
        static string GetValueString(object value)
        {
            if (value == null || value == DBNull.Value)
                return "NULL";

            Type type = value.GetType();

            if (type == typeof(string))
            {
                string s = (string)value;

                if (s == null)
                    return "NULL";
                else if (s.Trim().Length == 0)
                    return "''";
                else
                    return s;
            }

            if (type == typeof(bool))
            {
                bool b = (bool)value;

                if (b)
                    return "1";
                else
                    return "0";
            }

            if (type == typeof(DateTime))
            {
                DateTime d = (DateTime)value;

                if (d <= DateTime.Parse("1900-01-01") || d >= DateTime.Parse("2999-12-1"))
                    return "NULL";
                else if ((d.Hour == 0 || d.Hour == 12) && d.Minute == 0 && d.Second == 0)
                    return "'" + d.ToString("yyyy-MM-dd") + "'";
                else
                    return "'" + d.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }

            if (type.IsNumeric())
                return value.ToString();

            throw new NotSupportedException();
        }

        //internal static bool IsNumeric(string source)
        //{
        //    if (IsNullOrBlank(source))
        //        return false;

        //    foreach (char c in source)
        //        if (!char.IsDigit(c))
        //            return false;

        //    return true;
        //}
    }
}
