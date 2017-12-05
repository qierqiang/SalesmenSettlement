using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Linq;
using ADODB;
using System.Collections;
using ZOC.Client;

namespace ZOC.IO
{
    /// <summary>Excel数据操作类</summary>
    public class Excel : Database
    {
        //Microsoft ActiveX Data Objects 2.0 Library
        //Microsoft Excel 11.0 Object Library

        static readonly Missing miss = Missing.Value;
        OleDbConnection _cnn;

        /// <summary>新实例</summary>
        public Excel() : base() { }

        /// <summary>新实例</summary>
        public Excel(string fileName, bool HDR) : base(string.Format("Provider=Microsoft.Jet.OleDb.4.0;data source={0};Extended Properties='Excel 8.0; HDR={1}; IMEX=2'", fileName, HDR ? "YES" : "NO")) { }

        /// <summary>新实例</summary>
        public Excel(OleDbConnection cnn) : base(cnn) { }

        /// <summary>连接实例</summary>
        public override IDbConnection Connection
        {
            get { return _cnn; }
            set
            {
                if (!(value is OleDbConnection))
                    throw new Exception("所给的值不是有效的OleDbConnection.");

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
            get { return DatabaseType.Excel; }
        }

        private static Recordset ConvertDataTableToRecordset(DataTable table)
        {
            Recordset result = new RecordsetClass();

            foreach (DataColumn col in table.Columns)
                result.Fields._Append(col.ColumnName, GetDataType(col.DataType), -1, FieldAttributeEnum.adFldIsNullable);

            result.Open(Missing.Value, Missing.Value, CursorTypeEnum.adOpenUnspecified, LockTypeEnum.adLockUnspecified, -1);

            foreach (DataRow row in table.Rows)
            {
                result.AddNew(Missing.Value, Missing.Value);

                for (int i = 0; i < table.Columns.Count; i++)
                    result.Fields[i].Value = row[i];
            }

            return result;
        }

        private static Recordset ConvertDataGridViewToRecordSet(DataGridView dgv)
        {
            Recordset result = new RecordsetClass();

            foreach (DataGridViewColumn col in dgv.Columns)
                if (col.Visible)
                    result.Fields._Append(col.HeaderText, GetDataType(col.ValueType), -1, FieldAttributeEnum.adFldIsNullable);

            result.Open(miss, miss, CursorTypeEnum.adOpenUnspecified, LockTypeEnum.adLockUnspecified, -1);

            foreach (DataGridViewRow row in dgv.Rows)
            {
                result.AddNew(miss, miss);
                int i = 0;

                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    if (col.Visible)
                    {
                        result.Fields[i].Value = row.Cells[col.Name].Value;
                        i++;
                    }
                }
            }

            return result;
        }

        private static Recordset[] ConvertDataSetToRecordset(DataSet ds)
        {
            Recordset[] result = new Recordset[ds.Tables.Count];

            for (int i = 0; i < result.Length; i++)
                result[i] = ConvertDataTableToRecordset(ds.Tables[i]);

            return result;
        }

        private static DataTypeEnum GetDataType(Type dataType)
        {
            switch (dataType.ToString())
            {
                case "System.Boolean": return DataTypeEnum.adBoolean;
                case "System.Byte": return DataTypeEnum.adUnsignedTinyInt;
                case "System.Char": return DataTypeEnum.adChar;
                case "System.DateTime": return DataTypeEnum.adDate;
                case "System.Decimal": return DataTypeEnum.adDouble;
                case "System.Double": return DataTypeEnum.adDouble;
                case "System.Int16": return DataTypeEnum.adSmallInt;
                case "System.Int32": return DataTypeEnum.adInteger;
                case "System.Int64": return DataTypeEnum.adBigInt;
                case "System.SByte": return DataTypeEnum.adTinyInt;
                case "System.Single": return DataTypeEnum.adSingle;
                case "System.String": return DataTypeEnum.adVarChar;
                case "System.UInt16": return DataTypeEnum.adUnsignedSmallInt;
                case "System.UInt32": return DataTypeEnum.adUnsignedInt;
                case "System.UInt64": return DataTypeEnum.adUnsignedBigInt;
                default: throw new Exception("没有对应的数据类型");
            }
        }

