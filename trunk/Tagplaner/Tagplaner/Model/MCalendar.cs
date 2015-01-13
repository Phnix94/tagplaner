﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tagplaner
{
    [Serializable()]
    public class MCalendar
    {
        private int id;
        private List<MCalendarDay> calendarList = new List<MCalendarDay>();
        private static MCalendar instance;
        private DateTime startdate;
        private DateTime enddate;
        private String currentYear;
        private String nextYear;
        private string startdateString;
        private string enddateString;
        private List<MSpeciality> speciality;

        #region getter
        public int Id
        {
            get { return id; }
            set { this.id = value; }
        }
        public DateTime Startdate
        {
            get { return startdate; }
            set { startdate = value; }
        }
        public DateTime Enddate
        {
            get { return enddate; }
            set { enddate = value; }
        }
        public string StartdateString
        {
            get { return startdateString; }
            set { startdateString = value; }
        }
        public string EnddateString
        {
            get { return enddateString; }
            set { enddateString = value; }
        }
        public List<MCalendarDay> CalendarList
        {
            get { return calendarList; }
            set { calendarList = value; }
        }
        public List<MSpeciality> Speciality
        {
            get { return speciality; }
            set { speciality = value; }
        }
        #endregion
        private MCalendar()
        {

        }

        public static MCalendar getInstance()
        {
            if (instance == null)
            {
                instance = new MCalendar();
            } return instance;
        }


        //public void fillCalendarInitial(DateTime start, DateTime end, String vacationCurrentYearUrl, String vacationNextYearUrl, String holidayCurrentYearUrl, String holidayNextYearUrl)
        public void fillCalendarInitial(DateTime start, DateTime end, int numberOfYears, List<String> typesOfClasses)
        {

            startdate = start;
            enddate = end;

            //this.currentYear = currentYearParameter;
            //this.nextYear = nextYearParameter;

            StartdateString = String.Format("{0:D}", start);
            EnddateString = String.Format("{0:D}", end);

            CalendarList.Clear();

            //Befüllen der CalendarList von startdatum bis enddatum
            CCalendar ccalendar = new CCalendar();
            //CalendarList = ccalendar.fillDaysInitial(start, end, CalendarList, vacationCurrentYearUrl, vacationNextYearUrl);
            CalendarList = ccalendar.fillDaysInitial(start, end, CalendarList);

            //CalendarList = ccalendar.fillHolidaysInitial(start, end, CalendarList, holidayCurrentYearUrl, holidayNextYearUrl);
            CalendarList = ccalendar.fillHolidaysInitial(start, end, CalendarList);
            Speciality = ccalendar.fillSpeziallityInitial(Speciality, numberOfYears, typesOfClasses);
        }

        #region AddDay-Methods

        #endregion

    }
}
