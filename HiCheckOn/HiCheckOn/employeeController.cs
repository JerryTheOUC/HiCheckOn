using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiCheckOn
{
    class EmployeeController
    {
        public static List<Employee> getEmployee(System.Data.DataTable dt)
        {
            List<Employee> employees = new List<Employee>();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr[0] == string.Empty || dr[1] == String.Empty)
                    continue;
                if (employees.Count == 0 || !containEmployee(employees, dr))
                {
                    Employee ep = new Employee();
                    ep.number = dr[0].ToString();
                    ep.name = dr[1].ToString();
                    ep.department = dr[2].ToString();
                    ep.subsidy = 0;
                    ep.saturdayWorkDays = 0;
                    ep.sundayAndHolidyWorkDays = 0;
                    employees.Add(ep);
                }
            }
            return employees;
        }

        static bool containEmployee(List<Employee> list, DataRow dr)
        {
            if (dr[0] != string.Empty && dr[1] != String.Empty)
            {
                foreach (Employee ep in list)
                {
                    if (ep.name.Equals(dr[1].ToString()) && ep.number.Equals(dr[0].ToString()))
                        return true;
                }
            }
            return false;
        }

        public static void setSubsidy(List<Employee> listEmployee, System.Data.DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                if (dr[0].Equals(String.Empty)
                    || dr[1].ToString().Equals(String.Empty)
                    || dr[3].ToString().Equals(String.Empty)
                    || dr[4].ToString().Equals(String.Empty)
                    || dr[5].ToString().Equals(String.Empty)
                    || dr[6].ToString().Equals(String.Empty))
                    continue;
                Employee ep =
                    listEmployee.SingleOrDefault(
                        q => q.name.Equals(dr[1].ToString()) && q.number.Equals(dr[0].ToString()));
                if (ep != null)
                    setSubsidyForOneDay(dr, ep);
            }
        }
        
/*        static void setSubsidyForOneDay(DataRow dr, Employee ep)
        {
            if (!dr[3].Equals(dr[5]))
                return;
            DateTime timeToWork = new DateTime();
            try
            {
                timeToWork = Convert.ToDateTime(dr[4]);
            }
            catch (Exception)
            {
                return;
            }

            if (timeToWork.TimeOfDay.CompareTo(new TimeSpan(6, 0, 0)) < 0 ||
                timeToWork.TimeOfDay.CompareTo(new TimeSpan(10, 0, 0)) > 0)
                return;
            DateTime timeToHome = new DateTime();
            try
            {
                timeToHome = Convert.ToDateTime(dr[6]);
            }
            catch (Exception)
            {
                return;
            }

            DateTime dt = new DateTime();
            try
            {
                dt = Convert.ToDateTime(dr[3]);
            }
            catch (Exception)
            {
                return;
            }

            if (isHoliday(dt))
            {
                setHoliDaySubsidy(ep, timeToHome.TimeOfDay);
                return;
            }
            if (isWorkDay(dt))
            {
                setWordDaySubsidy(ep, timeToHome.TimeOfDay);
                return;
            }
            if (isWeeken(dt))
            {
                setWeekenSubsidy(ep, dt, timeToHome.TimeOfDay);
            }
        }*/

        static void setSubsidyForOneDay(DataRow dr, Employee ep)
        {
            if (!dr[3].Equals(dr[5]))
                return;
            DateTime timeToWork = new DateTime();
           
           timeToWork = Convert.ToDateTime(dr[4]);
         

            if (timeToWork.TimeOfDay.CompareTo(new TimeSpan(6, 0, 0)) < 0 ||
                timeToWork.TimeOfDay.CompareTo(new TimeSpan(10, 0, 0)) > 0)
                return;
            DateTime timeToHome = new DateTime();
            
                timeToHome = Convert.ToDateTime(dr[6]);
        

            DateTime dt = new DateTime();
      
                dt = Convert.ToDateTime(dr[3]);
  

            if (isHoliday(dt))
            {
                setHoliDaySubsidy(ep, timeToHome.TimeOfDay);
                return;
            }
            if (isWorkDay(dt))
            {
                setWordDaySubsidy(ep, timeToHome.TimeOfDay);
                return;
            }
            if (isWeeken(dt))
            {
                setWeekenSubsidy(ep, dt, timeToHome.TimeOfDay);
            }
        }
        static void setWordDaySubsidy(Employee ep, TimeSpan ts)
        {
            if (ts.CompareTo(new TimeSpan(20, 0, 0)) > 0)
            {
                ep.subsidy += 10;
                ep.workDayWorkDays += 1;
            }
        }

        static void setHoliDaySubsidy(Employee ep, TimeSpan ts)
        {
            if (ts.CompareTo(new TimeSpan(17, 0, 0)) > 0)
            {
                ep.subsidy += 20;
                ep.sundayAndHolidyWorkDays += 1;
            }
        }

        static void setWeekenSubsidy(Employee ep, DateTime dt, TimeSpan ts)
        {
            if (ts.CompareTo(new TimeSpan(17, 0, 0)) > 0)
            {
                if (dt.DayOfWeek.ToString().Equals("Saturday"))
                {
                    ep.subsidy += 10;
                    ep.saturdayWorkDays += 1;
                }
                if (dt.DayOfWeek.ToString().Equals("Sunday"))
                {
                    ep.subsidy += 20;
                    ep.sundayAndHolidyWorkDays += 1;
                }
            }
        }

        static bool isWorkDay(DateTime dt)
        {
            if (isleaveInLieu(dt))
                return true;
            if (!dt.DayOfWeek.ToString().Equals("Saturday") && !dt.DayOfWeek.ToString().Equals("Sunday"))
                return true;
           
            return false;
        }

        static bool isleaveInLieu(DateTime dt)
        {
            if (HolidayRecorder.isleaveInLieu.Contains(dt))
                return true;
            return false;
        }
        static bool isHoliday(DateTime dt)
        {
/*            if (dt.ToShortDateString().Equals("2020-04-04") || dt.ToShortDateString().Equals("2020-04-05") ||
                dt.ToShortDateString().Equals("2020-04-06"))
                return true;*/
            if (HolidayRecorder.holidays.Contains(dt))
                return true;
            return false;
        }

        static bool isWeeken(DateTime dt)
        {
            if (!isWorkDay(dt))
                return true;
            return false;
        }
    }
}