        /// <summary>获取所有工作簿名称</summary>
        public virtual string[] GetSheetNames()
        {
            lock (this)
            {
                OpenConnection();

                try
                {
                    DataTable table = _cnn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                    string[] result = new string[table.Rows.Count];

                    for (int i = 0; i < table.Rows.Count; i++)
                        result[i] = table.Rows[i]["TABLE_NAME"].ToString();

                    return result;
                }
                finally
                {
                    CloseConnection();
                }
            }
        }

        /// <summary>把列序号转换为字母形式</summary>
        public static string GetColumnLabel(int columnIndex)
        {
            string rtnStr = string.Empty;
            while (columnIndex > 0)
            {
                rtnStr = (char)('A' + (columnIndex - 1) % 26) + rtnStr;
                columnIndex = (columnIndex - 1) / 26;
            }
            return rtnStr;
        }

        /// <summary>导出到Excel</summary>
        [Obsolete("已停用")]
        public static void ExportToExcel(DataTable table, string fileName, bool overWrite, string sheetName, bool exportHeaders, bool open)
        {
            if (table == null)
                throw new ArgumentNullException("table");

            if (File.Exists(fileName))
            {
                if (overWrite)
                    File.Delete(fileName);
                else
                    throw new Exception("文件已经存在!");
            }

            Interop.Excel.ApplicationClass app  = new Interop.Excel.ApplicationClass { Visible = false };
            Interop.Excel.Workbooks books       = app.Workbooks;
            Interop.Excel.Workbook book         = books.Add(miss);
            Interop.Excel.Worksheet sheet       = (Interop.Excel.Worksheet)book.ActiveSheet;

            foreach (Interop.Excel.Worksheet s in book.Sheets)
                if (s != sheet)
                    s.Delete();

            if (!string.IsNullOrEmpty(sheetName))
                sheet.Name = sheetName;

            Recordset rs = ConvertDataTableToRecordset(table);

            Interop.Excel.Range range = (Interop.Excel.Range)sheet.Cells[1, 1];

            if (exportHeaders) // 标题行
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    Interop.Excel.Range rangeHeader = sheet.get_Range((object)(GetColumnLabel(i + 1) + "1"), miss);
                    rangeHeader.Value2 = table.Columns[i].Caption;
                }

                range = (Interop.Excel.Range)sheet.Cells[2, 1];
            }

            range.CopyFromRecordset(rs, miss, miss);

            book.SaveAs(fileName, Interop.Excel.XlFileFormat.xlExcel7, miss, miss, miss, miss,
                Interop.Excel.XlSaveAsAccessMode.xlNoChange, miss, miss, miss, miss, miss);

            if (open)
            {
                app.Visible = true;
            }
            else
            {
                book.Close(false, miss, miss);
                books.Close();
                app.Quit();
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(books);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
            GC.Collect();
        }
        /// <summary>导出到Excel</summary>
        [Obsolete("已停用")]
        public static void ExportToExcel(DataGridView dgv, string fileName, bool overWrite, string sheetName, bool exportHeaders, bool open)
        {
            if (dgv == null)
                throw new ArgumentNullException("dgv");

            if (dgv.Columns.Count == 0)
                throw new Exception("所提供的 DataGridView 中不包含任何列.");

            if (File.Exists(fileName))
            {
                if (overWrite)
                    File.Delete(fileName);
                else
                    throw new Exception("文件已经存在!");
            }

            Interop.Excel.Application app = new Interop.Excel.ApplicationClass { Visible = false };
            Interop.Excel.Workbooks books = app.Workbooks;
            Interop.Excel.Workbook book = books.Add(miss);
            Interop.Excel.Worksheet sheet = (Interop.Excel.Worksheet)book.ActiveSheet;

            foreach (Interop.Excel.Worksheet s in book.Sheets)
                if (s != sheet)
                    s.Delete();

            if (!string.IsNullOrEmpty(sheetName))
                sheet.Name = sheetName;

            Recordset rs = ConvertDataGridViewToRecordSet(dgv);

            Interop.Excel.Range range = (Interop.Excel.Range)sheet.Cells[1, 1];

            if (exportHeaders) // 标题行
            {
                int i = 0;
                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    if (col.Visible)
                    {
                        Interop.Excel.Range rangeHeader = sheet.get_Range((object)(GetColumnLabel(i + 1) + "1"), miss);
                        rangeHeader.Value2 = col.HeaderText;
                        i++;
                    }
                }

                range = (Interop.Excel.Range)sheet.Cells[2, 1];
            }

