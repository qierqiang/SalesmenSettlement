using System;
using System.Data;
using System.Data.OleDb;

namespace ZOC.IO
{
   /// <summary>Access数据库操作类</summary>
   public class Access : Database
   {
       OleDbConnection _cnn;

       /// <summary>新实例</summary>
       public Access() : base() { }

       /// <summary>新实例</summary>
       public Access(string fileName) : base(string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};", fileName)) { }
       //public Access(string fileName) : base(string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};", fileName)) { }

       /// <summary>新实例</summary>
       public Access(OleDbConnection connection) : base(connection) { }
       
       /// <summary>与数据库的连接Connection实例</summary>
       public override IDbConnection Connection
       {
           get { return _cnn; }
           set
           {
               //if (!(value is OleDbConnection))
               //    throw new Exception("所给的值不是有效的OleDbConnection.");

               OleDbConnection cnn = (OleDbConnection)value;

               if (_cnn != cnn)
               {
                   if (_cnn != null)
                   {
                       if (_cnn.State != ConnectionState.Closed)
                           _cnn.Close();

                       _cnn.Dispose();
                   }

                   _cnn = cnn;
               }
           }
       }

       /// <summary>数据库类型</summary>
       public override DatabaseType DatabaseType
       {
           get { return DatabaseType.Access; }
       }
   }
}