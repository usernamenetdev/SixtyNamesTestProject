using Microsoft.Office.Interop.Excel;
using System;
using System.IO;

namespace TestProject.Masters
{
    public sealed class XlsxMaster
    {
        #region Construcor
        private XlsxMaster() { }
        #endregion

        #region Fields
        private static XlsxMaster _instance;
        private static object _instanceLock = new object();
        #endregion

        #region Methods
        public static XlsxMaster GetInstance()
        {
            lock (_instanceLock ) 
            {
                _instance ??= new XlsxMaster();
            }
            return _instance;
        }

        public string CreateReport(System.Data.DataTable dataTable, string path = null, string fileName = null)
        {
            fileName ??= $"{DateTime.Now.ToString().Replace(':', '_').Replace(' ', '_')}.xlsx";

            path ??= $@"{AppDomain.CurrentDomain.BaseDirectory}ExcelReports\";

            string fullPath = Path.Combine(path, fileName);

            Directory.CreateDirectory(path);

            var excelApp = new Application();
            excelApp.Workbooks.Add();
            _Worksheet workSheet = excelApp.ActiveSheet;

            for (var i = 0; i < dataTable.Columns.Count; i++)
                workSheet.Cells[1, i + 1] = dataTable.Columns[i].ColumnName;

            for (var i = 0; i < dataTable.Rows.Count; i++)
            {
                for (var j = 0; j < dataTable.Columns.Count; j++)
                    workSheet.Cells[i + 2, j + 1] = dataTable.Rows[i][j];
            }

            try
            {
                lock (_instanceLock)
                    workSheet.SaveAs(fullPath);

                return fullPath;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                excelApp.Quit();
            }
        }
        #endregion

    }
}
