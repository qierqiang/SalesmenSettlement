using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SalesmenSettlement.Utility;

namespace ModelGenerator
{
    public partial class Form1 : Form
    {
        Database _database;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DisplayTables();
        }

        private void DisplayTables()
        {
            _database = new Database(textBox1.Text);
            string sql = "SELECT [name] FROM [sysobjects] WHERE [xtype]='u'";
            comboBox1.DataSource = _database.GetColumnValues<string>(sql).ToArray();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sql = string.Format(@"
            SELECT
            column_name, is_nullable, data_type, character_maximum_length
            FROM information_schema.columns
            WHERE table_name = '{0}'",
            comboBox1.Text);
            DataTable dt = _database.GetDataTable(sql);

            StringBuilder sb = new StringBuilder("using System;\r\nusing SalesmenSettlement.Utility;\r\n\r\nnamespace SalesmenSettlement.Model\r\n{\r\n\tpublic class ");
            sb.AppendLine(comboBox1.Text + " : NotifyPropertyChanged\r\n\t{");
            foreach (DataRow r in dt.Rows)
            {
                sb.AppendLine("\t\tpublic " + _typeMap[r[2].ToString()] + " " + r[0] + " { get; set; }");
            }
            sb.AppendLine("\t}\r\n}");
            richTextBox1.Text = sb.ToString();
        }

        static Dictionary<string, string> _typeMap = new Dictionary<string, string>();

        static Form1()
        {
            _typeMap.Add("bigint", "long");
            _typeMap.Add("binary", "object");
            _typeMap.Add("bit", "bool");
            _typeMap.Add("char", "string");
            _typeMap.Add("datetime", "DateTime");
            _typeMap.Add("decimal", "decimal");
            _typeMap.Add("float", "decimal");
            _typeMap.Add("image", "object");
            _typeMap.Add("int", "int");
            _typeMap.Add("money", "decimal");
            _typeMap.Add("nchar", "string");
            _typeMap.Add("ntext", "string");
            _typeMap.Add("numeric", "decimal");
            _typeMap.Add("nvarchar", "string");
            _typeMap.Add("real", "decimal");
            _typeMap.Add("smalldatetime", "DateTime");
            _typeMap.Add("sqlint", "int");
            _typeMap.Add("sqlmoney", "decimal");
            _typeMap.Add("sql_variant", "object");
            _typeMap.Add("text", "string");
            _typeMap.Add("timestamp", "Byte[]");
            _typeMap.Add("tinyint", "int");
            _typeMap.Add("uniqueidentifier", "Guid");
            _typeMap.Add("varbinary", "object");
            _typeMap.Add("varchar", "string");
            _typeMap.Add("xml", "string");
        }
    }
}
