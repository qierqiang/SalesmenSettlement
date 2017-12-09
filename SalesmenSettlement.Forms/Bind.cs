using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SalesmenSettlement.Forms
{
    public static class Bind
    {
        public static void CreateControls(TableLayoutPanel container, IEnumerable<PropertyInfo> properties)
        {
            foreach (PropertyInfo p in properties)
            {
                Attribute[] attributes = p.GetCustomAttributes(true).Cast<Attribute>().ToArray();
                var queryAgc = from a in attributes where a is AutoGenControlAttribute select (AutoGenControlAttribute)a;

                if (queryAgc.Any())
                {
                    var a = queryAgc.First();
                    // create Label
                    string labelName = "lable_" + p.Name;
                    Label label = new Label { Name = labelName, AutoSize = false, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight, Margin = new Padding(10, 11, 3, 11), Text = a.DisplayName ?? p.Name };


                    string editorName = a.EditorType.Name.LowerFirstLetter() + "_" + p.Name;


                }
            }


            //query = from c in "fdsfdsfdfds" select typeof(string).GetProperty("");
            //Label label = new Label { AutoSize = false}
        }

        //static string Get EditorType.Name.LowerFirstLetter() + "_" + propertyName;
    }
}
