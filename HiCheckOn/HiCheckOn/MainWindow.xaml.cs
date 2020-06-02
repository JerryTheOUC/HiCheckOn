using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using MessageBox = System.Windows.Forms.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace HiCheckOn
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }



        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            if (d.ShowDialog() == true) // Result could be true, false, or null
                checkOnExcel.Text = d.FileName;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            if (folderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                saveLocation.Text = folderBrowser.SelectedPath;
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            HolidayRecorder.setHolidayAndInlieu();
            if (string.Empty.Equals(checkOnExcel.Text) || string.Empty.Equals(saveLocation.Text))
            {
                MessageBox.Show("请先选择考勤记录文件记录/保存位置！");
                return;
            }
            DataSet ds = ExcelController.ExcelToDS(checkOnExcel.Text);
            System.Data.DataTable dt = ds.Tables[0];

            List<Employee> listEmployees = EmployeeController.getEmployee(dt);
            EmployeeController.setSubsidy(listEmployees, dt);

            try
            {
               ExcelController.writeExcel(listEmployees, saveLocation.Text);
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("出错了，请检查文件是否正确或联系作者！" + ex.Message);
                return;
            }
            MessageBox.Show("保存成功！");
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           modifyHoliday mh = new modifyHoliday();
            mh.Show();

        }



    }
}
