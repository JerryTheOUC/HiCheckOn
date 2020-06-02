using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;


namespace HiCheckOn
{
    class ExcelController
    {

        public static DataSet ExcelToDS(string Path)
        {
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + Path + ";" + "Extended Properties=Excel 8.0;";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            string strExcel = "";
            OleDbDataAdapter myCommand = null;
            DataSet ds = null;
           
            System.Data.DataTable schemaTable = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
            string tableName = schemaTable.Rows[0][2].ToString().Trim();

            strExcel = "select * from [" + tableName + "]";
           // strExcel = "select * from [各部门考勤$]";
            myCommand = new OleDbDataAdapter(strExcel, strConn);
            ds = new DataSet();
            myCommand.Fill(ds, "table1");
            return ds;
        }

        public static void writeExcel(List<Employee> listEmployees,string exportExcelPath)
        {
            //string importExcelPath = "E:\\餐补申请表-4月.xlsx";
            exportExcelPath += "\\计算结果";
            //创建
            Excel.Application xlApp = new Excel.Application();
            xlApp.DisplayAlerts = false;
            xlApp.Visible = false;
            xlApp.ScreenUpdating = false;
            //打开Excel
/*            Excel.Workbook xlsWorkBook = xlApp.Workbooks.Open(importExcelPath, System.Type.Missing, System.Type.Missing, System.Type.Missing,
            System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing,
            System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing);*/
            Excel.Workbook xlsWorkBook = xlApp.Workbooks.Add();
            //处理数据过程，更多操作方法自行百度
            Excel.Worksheet sheet = xlsWorkBook.Worksheets[1];//工作薄从1开始，不是0
            //sheet.Cells[1, 1] = "test";
            sheet.Cells[1, 1] = "序号";
            sheet.Cells[1, 2] = "部门";
            sheet.Cells[1, 3] = "工号";
            sheet.Cells[1, 4] = "姓名";
            sheet.Cells[1, 5] = "工作日天数";
            sheet.Cells[1, 6] = "标准（元/天）";
            sheet.Cells[1, 7] = "周六天数";
            sheet.Cells[1, 8] = "标准（元/天）";
            sheet.Cells[1, 7] = "节假日/周日天数";
            sheet.Cells[1, 10] = "标准（元/天）";
            sheet.Cells[1, 11] = "金额";
            int i = 2;
            foreach (Employee ep in listEmployees)
            {
                if(ep.subsidy == 0)
                    continue;
                sheet.Cells[i, 1] = i-1;
                sheet.Cells[i, 2] = ep.department;
                sheet.Cells[i, 3] = ep.number;
                sheet.Cells[i, 4] = ep.name;
                sheet.Cells[i, 5] = ep.workDayWorkDays;
                sheet.Cells[i, 6] = 10;
                sheet.Cells[i, 7] = ep.saturdayWorkDays;
                sheet.Cells[i, 8] = 10;
                sheet.Cells[i, 9] = ep.sundayAndHolidyWorkDays;
                sheet.Cells[i, 10] = 20;
                sheet.Cells[i, 11] = ep.subsidy;
                i++;
            }
            //另存
            xlsWorkBook.SaveAs(exportExcelPath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlNoChange,
              Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            //关闭Excel进程
            ClosePro(xlApp, xlsWorkBook);
        }

        public static void ClosePro(Excel.Application xlApp, Excel.Workbook xlsWorkBook)
        {
            if (xlsWorkBook != null)
                xlsWorkBook.Close(true, Type.Missing, Type.Missing);
            xlApp.Quit();
            // 安全回收进程
            System.GC.GetGeneration(xlApp);
            IntPtr t = new IntPtr(xlApp.Hwnd);  //获取句柄
            int k = 0;
            GetWindowThreadProcessId(t, out k);  //获取进程唯一标志
            System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(k);
            p.Kill();   //关闭进程
        }

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);
    }
}
