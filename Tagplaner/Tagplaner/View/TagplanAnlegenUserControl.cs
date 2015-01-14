﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Tagplaner
{
    public partial class TagplanAnlegenUserControl : UserControl
    {
        Tagplaner.View.FerienFeiertageAuswaehlenForm ferienFeiertageAuswaehlenForm;
        private DataGridView dGV = new DataGridView();

        private FormInit formInit;

        MCalendar calendarWithDays;

        int numberOfYears;
        List<String> typeOfClasses;

        TagplanBearbeitenUserControl tagplanBearbeitenUC;

        CSerialize serializer;
        CDatabase databaseController;

        String vacationCurrentYearUrl;
        String vacationNextYearUrl;
        String holidayCurrentYearUrl;
        String holidayNextYearUrl;

        public TagplanAnlegenUserControl(FormInit formInit, TagplanBearbeitenUserControl tagplanBearbeitenUC)
        {
            this.formInit = formInit;
            this.tagplanBearbeitenUC = tagplanBearbeitenUC;
            serializer = new CSerialize();
            typeOfClasses = new List<String>();
            numberOfYears = 1;
            databaseController = new CDatabase();
            //databaseController.FillFederalStateCombobox(comboBoxBundesland);
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void TagplanAnlegenUserControl_Load(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Visible = true;
            groupBox2.Visible = false;
            numberOfYears = 1;
        }

        private void radioButton2_CheckedChanged_1(object sender, EventArgs e)
        {
            groupBox2.Visible = true;
            numberOfYears = 2;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        //Zum nächsten Tab springen
        public void nextTabPage()
        {
            formInit.tabPageChange(1);
            tagplanBearbeitenUC.GetListView().AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.HeaderSize);
            tagplanBearbeitenUC.GetListView().AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.HeaderSize);
            tagplanBearbeitenUC.GetListView().AutoResizeColumn(2, ColumnHeaderAutoResizeStyle.HeaderSize);
            tagplanBearbeitenUC.GetListView().AutoResizeColumn(3, ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        //Initialisieren des Kalenderobjekts
        public void GetCalendarWithDates()
        {
            CeckCheckboxes();
            MCalendar.getInstance().fillCalendarInitial(this.dateTimePickerVon.Value, this.dateTimePickerBis.Value, numberOfYears, typeOfClasses, vacationCurrentYearUrl, vacationNextYearUrl, holidayCurrentYearUrl, holidayNextYearUrl);
            calendarWithDays = MCalendar.getInstance();
        }

        //Füllen der Listview mit Tagen, Ferien und Feiertagen
        public void fillListViewWithDays(List<MCalendarDay> calendarDayList, ListView listView)
        {
            foreach (MCalendarDay calendarDay in calendarDayList)
            {
                ListViewItem listViewItem = new ListViewItem();

                if (int.Parse(calendarDay.CalendarWeek) % 2 == 0)
                {
                    listViewItem.Text = calendarDay.CalendarWeek;
                    listViewItem.SubItems.Add((calendarDay.GetCalendarDatePrintDate()));

                    listViewItem.SubItems[0].BackColor = Color.LightCyan;
                    listViewItem.SubItems[1].BackColor = Color.LightCyan;
                    listViewItem.UseItemStyleForSubItems = false;
                }

                else
                {
                    listViewItem.Text = calendarDay.CalendarWeek;
                    listViewItem.SubItems.Add(calendarDay.GetCalendarDatePrintDate());
                }

                if (calendarDay.VacationName != null)
                {
                    listViewItem.SubItems.Add(calendarDay.VacationName);

                    listViewItem.SubItems[2].BackColor = Color.LightGreen;
                    listViewItem.UseItemStyleForSubItems = false;
                }

                if (calendarDay.HolidayName != null && calendarDay.VacationName != null)
                {
                    listViewItem.SubItems.Add(calendarDay.HolidayName);

                    listViewItem.SubItems[3].BackColor = Color.LightGreen;
                    listViewItem.UseItemStyleForSubItems = false;
                }

                if (calendarDay.HolidayName != null)
                {
                    listViewItem.SubItems.Add("");
                    listViewItem.SubItems.Add(calendarDay.HolidayName);

                    listViewItem.SubItems[3].BackColor = Color.LightGreen;
                    listViewItem.UseItemStyleForSubItems = false;
                }

                listView.Items.Add(listViewItem);
            }
            CEditPlan ceditplan = new CEditPlan();
            ceditplan.fillGrids(tagplanBearbeitenUC.dGVReturn(), 12, listView);
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void buttonWeiter_Click(object sender, EventArgs e)
        {
            if (vacationCurrentYearUrl != null && vacationNextYearUrl != null && holidayCurrentYearUrl != null && holidayNextYearUrl != null)
            {
                tagplanBearbeitenUC.GetListView().Items.Clear();

                //Werte aus Datepicker werden an Kalenderobjekt übergeben
                GetCalendarWithDates();

                //ListView wird mit Tagen gefüllt
                fillListViewWithDays(calendarWithDays.CalendarList, tagplanBearbeitenUC.GetListView());

                //TagplanBearbeitenTab wird angezeigt
                this.label3.Visible = false;
                nextTabPage();
            }
        }

        //Laden der Ferien und Feiertage
        private void button6_Click(object sender, EventArgs e)
        {
            using (ferienFeiertageAuswaehlenForm = new Tagplaner.View.FerienFeiertageAuswaehlenForm())
            {
                DialogResult dr = ferienFeiertageAuswaehlenForm.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    holidayCurrentYearUrl = ferienFeiertageAuswaehlenForm.TextForBox1;
                    holidayNextYearUrl = ferienFeiertageAuswaehlenForm.TextForBox2;
                    vacationCurrentYearUrl = ferienFeiertageAuswaehlenForm.TextForBox3;
                    vacationNextYearUrl = ferienFeiertageAuswaehlenForm.TextForBox4;

                    this.label4.Text = "Feriendatei (Von): " + splitUrl(holidayCurrentYearUrl) + "\n" +
                                       "Feriendatei (Bis): " + splitUrl(holidayNextYearUrl) + "\n" +
                                       "Feiertagdatei (Von): " + splitUrl(vacationCurrentYearUrl) + "\n" +
                                       "Feiertagdatei (Bis): " + splitUrl(vacationNextYearUrl) + " geöffnet";
                    this.label4.Visible = true;
                    this.buttonWeiter.Enabled = true;
                }
            }

        }

        private void btn_feiertageOeffnen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Feiertage");

            openFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "Feiertage";
            openFileDialog1.Title = "Feiertagedatei öffnen";
            openFileDialog1.Filter = "Feiertage|*.ft";
            openFileDialog1.Multiselect = true;

            DialogResult fileChoiceResult = openFileDialog1.ShowDialog();

            if (fileChoiceResult == DialogResult.OK)
            {
                //choosenHoliday = (MHoliday)serializer.DeserializeObject(openFileDialog1.FileName);

                this.label4.Text = openFileDialog1.FileName + " geöffnet";
                this.label4.Visible = true;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Tagplan");

            saveFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "Tagplan";

            saveFileDialog1.Filter = "Tagplan|*.tp";
            saveFileDialog1.Title = "Tagplan abspeichern";

            DialogResult fileSaveResult = saveFileDialog1.ShowDialog();

            if (fileSaveResult == DialogResult.OK && saveFileDialog1.FileName != null)
            {
                calendarWithDays = MCalendar.getInstance();
                serializer.SerializeObject(calendarWithDays, saveFileDialog1.FileName);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Tagplan");

            openFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "Tagplan";
            openFileDialog1.Title = "Tagplan öffnen";
            openFileDialog1.Filter = "Tagplan|*.tp";
            openFileDialog1.Multiselect = true;

            DialogResult fileChoiceResult = openFileDialog1.ShowDialog();

            if (fileChoiceResult == DialogResult.OK)
            {
                calendarWithDays = (MCalendar)serializer.DeserializeObject(openFileDialog1.FileName);
                tagplanBearbeitenUC.GetListView().Items.Clear();
                fillListViewWithDays(calendarWithDays.CalendarList, tagplanBearbeitenUC.GetListView());
                this.label3.Text = "Tagplan: " + splitUrl(openFileDialog1.FileName) + " geöffnet";
                this.label3.Visible = true;
                nextTabPage();
            }
        }

        public void CeckCheckboxes()
        {
            if (checkBoxErsterJahrgangAE.Checked)
            {
                typeOfClasses.Add("AE");
            }
            else
                typeOfClasses.Add("");

            if (checkBoxErsterJahrgangSI.Checked)
            {
                typeOfClasses.Add("SI");
            }
            else
                typeOfClasses.Add("");

            if (checkBoxZweiterJahrgangAE.Checked)
            {
                typeOfClasses.Add("AE");
            }
            else
                typeOfClasses.Add("");

            if (checkBoxZweiterJahrgangSI.Checked)
            {
                typeOfClasses.Add("SI");
            }
            else
                typeOfClasses.Add("");
        }

        public String splitUrl(String url)
        {
            String splittedUrl;
            int counter = 0;
            String[] substrings = url.Split('\\');
            foreach (String partOfSubstring in substrings)
            {
                counter++;
            }
            splittedUrl = substrings[counter - 1];
            return splittedUrl;
        }

        public String createTextForUrlLabel(List<String> urlList)
        {
            StringBuilder sb = new StringBuilder();

            foreach (String url in urlList)
            {
                sb.Append(url + "\n");
            }
            return sb.ToString();
        }
    }
}