using SalesmenSettlement.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SalesmenSettlement.Model
{
    public abstract class EntityBasedViewModel : ViewModelBase
    {
        Dictionary<string, EntityBase> _dictionary;

        public EntityBasedViewModel(Dictionary<string, EntityBase> dictionary)
        {
            _dictionary = dictionary;
        }

        /// <summary>
        /// 从数据源实体中加载属性值
        /// </summary>
        protected virtual void LoadProperties()
        {
            List<PropertyInfo> thisProperties = this.GetType().GetProperties().Where(p => p.CanWrite).ToList();
            var reffProperties = (from item in _dictionary
                                  from p in item.Value.GetType().GetProperties()
                                  select new { Alias = item.Key + "_" + p.Name, PropertyInfo = p, Entity = item.Value }).ToDictionary(o => o.Alias);

            foreach (PropertyInfo p in thisProperties)
            {
                if (p.Name.IndexOf('_') > -1) // 如：UserInfo_CreatorID
                {
                    if (reffProperties.ContainsKey(p.Name))
                    {
                        var item = reffProperties[p.Name];
                        object value = item.PropertyInfo.GetValue(item.Entity, null);
                        p.SetValue(this, value, null);
                    }
                    else
                    {
                        Console.WriteLine("设置属性值失败：" + p.Name);
                    }
                }
                else // 如：Name
                {
                    string[] names = reffProperties.Keys.Where(k => k.EndsWith(p.Name)).ToArray();

                    if (names.Length > 1)
                        throw new Exception("属性映射关系不明确：" + p.Name);
                    if (names.Length > 0)
                    {
                        string name = names[0];
                        var item = reffProperties[name];
                        object value = item.PropertyInfo.GetValue(item.Entity, null);
                        p.SetValue(this, value, null);
                    }
                }
            }
        }

        //protected virtual void 


        public IEnumerable<string> Stub(params EntityBase[] entities)
        {
            var allEntityTypes = from e in entities select e.GetType();
            HashSet<string> hashSet = new HashSet<string>();

            foreach (var t in allEntityTypes)
            {
                int i = 1;
                string name;
                do { name = t.FullName + "_" + i++; }
                while (hashSet.Contains(name));
                hashSet.Add(name);

                foreach (var p in t.GetProperties())
                {
                    yield return name + "_" + p.Name;
                }
            }
        }

        //private static Dictionary<EntityBase, string> _reffering = new Dictionary<EntityBase, string>();

        //public static void RefferEntity(EntityBase entity, string alias = null)
        //{
        //    if (alias.IsNullOrWhiteSpace())
        //        alias = entity.GetType().Name;
        //    if (_reffering.ContainsKey(alias))
        //        throw new Exception("引用类别的名称有重复:" + alias);
        //    _reffering.Add(alias, entity);
        //}

        //public static void ValidatePropertyNames()
        //{
        //    //UserInfoCreateTime 写成：UserInfo_CreateTime
        //}
    }
}