            range.CopyFromRecordset(rs, miss, miss);

            book.SaveAs(fileName, Interop.Excel.XlFileFormat.xlExcel7, miss, miss, miss, miss,
                Interop.Excel.XlSaveAsAccessMode.xlNoChange, miss, miss, miss, miss, miss);

            if (open)
            {
                app.Visible = true;
            }
            else
            {
                book.Close(false, miss, miss);
                books.Close();
                app.Quit();
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(books);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
            GC.Collect();
        }
        /// <summary>导出到Excel</summary>
        [Obsolete("已停用")]
        public static void ExportToExcel(DataSet ds, string fileName, bool overWrite, bool exportHeaders, bool open)
        {
            if (ds == null)
                throw new ArgumentNullException("ds");

            if (File.Exists(fileName))
            {
                if (overWrite)
                    File.Delete(fileName);
                else
                    throw new Exception("文件已经存在!");
            }

            Interop.Excel.Application app = new Interop.Excel.Application { Visible = false };
            Interop.Excel.Workbooks books = app.Workbooks;
            Interop.Excel.Workbook book = books.Add(miss);

            foreach (Interop.Excel.Worksheet item in book.Sheets)
                item.Delete();

            foreach (DataTable dt in ds.Tables)
            {
                Interop.Excel.Worksheet sheet = (Interop.Excel.Worksheet)book.Sheets.Add(miss, miss, miss, miss);

                if (!string.IsNullOrEmpty(dt.TableName))
                    sheet.Name = dt.TableName;

                Recordset rs = ConvertDataTableToRecordset(dt);
                Interop.Excel.Range range = (Interop.Excel.Range)sheet.Cells[1, 1];

                if (exportHeaders) // 标题行
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        Interop.Excel.Range rangeHeader = sheet.get_Range((object)(GetColumnLabel(i + 1) + "1"), miss);
                        rangeHeader.Value2 = dt.Columns[i].Caption;
                    }

                    range = (Interop.Excel.Range)sheet.Cells[2, 1];
                }

