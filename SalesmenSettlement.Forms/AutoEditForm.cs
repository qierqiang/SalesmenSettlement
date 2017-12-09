using SalesmenSettlement.Model;
using SalesmenSettlement.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Windows.Forms;

namespace SalesmenSettlement.Forms
{
    public partial class AutoEditForm : Form
    {
        //events
        public event EventHandler DataSourceChanged;
        public event FilteringPropertiesEventHandler FilteringProperties;

        //fields
        ViewModelBase _dataSource;

        //properties
        public string ViewModelTypeFullName { get; set; }

        [Browsable(false)]
        public virtual ViewModelBase DataSource
        {
            get => _dataSource;
            set
            {
                if (_dataSource != value)
                {
                    _dataSource = value;
                    OnDataSourceChanged();
                }
            }
        }
        public string Title { get => tslTitle.Text; set => tslTitle.Text = value; }

        //ctor
        public AutoEditForm()
        {
            InitializeComponent();
        }

        public AutoEditForm(ViewModelBase model, string title)
        {
            InitializeComponent();
            this.ViewModelTypeFullName = model.GetType().FullName;
            Title = title;

            //过滤属性并创建控件
            var query = from p in model.GetType().GetProperties()
                        where FilterProperty(p)
                        select p;
            CreateControls(query);
        }

        //protected
        protected virtual void OnDataSourceChanged()
        {
            DataBind();
            DataSourceChanged?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnFilteringProperties(FilteringPropertiesEventArgs e)
        {
            FilteringProperties?.Invoke(this, e);
        }

        //public
        public virtual void DataBind()
        {
            //此时控件应该已经创建好了，根据名称查找并绑定

            if (DataSource != null)
            {
                Type type = (object)DataSource is RealProxy rp ? rp.GetProxiedType() : DataSource.GetType();

                var query = from p in type.GetProperties()
                            where FilterProperty(p)
                            from a in p.GetCustomAttributes(typeof(AutoGenControlAttribute), true).OfType<AutoGenControlAttribute>()
                            let c = this.FindFirstChildControl(c => c.Name == a.EditorType.Name.LowerFirstLetter() + "_" + p.Name)
                            where c != null
                            from ca in c.GetType().GetCustomAttributes(typeof(DefaultPropertyAttribute), true).OfType<DefaultPropertyAttribute>()
                            where ca != null
                            select new
                            {
                                Property = p.Name,              //数据源要绑定的属性名称
                                //Attribute =a,                   //AutoGenControlAttribute
                                Control = c,                    //要绑定的控件
                                DefaultPropertyName = ca.Name,  //要绑定的控件属性名称
                            };

                foreach (var item in query)
                {
                    item.Control.DataBindings.Add(new Binding(item.DefaultPropertyName, DataSource, item.Property, true, DataSourceUpdateMode.OnValidation));
                }
            }
        }

        public virtual void CreateControls(IEnumerable<PropertyInfo> properties)
        {
            //过滤掉不要的属性//错误，已经过滤掉了！
            //properties = properties.Where(p => FilterProperty(p));
            //查找组
            var query = from p in properties
                        from a in p.GetCustomAttributes(typeof(AutoGenControlAttribute), true).OfType<AutoGenControlAttribute>()
                        select a.GroupName;

            foreach (string g in query.Distinct())
            {
                //创建组控件！！！需要Init
                AutoEditGroup group = new AutoEditGroup();
                group.BeginInit();
                group.Title = g ?? "";
                group.ViewModelTypeFullName = ViewModelTypeFullName;
                group.DataSource = DataSource;
                group.Dock = DockStyle.Top;
                group.FilteringProperties += (object sender, FilteringPropertiesEventArgs e) =>
                {
                    //用组名过滤属性
                    if (!e.Ignored)
                    {
                        var queryGroupname = from attrib in e.PropertyInfo.GetCustomAttributes(typeof(AutoGenControlAttribute), true).OfType<AutoGenControlAttribute>()
                                             select attrib.GroupName;
                        string gName = queryGroupname.FirstOrDefault() ?? "";
                        e.Ignored = !string.Equals(g ?? "", gName, StringComparison.CurrentCultureIgnoreCase);
                    }
                };
                group.EndInit();
                group.Parent = this;
                group.BringToFront();
            }
        }

        //private
        private bool FilterProperty(PropertyInfo property)
        {
            FilteringPropertiesEventArgs e = new FilteringPropertiesEventArgs(property);
            OnFilteringProperties(new FilteringPropertiesEventArgs(property));
            return !e.Ignored;
        }























        private void bSave_Click(object sender, EventArgs e)
        {
            //Submit();
        }

        private void bClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AutoEditForm_Load(object sender, EventArgs e)
        {

        }

        private void AutoEditForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        //public virtual void AutoGenerateControls()
    }
}
