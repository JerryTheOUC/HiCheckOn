using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace HiCheckOn
{
    /// <summary>
    /// modifyHoliday.xaml 的交互逻辑
    /// </summary>
    public partial class modifyHoliday : Window
    {
        public modifyHoliday()
        {
            InitializeComponent();
            XmlDocument doc = new XmlDocument();
            //doc.Load(@"..\..\holiday.xml");
            doc.Load(@"holiday.xml");
            string holidayList = doc.SelectSingleNode("/body/holiday").InnerText;
            string inLieuList = doc.SelectSingleNode("/body/leaveInLieu").InnerText;
            tbHoliday.Text = holidayList;
            tbLeaveInLieu.Text = inLieuList;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HolidayRecorder.updateHolidayXML(tbHoliday.Text,tbLeaveInLieu.Text);
            this.Close();
        }
    }
}
