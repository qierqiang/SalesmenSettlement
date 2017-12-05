using System.ComponentModel;
using System.Data;
using System.Threading;
using System;

namespace ZOC.IO
{
    /// <summary>侦听数据库记录更改通知，并在据库记录更改更改时引发事件。</summary>
    public class DbWatcher : Component
    {
        bool _fetched = false; // 是否已经查过一次
        object _lastReuslt; // 储存上次结果
        Thread _thread; // 监视线程
        Database _db;

        /// <summary>新实例</summary>
        public DbWatcher(Database db, IDbCommand watchCommand)
        {
            if (db == null)
                throw new ArgumentNullException("db");

            if (db.Connection == null)
                throw new Exception("db的Connection属性尚未初始化");

            if (watchCommand == null)
                throw new ArgumentNullException("watchCommand");

            if (string.IsNullOrEmpty(watchCommand.CommandText))
                throw new Exception("watchCommand的CommandText属性尚未初始化");

            _db = db;
            _db.KeepConnectionOpen = true;
            this.Command = watchCommand;

            this.Interval = 3000;
            _thread = new Thread((ThreadStart)Watch);
            _thread.IsBackground = true;
        }

        /// <summary>新实例</summary>
        public DbWatcher(DatabaseType dbType, string dbConnection, IDbCommand watchCommand)
            : this(Database.CreateDatabase(dbType, dbConnection), watchCommand) { }

        /// <summary>对数据库的轮询时间间隔</summary>
        [DefaultValue(3000)]
        public int Interval { get; set; }

        /// <summary>要执行的命令</summary>
        public IDbCommand Command { get; set; }

        /// <summary>当监视的内容被更新时发生</summary>
        public event DbChangeEventHandler Updated;
        /// <summary>当监视的内容被创建时发生</summary>
        public event DbChangeEventHandler Inserted;
        /// <summary>当监视的内容被删除时发生</summary>
        public event DbChangeEventHandler Deleted;
        /// <summary>当监视的内容被改变时发生</summary>
        public event DbChangeEventHandler Changed;
        /// <summary>当执行查询语句出错时发生</summary>
        public event System.IO.ErrorEventHandler Error;

        /// <summary>开始监控</summary>
        public void StartWatching()
        {
            if (_thread.IsAlive)
                throw new Exception("监控线程已经启动");

            _thread.Start();
        }
        /// <summary>开始监控</summary>
        public void TryStartWatching()
        {
            if (!_thread.IsAlive)
                _thread.Start();
        }

        /// <summary></summary>
        protected override void Dispose(bool disposing)
        {
            _thread = null;
            _db.Dispose();

            base.Dispose(disposing);
        }
        /// <summary>停止监控</summary>
        public void StopWatching()
        {
            _thread.Abort();
        }
        /// <summary>监控</summary>
        protected virtual void Watch()
        {
            while (true)
            {
                try
                {
                    object value;

                    try { value = _db.ExecuteScalar(Command); }
                    catch (Exception ex)
                    {
                        OnError(new System.IO.ErrorEventArgs(ex));
                        continue;
                    }

                    if (!_fetched || object.Equals(_lastReuslt, value))
                    {
                        _lastReuslt = value;
                        _fetched = true;
                        continue;
                    }

                    try
                    {
                        DbChangeType ct;
                        if (_lastReuslt == null && value != null)
                        {
                            ct = DbChangeType.Insert;
                            OnInserted(new DbChangeEventArgs(ct, this.Command));
                        }
                        else if (_lastReuslt != null && value == null)
                        {
                            ct = DbChangeType.Delete;
                            OnDeleted(new DbChangeEventArgs(ct, this.Command));
                        }
                        else
                        {
                            ct = DbChangeType.Update;
                            OnUpdated(new DbChangeEventArgs(ct, this.Command));
                        }

                        OnChanged(new DbChangeEventArgs(ct, this.Command));
                    }
                    finally
                    {
                        _lastReuslt = value;
                        _fetched = true;
                    }
                }
                finally
                {
                    Thread.Sleep(this.Interval);
                }
            }
        }
        /// <summary>引发Updated事件</summary>
        protected void OnUpdated(DbChangeEventArgs e)
        {
            if (this.Updated != null)
                this.Updated.Invoke(this, e);
        }
        /// <summary>引发Inserted事件</summary>
        protected void OnInserted(DbChangeEventArgs e)
        {
            if (this.Inserted != null)
                this.Inserted.Invoke(this, e);
        }
        /// <summary>引发Deleted事件</summary>
        protected void OnDeleted(DbChangeEventArgs e)
        {
            if (this.Deleted != null)
                this.Deleted.Invoke(this, e);
        }
        /// <summary>引发Changed事件</summary>
        protected virtual void OnChanged(DbChangeEventArgs e)
        {
            if (this.Changed != null)
                this.Changed.Invoke(this, e);
        }
        /// <summary>引发Error事件</summary>
        protected void OnError(System.IO.ErrorEventArgs e)
        {
            if (this.Error != null)
                this.Error.Invoke(this, e);
        }
        /// <summary>等待一个指定类型的改变发生</summary>
        [Obsolete]
        public DbChangedResult WaitForChanged(DbChangeType changeType)
        {
            throw new NotImplementedException();
        }
        /// <summary>等待一个指定类型的改变发生</summary>
        [Obsolete]
        public DbChangedResult WaitForChanged(DbChangeType changeType, int timeout)
        {
            throw new NotImplementedException();
        }
    }
}
