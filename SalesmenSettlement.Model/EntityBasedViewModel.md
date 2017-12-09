# Usage
1. 记录引用的Entity类型（及别名）`RefferEntity(EneityBase, string)`
   > 例如：
   > `RefferEntity(typeof(Item),"Material")`
2. 命名时使用“别名+Entity原属性名”作为ViewModel的属性名
   > 如：`MaterialName`会自动对应Item中的Name属性
3. 确定没有重复的名称可以直接使用Entity原属性名作为ViewModel属性的名称
   > 如：属性名`BatchNO`在所有引用过的Entity中只有唯一存在，就可以直接使用
4. 想确保每个属性都没有冲突可以调用`ValidatePropertyNames`检查，没有异常则检查通过
5. 可以添加与Entity无关的属性。
6. 示例
  >
    class Bill: ViewModel
    {
        static Bill()
        {
            RefferEntity(user, "");
            RefferEntity(material, "Material");
            RefferEntity(product, "Product");
            ValidatePropertyNames();
        }

        public string UserName { get; set; }
        public string MaterialName { get; set; }
        public string ProductName { get; set; }
        public string Date { get; set; }
    }

__按顺序添加__