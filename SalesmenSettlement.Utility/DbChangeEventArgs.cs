using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ZOC.IO
{
    /// <summary>更改类型</summary>
    [Flags]public enum DbChangeType
    {
        /// <summary></summary>
        Update = 1,
        /// <summary></summary>
        Insert = 2,
        /// <summary></summary>
        Delete = 4,
    }

    /// <summary>数据改变事件参数</summary>
    public class DbChangeEventArgs : EventArgs
    {
        /// <summary>新实例</summary>
        public DbChangeEventArgs(DbChangeType changeType, IDbCommand cmd)
        {
            if (cmd == null)
                throw new ArgumentNullException("cmd");

            this.ChangeType = changeType;
            this.WatchCommand = cmd;
        }

        /// <summary>改变的类型</summary>
        public DbChangeType ChangeType { get; private set; }

        /// <summary>能查出监视结果的查询命令</summary>
        public IDbCommand WatchCommand { get; private set; }
    }

    /// <summary>将要处理DbChange事件的方法</summary>
    public delegate void DbChangeEventHandler(object sender, DbChangeEventArgs e);
}
