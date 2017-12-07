using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SalesmenSettlement.Utility;
using System.Xml.Linq;
using System.Reflection;
using System.IO;

namespace ModelGenerator
{
    public partial class MainForm : Form
    {
        Database _database;

        public MainForm()
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
                sb.AppendLine("\t\tpublic " + GetTypeFromDbType(r[2].ToString(), string.Equals(r[1].ToString(), "YES", StringComparison.CurrentCultureIgnoreCase)) + " " + r[0] + " { get; set; }");
            }
            sb.AppendLine("\t}\r\n}");
            richTextBox1.Text = sb.ToString();
        }

        static Dictionary<string, string> _typeMap;

        static readonly string[] ValueTypes = new string[]
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

        public static string GetTypeFromDbType(string dbType, bool nullable)
        {
            if (_typeMap == null)
            {
                XElement root = XElement.Load(AppDomain.CurrentDomain.BaseDirectory + "SqlAndCSharpTypeMapping.xml");

                var custs = (from c in root.Elements("Language")
                             where c.Attribute("From").Value.Equals("SQL") && c.Attribute("To").Value.Equals("C#")
                             select c).ToList();

                _typeMap = new Dictionary<string, string>();

                foreach (XElement node in custs.Elements("Type"))
                {
                    _typeMap.Add(node.Attribute("From").Value, node.Attribute("To").Value);
                }
            }

            string tmp = _typeMap[dbType];

            if (nullable && ValueTypes.Contains(tmp))
            {
                tmp += '?';
            }

            return tmp;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //    FolderBrowserDialog dialog = new FolderBrowserDialog { ShowNewFolderButton = true };

            //    if (dialog.ShowDialog() != DialogResult.OK)
            //        return;

            //    string dir = dialog.SelectedPath;

            //string dir = @"C:\Users\apple\Desktop\cs";
            string dir = @"C:\Users\Snokye\Desktop";

            ModelGenerator generator = new ModelGenerator(textBox1.Text);
            string[] tableNames = generator.GetTableNames();
            foreach (string table in tableNames)
            {
                string content = generator.WriteClass(table, generator.GetColumns(table));
                File.WriteAllText(dir + "\\" + table + ".cs", content);
            }

            MessageBox.Show("Done!");
        }
    }
}
