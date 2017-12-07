using SalesmenSettlement;
using SalesmenSettlement.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ModelGenerator
{
    public class ModelGenerator
    {
        Database _db;

        public ModelGenerator(string cnn)
        {
            _db = new Database(cnn);
        }

        public string[] GetTableNames()
        {
            string sql = "SELECT name FROM sysobjects WHERE xtype='u'";
            return _db.GetColumnValues<string>(sql).ToArray();
        }

        public ColumnInfoModel[] GetColumns(string tableName)
        {
            if (tableName.IsNullOrWhiteSpace())
            {
                throw new ArgumentException("message", nameof(tableName));
            }

            string sql = @"
            SELECT
	            t1.column_name AS 'Name', CONVERT(bit, t3.isnullable) AS 'Nullable',
	            t1.data_type AS 'DbType', ISNULL(t1.character_maximum_length,-1) AS 'Length', ISNULL(t1.numeric_scale,0) AS 'Precision',
	            ISNULL(t2.value, '') AS 'Description', CONVERT(bit,t3.colstat) AS 'AutoGenerate'
            FROM information_schema.columns t1
            LEFT JOIN sys.extended_properties t2
	            ON t2.major_id=OBJECT_ID(t1.table_name)
	            AND t2.minor_id=t1.ordinal_position
	            AND t2.name='MS_Description'
	            AND t2.class_desc='OBJECT_OR_COLUMN'
            LEFT JOIN syscolumns t3
	            ON t3.id=OBJECT_ID(t1.table_name)
	            AND t3.name=t1.column_name
            WHERE t1.table_name=@p0";

            DataTable dt = _db.GetDataTable(new SqlCommand(sql), new SqlParameter("@p0", tableName));
            ColumnInfoModel[] result = new ColumnInfoModel[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow r = dt.Rows[i];
                ColumnInfoModel col = new ColumnInfoModel();

                foreach (var p in typeof(ColumnInfoModel).GetProperties())
                {
                    //bool 要convert
                    p.SetValue(col, p.GetType() == typeof(bool) ? Convert.ToBoolean(r[p.Name]) : r[p.Name], null);
                }

                result[i] = col;
            }

            return result;
        }

        public string GetModelTemplate()
        {
            string fileName = AppDomain.CurrentDomain.BaseDirectory + "ModelClassTemplate.txt";
            return File.ReadAllText(fileName);
        }

        public string WriteClass(string tableName, ColumnInfoModel[] cols)
        {
            //加载模板
            string propertyTemplate = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "PropertyTemplate.txt");
            List<string> propList = new List<string>();
            //替换参数
            foreach (var c in cols)
            {
                Type csType = GetTypeFromDbType(c.DbType, c.Nullable);
                string privateField = "_" + c.Name.Substring(0, 1).ToLower() + c.Name.Substring(1);
                string description = c.Description.IsNullOrWhiteSpace() ? "" :
                    "/// <summary>\r\n\t\t/// {0}\r\n\t\t/// </summary>".FormatWith(c.Description);
                string attributes = GetAttributes(c, csType);
                StringBuilder sb = new StringBuilder(propertyTemplate);
                sb.Replace("@type", csType.IsGenericType ? csType.GetGenericArguments().First().Name + '?' : csType.Name)
                  .Replace("@privateField", privateField)
                  .Replace("@description", description)
                  .Replace("@attributes", attributes)
                  .Replace("@name", c.Name);

                //删除多余空行
                sb.Replace(">\r\n\t\t\r\n\t\tpublic", ">\r\n\t\tpublic");
                propList.Add(sb.ToString());
            }

            string properties = string.Join("\r\n", propList.ToArray());
            //合成
            string classTemplate = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "ModelClassTemplate.txt");
            StringBuilder stringBuilder = new StringBuilder(classTemplate);
            stringBuilder.Replace("@className", tableName).Replace("@properties", properties);
            return stringBuilder.ToString();
        }

        private string GetAttributes(ColumnInfoModel c, Type csType)
        {
            TypeCode typeCode = Type.GetTypeCode(csType);

            StringBuilder sb = new StringBuilder();

            if (c.Length > 0 && typeCode == TypeCode.String)
            {
                sb.Append("length: " + c.Length + ", ");
            }
            if (c.Precision > 0)
            {
                if (csType.IsGenericType)
                {
                    csType = Nullable.GetUnderlyingType(csType);
                }

                if (typeCode.In(TypeCode.Single, TypeCode.Double, TypeCode.Decimal))
                {
                    sb.Append("precision: " + c.Precision + ", ");
                }
            }
            if (sb.Length > 0)
            {
                sb.Insert(0, "[Validate(").Remove(sb.Length - 2, 2).Append(")]");
            }
            if (c.AutoGenerate)
            {
                sb.Append("[AutoGenerate]");
            }
            return sb.ToString();
        }

        private static Dictionary<string, string> _typeMap;

        [Obsolete]
        private static readonly string[] ValueTypes = new string[]
        {
            "long",
            "bool",
            "DateTime",
            "decimal",
            "double",
            "int",
            "float",
            "short",
            "byte",
            "Guid"
        };

        public static Type GetTypeFromDbType(string dbType, bool nullable)
        {
            if (_typeMap == null)
            {
                XElement root = XElement.Load(AppDomain.CurrentDomain.BaseDirectory + "SqlAndCSharpTypeMapping.xml");

                var custs = (from c in root.Elements("Language")
                             where c.Attribute("From").Value.Equals("SQL") && c.Attribute("To").Value.Equals("C# System Types")
                             select c).ToList();

                _typeMap = new Dictionary<string, string>();

                foreach (XElement node in custs.Elements("Type"))
                {
                    _typeMap.Add(node.Attribute("From").Value, node.Attribute("To").Value);
                }
            }

            string tmp = _typeMap[dbType];
            Type result = Type.GetType(tmp);

            if (nullable && Type.GetType(tmp).IsValueType)
            {
                result = Type.GetType("System.Nullable`1").MakeGenericType(result);
            }

            return result;
        }
    }
}