                range.CopyFromRecordset(rs, miss, miss);
            }

            book.SaveAs(fileName, Interop.Excel.XlFileFormat.xlExcel7, miss, miss, miss, miss,
                Interop.Excel.XlSaveAsAccessMode.xlNoChange, miss, miss, miss, miss, miss);

            if (open)
            {
                app.Visible = true;
            }
            else
            {
                book.Close(false, miss, miss);
                books.Close();
                app.Quit();
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(books);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
            GC.Collect();
        }
        /// <summary>
        /// 导出到Excel
        /// Copied from APSClientUC.dll::ZOC.Client.EIport.ExportExcel.DataGridViewToExcel()
        /// by yaowangsheng and modified by Snokye 2011-03-11
        /// </summary>
        public static void ExportToExcel(DataGridView dgv)
        {
            if (dgv.Rows.Count == 0)
            {
                MessageBox.Show("没有数据可供导出！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // step 1.
            // Choose a file to save.
            string fileName;

            using (SaveFileDialog dlgSave = new SaveFileDialog())
            {
                dlgSave.Filter = "Execl files (*.xls)|*.xls";
                dlgSave.FilterIndex = 0;
                dlgSave.RestoreDirectory = true;
                dlgSave.Title = "导出文件保存路径";
                dlgSave.FileName = null;

                if (dlgSave.ShowDialog() != DialogResult.OK)
                    return;

                fileName = dlgSave.FileName;
            }

            // step 2.
            // Select all visible columns in dgv into the datatable.
            var dispalyedColumns = (from c in dgv.Columns.OfType<DataGridViewColumn>()
                                    where c.Visible select c).ToList();

            DataTable dt = new DataTable();

            foreach (DataGridViewColumn col in dispalyedColumns)
            {
                DataColumn dtCol = new DataColumn(col.Name, col.ValueType);
                dtCol.Caption = col.HeaderText;
                dt.Columns.Add(dtCol);
            }

            foreach (DataGridViewRow row in dgv.Rows)
            {
                ArrayList values = new ArrayList();

                foreach (DataColumn col in dt.Columns)
                    values.Add(row.Cells[col.ColumnName].Value);

                dt.Rows.Add(values.ToArray());
            }

            // step 3.
            // Write file context.
                if (File.Exists(fileName))
                    File.Delete(fileName);

            using (FileStream fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write))
            using (StreamWriter streamWriter = new StreamWriter(fileStream, System.Text.Encoding.GetEncoding(-0)))
            {
                string strLine = string.Empty;
                char seperatorChar = Convert.ToChar(9);

                foreach (DataColumn col in dt.Columns)
                    strLine += col.Caption + seperatorChar;

                streamWriter.WriteLine(strLine);

                foreach (DataRow row in dt.Rows)
                {
                    strLine = string.Empty;

                    foreach (DataColumn col in dt.Columns)
                        strLine += row[col] + seperatorChar.ToString();

                    streamWriter.WriteLine(strLine);
                }

                streamWriter.Close();
                fileStream.Close();
            }

            MessageBox.Show("数据已经成功导出到：" + fileName, "导出数据完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /// <summary>导出到Excel</summary>
        public static void ExportToExcel(DataTable dt)
        {
            if (dt == null)
                throw new ArgumentNullException("dt");

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("没有数据可供导出！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // step 1.
            // Choose a file to save.
            string fileName;

            using (SaveFileDialog dlgSave = new SaveFileDialog())
            {
                dlgSave.Filter = "Execl files (*.xls)|*.xls";
                dlgSave.FilterIndex = 0;
                dlgSave.RestoreDirectory = true;
                dlgSave.Title = "导出文件保存路径";
                dlgSave.FileName = null;

                if (dlgSave.ShowDialog() != DialogResult.OK)
                    return;

                fileName = dlgSave.FileName;
            }

            // step 2.
            // Write file context.
            if (File.Exists(fileName))
                File.Delete(fileName);

            using (FileStream fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write))
            using (StreamWriter streamWriter = new StreamWriter(fileStream, System.Text.Encoding.GetEncoding(-0)))
            {
                string strLine = string.Empty;
                char seperatorChar = Convert.ToChar(9);

                foreach (DataColumn col in dt.Columns)
                    strLine += col.Caption + seperatorChar;

                streamWriter.WriteLine(strLine);

                foreach (DataRow row in dt.Rows)
                {
                    strLine = string.Empty;

                    foreach (DataColumn col in dt.Columns)
                        strLine += row[col] + seperatorChar.ToString();

                    streamWriter.WriteLine(strLine);
                }

                streamWriter.Close();
                fileStream.Close();
            }

            MessageBox.Show("数据已经成功导出到：" + fileName, "导出数据完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}

#region 注释代码

/*
 /// <summary>Excel数据操作类</summary>
    public class Excel : Database
    {
        static readonly Missing miss = Missing.Value;
        OleDbConnection _cnn;
        
        /// <summary>新实例</summary>
        public Excel() : base() { }

        /// <summary>新实例</summary>
        public Excel(string fileName, bool HDR) : base(string.Format("Provider=Microsoft.Jet.OleDb.4.0;data source={0};Extended Properties='Excel 8.0; HDR={1}; IMEX=2'", fileName, HDR ? "YES" : "NO")) { }

        /// <summary>新实例</summary>
        public Excel(OleDbConnection cnn) : base(cnn) { }

        /// <summary></summary>
        public virtual void Dispose()
        {
            if (_cnn != null)
            {
                if (_cnn.State != System.Data.ConnectionState.Closed)
                    _cnn.Close();

                _cnn.Dispose();
                _cnn = null;
            }
        }

        /// <summary>连接实例</summary>
        public override IDbConnection Connection
        {
            get { return _cnn; }
            set
            {
                if (!(value is OleDbConnection))
                    throw new Exception("所给的值不是有效的OleDbConnection.");

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

        private void CheckConnection()
        {
            if (_cnn == null)
                throw new Exception("");

            if (_cnn.State == System.Data.ConnectionState.Closed)
                _cnn.Open();
        }

        private static Recordset ConvertDataTableToRecordset(DataTable table)
        {
            Recordset result = new RecordsetClass();

            foreach (DataColumn col in table.Columns)
                result.Fields._Append(col.ColumnName, GetDataType(col.DataType), -1, FieldAttributeEnum.adFldIsNullable);

            result.Open(Missing.Value, Missing.Value, CursorTypeEnum.adOpenUnspecified, LockTypeEnum.adLockUnspecified, -1);

            foreach (DataRow row in table.Rows)
            {
                result.AddNew(Missing.Value, Missing.Value);

                for (int i = 0; i < table.Columns.Count; i++)
                    result.Fields[i].Value = row[i];
            }

            return result;
        }

        private static Recordset ConvertDataGridViewToRecordSet(DataGridView dgv)
        {
            Recordset result = new RecordsetClass();

            foreach (DataGridViewColumn col in dgv.Columns)
                if (col.Visible)
                    result.Fields._Append(col.HeaderText, GetDataType(col.ValueType), -1, FieldAttributeEnum.adFldIsNullable);

            result.Open(miss, miss, CursorTypeEnum.adOpenUnspecified, LockTypeEnum.adLockUnspecified, -1);

            foreach (DataGridViewRow row in dgv.Rows)
            {
                result.AddNew(miss, miss);
                int i = 0;

                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    if (col.Visible)
                    {
                        result.Fields[i].Value = row.Cells[col.Name].Value;
                        i++;
                    }
                }
            }

            return result;
        }

        private static Recordset[] ConvertDataSetToRecordset(DataSet ds)
        {
            Recordset[] result = new Recordset[ds.Tables.Count];

            for (int i = 0; i < result.Length; i++)
                result[i] = ConvertDataTableToRecordset(ds.Tables[i]);

            return result;
        }

        private static DataTypeEnum GetDataType(Type dataType)
        {
            switch (dataType.ToString())
            {
                case "System.Boolean":  return DataTypeEnum.adBoolean;
                case "System.Byte":     return DataTypeEnum.adUnsignedTinyInt;
                case "System.Char":     return DataTypeEnum.adChar;
                case "System.DateTime": return DataTypeEnum.adDate;
                case "System.Decimal":  return DataTypeEnum.adDecimal;
                case "System.Double":   return DataTypeEnum.adDouble;
                case "System.Int16":    return DataTypeEnum.adSmallInt;
                case "System.Int32":    return DataTypeEnum.adInteger;
                case "System.Int64":    return DataTypeEnum.adBigInt;
                case "System.SByte":    return DataTypeEnum.adTinyInt;
                case "System.Single":   return DataTypeEnum.adSingle;
                case "System.String":   return DataTypeEnum.adVarChar;
                case "System.UInt16":   return DataTypeEnum.adUnsignedSmallInt;
                case "System.UInt32":   return DataTypeEnum.adUnsignedInt;
                case "System.UInt64":   return DataTypeEnum.adUnsignedBigInt;
                default:                throw new Exception("没有对应的数据类型");
            }
        }

        /// <summary>获取所有工作簿名称</summary>
        public virtual string[] GetSheetNames()
        {
            lock (this)
            {
                CheckConnection();
                DataTable table = _cnn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                string[] result = new string[table.Rows.Count];

                for (int i = 0; i < table.Rows.Count; i++)
                    result[i] = table.Rows[i]["TABLE_NAME"].ToString();
                
                return result;
            }
        }

        /// <summary>把列序号转换为字母形式</summary>
        public static string GetColumnLabel(int columnIndex)
        {
            string rtnStr = string.Empty;
            while (columnIndex > 0)
            {
                rtnStr = (char)('A' + (columnIndex - 1) % 26) + rtnStr;
                columnIndex = (columnIndex - 1) / 26;
            }
            return rtnStr;
        }

        /// <summary>导出到Excel</summary>
        public static void ExportToExcel(DataTable table, string fileName, bool overWrite, string sheetName, bool exportHeaders, bool open)
        {
            if (table == null)
                throw new ArgumentNullException("table");

            if (File.Exists(fileName))
            {
                if (overWrite)
                    File.Delete(fileName);
                else
                    throw new Exception("文件已经存在!");
            }

            Interop.Excel.Application   app =   new Interop.Excel.ApplicationClass { Visible = false };
            Interop.Excel.Workbooks     books = app.Workbooks;
            Interop.Excel.Workbook      book =  books.Add(miss);
            Interop.Excel.Worksheet     sheet = (Interop.Excel.Worksheet)book.ActiveSheet;

            foreach (Interop.Excel.Worksheet s in book.Sheets)
                if (s != sheet)
                    s.Delete();

            if (!string.IsNullOrEmpty(sheetName))
                sheet.Name = sheetName;

            Recordset rs = ConvertDataTableToRecordset(table);

            Interop.Excel.Range range = (Interop.Excel.Range)sheet.Cells[1, 1];

            if (exportHeaders) // 标题行
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    Interop.Excel.Range rangeHeader = sheet.get_Range((object)(GetColumnLabel(i + 1) + "1"), miss);
                    rangeHeader.Value2 = table.Columns[i].Caption;
                }

                range = (Interop.Excel.Range)sheet.Cells[2, 1];
            }

            range.CopyFromRecordset(rs, miss, miss);

            book.SaveAs(fileName,Interop.Excel.XlFileFormat.xlExcel7, miss, miss, miss, miss,
                Interop.Excel.XlSaveAsAccessMode.xlNoChange, miss, miss, miss, miss, miss);

            if (open)
            {
                app.Visible = true;
            }
            else
            {
                book.Close(false, miss, miss);
                books.Close();
                app.Quit();
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(books);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
            GC.Collect();
        }
        /// <summary>导出到Excel</summary>
        public static void ExportToExcel(DataGridView dgv, string fileName, bool overWrite, string sheetName, bool exportHeaders, bool open)
        {
            if (dgv == null)
                throw new ArgumentNullException("dgv");

            if (dgv.Columns.Count == 0)
                throw new Exception("所提供的 DataGridView 中不包含任何列.");

            if (File.Exists(fileName))
            {
                if (overWrite)
                    File.Delete(fileName);
                else
                    throw new Exception("文件已经存在!");
            }

            Interop.Excel.Application app = new Interop.Excel.ApplicationClass { Visible = false };
            Interop.Excel.Workbooks books = app.Workbooks;
            Interop.Excel.Workbook book = books.Add(miss);
            Interop.Excel.Worksheet sheet = (Interop.Excel.Worksheet)book.ActiveSheet;

            foreach (Interop.Excel.Worksheet s in book.Sheets)
                if (s != sheet)
                    s.Delete();

            if (!string.IsNullOrEmpty(sheetName))
                sheet.Name = sheetName;

            Recordset rs = ConvertDataGridViewToRecordSet(dgv);

            Interop.Excel.Range range = (Interop.Excel.Range)sheet.Cells[1, 1];

            if (exportHeaders) // 标题行
            {
                int i = 0;
                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    if (col.Visible)
                    {
                        Interop.Excel.Range rangeHeader = sheet.get_Range((object)(GetColumnLabel(i + 1) + "1"), miss);
                        rangeHeader.Value2 = col.HeaderText;
                        i++;
                    }
                }

                range = (Interop.Excel.Range)sheet.Cells[2, 1];
            }

            range.CopyFromRecordset(rs, miss, miss);

            book.SaveAs(fileName, Interop.Excel.XlFileFormat.xlExcel7, miss, miss, miss, miss,
                Interop.Excel.XlSaveAsAccessMode.xlNoChange, miss, miss, miss, miss, miss);

            if (open)
            {
                app.Visible = true;
            }
            else
            {
                book.Close(false, miss, miss);
                books.Close();
                app.Quit();
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(books);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
            GC.Collect();
        }
        /// <summary>导出到Excel</summary>
        public static void ExportToExcel(DataSet ds, string fileName, bool overWrite, bool exportHeaders, bool open)
        {
            if (ds == null)
                throw new ArgumentNullException("ds");

            if (File.Exists(fileName))
            {
                if (overWrite)
                    File.Delete(fileName);
                else
                    throw new Exception("文件已经存在!");
            }

            Interop.Excel.Application app = new Interop.Excel.ApplicationClass { Visible = false };
            Interop.Excel.Workbooks books = app.Workbooks;
            Interop.Excel.Workbook book = books.Add(miss);

            foreach (Interop.Excel.Worksheet item in book.Sheets)
                item.Delete();

            foreach (DataTable dt in ds.Tables)
            {
                Interop.Excel.Worksheet sheet = (Interop.Excel.Worksheet)book.Sheets.Add(miss, miss, miss, miss);

                if (!string.IsNullOrEmpty(dt.TableName))
                    sheet.Name = dt.TableName;

                Recordset rs = ConvertDataTableToRecordset(dt);
                Interop.Excel.Range range = (Interop.Excel.Range)sheet.Cells[1, 1];

                if (exportHeaders) // 标题行
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        Interop.Excel.Range rangeHeader = sheet.get_Range((object)(GetColumnLabel(i + 1) + "1"), miss);
                        rangeHeader.Value2 = dt.Columns[i].Caption;
                    }

                    range = (Interop.Excel.Range)sheet.Cells[2, 1];
                }

                range.CopyFromRecordset(rs, miss, miss);
            }

            book.SaveAs(fileName, Interop.Excel.XlFileFormat.xlExcel7, miss, miss, miss, miss,
                Interop.Excel.XlSaveAsAccessMode.xlNoChange, miss, miss, miss, miss, miss);

            if (open)
            {
                app.Visible = true;
            }
            else
            {
                book.Close(false, miss, miss);
                books.Close();
                app.Quit();
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(books);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
            GC.Collect();
        }

        /// <summary>执行 SQL 语句并返回受影响的行数</summary>
        public virtual int ExecuteNonQuery(string sql)
        {
            OleDbCommand cmd = new OleDbCommand(sql);
            return ExecuteNonQuery(cmd);
        }
        /// <summary>执行 SQL 语句并返回受影响的行数</summary>
        public virtual int ExecuteNonQuery(OleDbCommand cmd)
        {
            return this.ExecuteNonQuery(cmd, null);
        }
        /// <summary>执行 SQL 语句并返回受影响的行数</summary>
        public virtual int ExecuteNonQuery(OleDbCommand cmd, params OleDbParameter[] parms)
        {
            lock (this)
            {
                if (cmd == null)
                    throw new ArgumentNullException("cmd");

                if (parms != null && parms.Length != 0)
                    foreach (OleDbParameter param in parms)
                        cmd.Parameters.Add(param);

                CheckConnection();
                cmd.Connection = _cnn;

                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。</summary>
        public virtual object ExecuteScalar(string sql)
        {
            OleDbCommand cmd = new OleDbCommand(sql);
            return ExecuteScalar(cmd);
        }
        /// <summary>执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。</summary>
        public virtual object ExecuteScalar(OleDbCommand cmd)
        {
            return ExecuteScalar(cmd, null);
        }
        /// <summary>执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。</summary>
        public virtual object ExecuteScalar(OleDbCommand cmd, params OleDbParameter[] parms)
        {
            lock (this)
            {
                if (cmd == null)
                    throw new ArgumentNullException("cmd");

                if (parms != null && parms.Length != 0)
                    foreach (OleDbParameter param in parms)
                        cmd.Parameters.Add(param);

                CheckConnection();
                cmd.Connection = _cnn;

                return cmd.ExecuteScalar();
            }
        }

        /// <summary>生成一个 OleDbDataReader</summary>
        public virtual IDataReader ExecuteReader(string sql)
        {
            OleDbCommand cmd = new OleDbCommand(sql);
            return ExecuteReader(cmd);
        }
        /// <summary>生成一个 OleDbDataReader</summary>
        public virtual IDataReader ExecuteReader(OleDbCommand cmd)
        {
            return ExecuteReader(cmd, null);
        }
        /// <summary>生成一个 OleDbDataReader</summary>
        public virtual IDataReader ExecuteReader(OleDbCommand cmd, params OleDbParameter[] parms)
        {
            lock (this)
            {
                if (cmd == null)
                    throw new ArgumentNullException("cmd");

                if (parms != null && parms.Length != 0)
                    foreach (OleDbParameter param in parms)
                        cmd.Parameters.Add(param);

                CheckConnection();
                cmd.Connection = _cnn;

                return cmd.ExecuteReader();
            }
        }

        /// <summary>执行查询，并返回查询所返回的结果集</summary>
        public virtual DataSet GetDataSet(string sql)
        {
            OleDbCommand cmd = new OleDbCommand(sql);
            return GetDataSet(cmd);
        }
        /// <summary>执行查询，并返回查询所返回的结果集</summary>
        public virtual DataSet GetDataSet(OleDbCommand cmd)
        {
            return GetDataSet(cmd, null);
        }
        /// <summary>执行查询，并返回查询所返回的结果集</summary>
        public virtual DataSet GetDataSet(OleDbCommand cmd, params OleDbParameter[] parms)
        {
            lock (this)
            {
                if (cmd == null)
                    throw new ArgumentNullException("cmd");

                if (parms != null && parms.Length != 0)
                    foreach (OleDbParameter param in parms)
                        cmd.Parameters.Add(param);

                CheckConnection();
                cmd.Connection = _cnn;
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                DataSet result = new DataSet();
                adapter.Fill(result);

                return result;
            }
        }

        /// <summary>执行查询，并返回查询所返回的表</summary>
        public virtual DataTable GetDataTable(string sql)
        {
            OleDbCommand cmd = new OleDbCommand(sql);
            return GetDataTable(cmd);
        }
        /// <summary>执行查询，并返回查询所返回的表</summary>
        public virtual DataTable GetDataTable(OleDbCommand cmd)
        {
            return GetDataTable(cmd, null);
        }
        /// <summary>执行查询，并返回查询所返回的表</summary>
        public virtual DataTable GetDataTable(OleDbCommand cmd, params OleDbParameter[] parms)
        {
            lock (this)
            {
                if (cmd == null)
                    throw new ArgumentNullException("cmd");

                if (parms != null && parms.Length != 0)
                    foreach (OleDbParameter param in parms)
                        cmd.Parameters.Add(param);

                CheckConnection();
                cmd.Connection = _cnn;
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                DataTable result = new DataTable();
                adapter.Fill(result);

                return result;
            }
        }

        /// <summary>执行数据库事务操作</summary>
        public virtual int[] ExecuteTransaction(IEnumerable<string> sqls)
        {
            OleDbCommand[] cmds = new OleDbCommand[sqls.Count()];

            for (int i = 0; i < cmds.Length; i++)
                cmds[i] = new OleDbCommand(sqls.ElementAt(i));

            return ExecuteTransaction(cmds);
        }
        /// <summary>执行数据库事务操作</summary>
        public virtual int[] ExecuteTransaction(IEnumerable<OleDbCommand> cmds)
        {
            lock (this)
            {
                if (cmds == null || !cmds.Any())
                    throw new ArgumentNullException("cmds");

                CheckConnection();

                OleDbTransaction tran = _cnn.BeginTransaction();
                int[] result = new int[cmds.Count()];

                try
                {
                    for (int i = 0; i < result.Length; i++)
                    {
                        OleDbCommand c = cmds.ElementAt(i);
                        c.Connection = _cnn;
                        c.Transaction = tran;
                        result[i] = c.ExecuteNonQuery();
                    }

                    tran.Commit();
                    return result;
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }
    }
 */

#endregion 