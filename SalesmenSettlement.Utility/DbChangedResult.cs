using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ZOC.IO
{
    /// <summary>数据改变结果</summary>
    public class DbChangedResult
    {
        /// <summary>改变类型</summary>
        public DbChangeType ChangeType { get; set; }

        /// <summary>查询的命名</summary>
        public IDbCommand WatchCommand { get; set; }

        /// <summary>监视到变更的时间</summary>
        public DateTime ChangedTime { get; set; }

        /// <summary>上一次的数据</summary>
        public DataTable OldValue { get; set; }

        /// <summary>这一次的数据</summary>
        public DataTable NewValue { get; set; }

        /// <summary>是否是因为超时而结束</summary>
        public bool TimedOut { get; set; }
    }
}
