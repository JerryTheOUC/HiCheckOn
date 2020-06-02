using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HiCheckOn
{
    public static class HolidayRecorder
    {
        public static List<DateTime> holidays = new List<DateTime>();

        public static List<DateTime> isleaveInLieu = new List<DateTime>();

        public static void updateHolidayXML(string holidayList, string inLieuList)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"holiday.xml");
            //doc.Load(@"..\..\holiday.xml");
            XmlNode holidayNode = doc.SelectSingleNode("/body/holiday");
            holidayNode.InnerText = holidayList;
            XmlNode inLieuNode = doc.SelectSingleNode("/body/leaveInLieu");
            inLieuNode.InnerText = inLieuList;
            //doc.Save(@"..\..\holiday.xml");
            doc.Save(@"holiday.xml");
        }

        public static void setHolidayAndInlieu()
        {
            XmlDocument doc = new XmlDocument();
            //doc.Load(@"..\..\holiday.xml");
            doc.Load(@"holiday.xml");
            string holidayList = doc.SelectSingleNode("/body/holiday").InnerText;
            string inLieuList = doc.SelectSingleNode("/body/leaveInLieu").InnerText;
            setHolidays(holidayList);
            setleaveInLieu(inLieuList);
        }

        public static void setHolidays(string list)
        {
            if(string.Empty.Equals(list))
                return;
            string[] holidayStrList = list.Split(',');
            foreach (string holidayStr in holidayStrList)
            {
                holidays.Add(Convert.ToDateTime(holidayStr));
            }
        }
        public static void setleaveInLieu(string list)
        {
            if (string.Empty.Equals(list))
                return;
           
            string[] inLieuList = list.Split(',');
            foreach (string inLieustr in inLieuList)
            {
                isleaveInLieu.Add(Convert.ToDateTime(inLieustr));
            }
        }
    }
}
