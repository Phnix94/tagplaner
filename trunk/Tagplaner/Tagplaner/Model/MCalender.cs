﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tagplaner
{
    class MCalender
    {
        private List<MCalendarEntry> calendarList = new List<MCalendarEntry>();
        private DateTime firstday { get; set; }
        private DateTime endday { get; set; }
        private static MCalender instance;
        public List<MCalendarEntry> CalendarList
        {
            get { return calendarList; }
        }


        private MCalender()
        {

        }

        public static MCalender getInstance()
        {
            if (instance == null)
            {
                instance = new MCalender();
            } return instance;
        }
        

        #region AddDay-Methods
        public void AddSeminarDay(MTrainer trainer, MTrainer cotrainer, MCalendarDay calendarDay, MSpeciality speciality, MSeminar seminar, MPlace place, MRoom room)
        {
            CalendarEntry cal = new CalendarEntry(trainer, cotrainer, calendarDay, speciality, seminar, place, room);
            calendarList.Add(cal);
        }

        public void AddPracticeDay(MCalendarDay calendarDay, MSpeciality speciality, MPractice practice)
        {
            CalendarEntry cal = new CalendarEntry(calendarDay, speciality, practice);
            calendarList.Add(cal);
        }

        public void AddSchoolDay(MCalendarDay calendarDay, MSpeciality speciality, MSchool school)
        {
            CalendarEntry cal = new CalendarEntry(calendarDay, speciality, school);
            calendarList.Add(cal);
        }

        public void AddHoliday(MCalendarDay calendarDay, MSpeciality speciality, MHoliday holiday)
        {
            CalendarEntry cal = new CalendarEntry(calendarDay, speciality, holiday);
            calendarList.Add(cal);
        }

        public void AddPracticeSeminarDay(MTrainer trainer, MTrainer cotrainer, MCalendarDay calendarDay, MSpeciality speciality, MSeminar seminar, MPlace place, MRoom room, MPractice practice)
        {
            CalendarEntry cal = new CalendarEntry(trainer, cotrainer, calendarDay, speciality, seminar, place, room, practice);
            calendarList.Add(cal);
        }

#endregion
        
    }
}
