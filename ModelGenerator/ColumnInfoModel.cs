using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelGenerator
{
    public class ColumnInfoModel
    {
        public string Name { get; set; }
        public bool Nullable { get; set; }
        public string DbType { get; set; }
        public int Length { get; set; }
        public int Precision { get; set; }
        public string Description { get; set; }
        public bool AutoGenerate { get; set; }
    }
}
