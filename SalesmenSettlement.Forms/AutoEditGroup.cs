using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SalesmenSettlement.Utility;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Remoting.Proxies;

namespace SalesmenSettlement.Forms
{
    public partial class AutoEditGroup : UserControl, ISupportInitialize
    {
        //events
        public event EventHandler DataSourceChanged;
        public event FilteringPropertiesEventHandler FilteringProperties;

        //fields
        private ViewModelBase _dataSource;
        private int _rowCount = 0;
        private bool _expanded = true;

        // * * * 事件 FiltingProperties，在EndInit触发
        // * * ctor 设计器不支持 或 需要派生新类型
        // init 设计时不能指定过滤条件
        // property 设计时不能指定过滤条件
        // * override 需要派生新类型
        // load 设计时看不到效果
        //

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

        public string Title
        {
            get
            {
                return lTitle.Text;
            }
            set { lTitle.Text = value; }
        }

        //ctor
        public AutoEditGroup()
        {
            InitializeComponent();
        }

        //protected
        protected virtual void DataBind()
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
        public void CreateControls(IEnumerable<PropertyInfo> properties)
        {
            var query = from p in properties
                        where FilterProperty(p)
                        from a in p.GetCustomAttributes(typeof(AutoGenControlAttribute), true).OfType<AutoGenControlAttribute>()
                        select new KeyValuePair<PropertyInfo, AutoGenControlAttribute>(p, a);

            CreateEditControl(query);
        }

        //private
        private void CreateEditControl(IEnumerable<KeyValuePair<PropertyInfo, AutoGenControlAttribute>> infos)
        {
            TableLayoutPanel layoutPanel = NewRow();

            foreach (var info in infos)
            {
                //布局分为两块，左边和右边。
                //如果左边被占用就放右边，右边也被占用就另起一行。
                int colIndex = 0;
                //左
                if (layoutPanel.GetControlFromPosition(colIndex, 0) != null)
                {
                    //右 前提没有强制另起一行
                    if (info.Value.BeginNewRow)
                    {
                        layoutPanel = NewRow();
                    }
                    else
                    {
                        colIndex = 3;
                        if (layoutPanel.GetControlFromPosition(colIndex, 0) != null)
                        {
                            //都占用就用新行的左边
                            layoutPanel = NewRow();
                            colIndex = 0;
                        }
                    }
                }
                //TODO: 长控件独占一行

                // create Editor
                Control editor = (Control)Activator.CreateInstance(info.Value.EditorType);
                editor.Name = info.Value.EditorType.Name.LowerFirstLetter() + "_" + info.Key.Name;
                editor.Dock = DockStyle.Fill;
                editor.Margin = new Padding(10, 12, 80, 10);
                editor.Enabled = info.Value.Enabled;
                //read only
                var query = (from p in info.Value.EditorType.GetProperties() where p.Name == "ReadOnly" && p.CanWrite select p).FirstOrDefault();
                if (query != null)
                {
                    query.SetValue(editor, info.Value.ReadOnly, null);
                }

                editor.Parent = layoutPanel;
                layoutPanel.SetColumn(editor, colIndex + 1);
                layoutPanel.SetRow(editor, 0);

                // create Label
                Label label = new Label
                {
                    Name = "lable_" + info.Key.Name,
                    AutoSize = false,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleRight,
                    Margin = new Padding(10, 11, 3, 11),
                    Text = info.Value.DisplayName ?? info.Key.Name,
                    Parent = layoutPanel,
                    Tag = editor,
                };
                layoutPanel.SetColumn(label, colIndex);
                layoutPanel.SetRow(label, 0);

                label.Click += Label_Click;
            }
        }

        private void Label_Click(object sender, EventArgs e)
        {
            if (sender is Label l && l.Tag is Control c && c.CanFocus)
            {
                c.Focus();
            }
        }

        private TableLayoutPanel NewRow()
        {
            Panel panel = new Panel
            {
                Name = "rowPanel" + _rowCount,
                Size = new Size(800, 46),
            };

            TableLayoutPanel layoutPanel = new TableLayoutPanel
            {
                Name = "layoutPanel" + _rowCount,
                RowCount = 1,
                ColumnCount = 5,
                Dock = DockStyle.Fill,
            };

            layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            layoutPanel.Parent = panel;
            panel.Parent = flowPanel;

            _rowCount++;

            return layoutPanel;
        }

        private void Title_DoubleClick(object sender, EventArgs e)
        {
            _expanded = !_expanded;
            var query = from p in flowPanel.Controls.OfType<Control>() where p.Name.StartsWith("rowPanel") select p;
            query.ToList().ForEach(c => c.Visible = _expanded);
            lExpandStatus.Text = _expanded ? "6" : "5";
        }

        private bool FilterProperty(PropertyInfo property)
        {
            FilteringPropertiesEventArgs e = new FilteringPropertiesEventArgs(property);
            OnFilteringProperties(e);
            return !e.Ignored;
        }

        #region ISupportInitialize

        public void BeginInit()
        {
            SuspendLayout();
        }

        public void EndInit()
        {
            //根据ViewModelTypeFullName，查找Type
            if (ViewModelTypeFullName.IsNullOrWhiteSpace())
            {
                return;
            }
            Type type = Type.GetType(ViewModelTypeFullName, true);
            //根据Type过滤属性并创建控件

            var query = from p in type.GetProperties()
                        where FilterProperty(p)
                        select p;
            CreateControls(query);
            ResumeLayout(true);
        }

        #endregion
    }

    public class FilteringPropertiesEventArgs : EventArgs
    {
        public PropertyInfo PropertyInfo { get; private set; }

        public bool Ignored { get; set; }

        public FilteringPropertiesEventArgs(PropertyInfo property, bool ignored = false)
        {
            PropertyInfo = property;
            Ignored = ignored;
        }
    }

    public delegate void FilteringPropertiesEventHandler(object sender, FilteringPropertiesEventArgs e);
}
