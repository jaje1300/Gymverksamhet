using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using NpgsqlTypes;

namespace Gymverksamhet_G3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();            
        }
        //VARIABLER 
        private Medlem aktuellMedlem;
        private Instruktor aktuellInstruktor;
        private Aktivitet aktuellAktivitet;
        private Aktivitet aktuellAktivitetsBokning;
        private Medlemtyp aktuellMedlemstyp;
        private Kompetens aktuellKompetens;
        private Lokal aktuellLokal;
        private Medlem bokningsmarkeradMedlem;      
        private Aktivitet bokningsmarkeradAktivitet; 
        private Inloggning aktuellInlogg;
        private bool utloggningsKontroll = false;
        private int räknaHämtaMedlemmarsBokadePass = 0;
                
        

        //FORMULÄRET LADDAS
        private void Form1_Load(object sender, EventArgs e)
        {           
            Uppdatera_Medlemslista();       
            Uppdatera_Instruktorslista();
            Uppdatera_Aktivitetslista();
            Uppdatera_Bokningslista();
            //Uppdatera_Schema_Kontroller(); //vid nytt träningspass ist
            Schema();
      
            Inloggningsinformation();   //läs in inloggninsinfo
            Behörighetsvy();            //ta bort tabbar & kontroller
            HämtaLokaler();
            HämtaMedlemstypAdminCombobox();
            Hämta_Användare_Admin();
        }
       
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(utloggningsKontroll == false)
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    DialogResult result = MessageBox.Show("Vill du verkligen stänga av programmet? Allt osparat arbete kommer gå förlorat.", "Warning", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
                else
                {
                    e.Cancel = true;
                }
            }            
        }

        //METODER
        //**************************************************************************************************************
        //**************************************************************************************************************        

        private void Inloggningsinformation()
        {            
            aktuellInlogg = Inloggning.DenInloggadeAnvändaren;     //placera den inloggade användarens värden i objektvariabeln 
            label_Form1_AktuelltAnvändarnamn.Text = aktuellInlogg.Anvandarnamn;
            label_Form1_AktuellBehörighet.Text = aktuellInlogg.Behorighet;
        }
        private void Behörighetsvy()
        {
            if (aktuellInlogg.Behorighet == "Medlem")
            {
                tabControl_Form1.TabPages.Remove(tabPage_Instruktörer);
                tabControl_Form1.TabPages.Remove(tabPage_Administration);
                
                button_Medlem_Ny.Hide();
                button_Medlem_TaBort.Hide();
                button_Medlem_Sök.Hide();
                textBox_Medlem_Sok.Hide();
                label_Aktivitet_Traningstyp.Hide();
                comboBox_Aktivitet_Traningstyp.Hide();
                label_Aktivitet_Instruktor.Hide();
                comboBox_Aktivitet_Instruktor.Hide();
                label_Aktivitet_Lokal.Hide();
                comboBox_Aktivitet_Lokal.Hide();
                button_Aktivitet_LäggTillPass.Hide();                
                dateTimePicker_Aktivitet_LäggTillPass.Hide();

                button_TaBortPass.Hide();
                button_Ändra_Pass.Hide();
                textBox_MaxAntal.Hide();
                label18.Hide();
                labelFrån.Hide();
                labelTill.Hide();
                dateTimePicker1.Hide();
                dateTimePicker2.Hide();
                button_Aktivitet_NyttPass.Hide();
                button_Bokning_SokMedlem.Hide();
               // listBox_Bokning_Medlem.Hide();
              //  groupBox_Bokning_Medlemmar.Hide();
                listBox_Bokning_PassetsInbokadeMedlemmar.Hide();                

                labelBokningSökMedlem.Hide();
                textBox_Bokning_SokMedlem.Hide();
            }
            else if (aktuellInlogg.Behorighet == "Instruktör")
            {                
                tabControl_Form1.TabPages.Remove(tabPage_Administration);
                tabControl_Form1.TabPages.Remove(tabPage_Medlemmar);
                
                button_Instruktor_Ny.Hide();
                button_Instruktor_TaBort.Hide();
                comboBox_Instruktor_Kompetens.Hide();
                button_Instruktor_Kompetens.Hide();
            }
            else if (aktuellInlogg.Behorighet == "Reception")
            {
                //reception ser allt utom administrationstabben
                tabControl_Form1.TabPages.Remove(tabPage_Administration);
            }
            else
            {
                //.allt visas för ägare
            }
        }

        private void Uppdatera_Medlemslista()
        {            
            listBox_Medlem.DataSource = Databasfunktioner.GetMedlemmar();
            listBox_Bokning_Medlem.DataSource = Databasfunktioner.GetMedlemmar();
        }
        private void Uppdatera_Instruktorslista()
        {
            listBox_Instruktor.DataSource = Databasfunktioner.GetInstruktorer();
        }
        private bool Klocka(bool yes, DateTime start, DateTime slut)
        {            
            int[] räknaHour = new int[12];
            int[] räknaMin = new int[60];
            int hh = 0;
            int mm = 0;

            for (hh = 0; hh > 12; hh++)
            {
                räknaHour[hh] = hh;                    
            }
            for (mm = 0; mm > 60; mm++)
            {
                räknaMin[mm] = mm;
            }
            
            if (räknaHour[hh] == start.Hour)
            {
                if (räknaMin[mm] == start.Minute)
                {
                    yes = true;
                }
            }
            if(räknaHour[hh] == slut.Hour)
            {
                if(räknaMin[mm] == slut.Minute)
                {
                    yes = true;
                }
            }

            return yes;
        }


        private void AddARow(DataTable table)
        {    
            DateTime toDay = new DateTime();
            toDay = DateTime.Now.Date;
            DateTime m, t, o, to, f, l, s = toDay;            

            DateTime[] days = new DateTime[6];
            days = VeckoDagarna();

            m = days[0];
            t = days[1];
            o = days[2];
            to = days[3];
            f = days[4];
            l = days[5];
            s = days[6];

            BindingList<Aktivitet> list = new BindingList<Aktivitet>();
            list = Databasfunktioner.GetAktiviteter1(m, t, o, to, f, l, s);
            foreach(Aktivitet at in list)                                   //för varje aktivitet denna vecka
            {                                                               //ny rad per aktivitet... behöver föra in likvärdigt schemalagda aktiviteter på samma rad
                //foreach(Aktivitet at2 in dataGridView_Schema.Contains)
                //{
                //    //so
                //}

                //if ())
                //{

                //}
                //table
                //foreach (DataGridViewRow row in table.Rows)
                //{
                //    string folderPath = row.Cells[5].Value.ToString();
                //    string backupIncludes = row.Cells[4].Value.ToString();
                //    Console.WriteLine("Folder Path: " + folderPath);
                //    Console.WriteLine("Backup Includes: " + backupIncludes);
                //}
                bool yes = false;
                Klocka(yes, at.TidFrån, at.TidTill);
                //DataColumn newColumn = table.
                if(yes == true)
                {
                   // table.Columns.Add(newColumn)
                    //samma rad
                }                
                
                DataRow newRow = table.NewRow();                            //radobjekt är en rad i table
                
                table.Rows.Add(newRow);                                     //lägg till radobjekt
            }
        }
        
        private void Uppdatera_Aktivitetslista()
        {  
            DateTime[] days = new DateTime[6];
            days = VeckoDagarna();

            //BindingList<Aktivitet> aktalistan = new BindingList<Aktivitet>();
            //aktalistan = Databasfunktioner.GetAktiviteter1(days[0], days[1], days[2], days[3], days[4], days[5], days[6]);
            //foreach (Aktivitet akt in aktalistan)
            //{
            //    //Aktivitet akt2 = new Aktivitet
            //    //akt2 = (Aktivitet)akt.Datum + akt.ToString();
            //    listBox_Bokning_Aktivitet.Items.Add(akt.Datum + akt.ToString());
            //    // = akt.Datum + akt.ToString();
            //    //aktalistan.Add(akt); 
            //}
            ////listBox_Bokning_Aktivitet.DataSource = aktalistan;

            listBox_Bokning_Aktivitet.DataSource = Databasfunktioner.GetAktiviteter1(days[0], days[1], days[2], days[3], days[4], days[5], days[6]);
            
            listBox_Administration_Aktivitet.DataSource = Databasfunktioner.GetAktiviteter2(); //adminlistBox, samtliga aktiviteter
                        
            DataTable table = new DataTable();

            AddARow(table);                                                     //ladda table med veckans aktiviteter
            foreach (DataGridViewRow row in dataGridView_Schema.Rows)           //varje rad och template formatteras
            { 
                dataGridView_Schema.RowTemplate.Height = 80;
                //column width
                row.Height = 80;
            }
            dataGridView_Schema.Rows.Add(table);                                //lägg in table i DGV
        }

        private void Uppdatera_Bokningslista()
        {
            listBox_Bokning_PassetsInbokadeMedlemmar.DataSource = Databasfunktioner.HämtaMedlemsID(bokningsmarkeradAktivitet.Passnummer);
        }
        private void Uppdatera_Medlemskapstypslista()
        {
            comboBox_Medlem_MedlemstypNamn.DataSource = Databasfunktioner.HämtaMedlemstyp();
        }
        private void HämtaMedlemstypAdminCombobox()
        {
            comboBox_Administration_Medlemskapstyp.DataSource = Databasfunktioner.HämtaMedlemstyp();
        }
        private void Uppdatera_Schema_Kontroller()
        {
            comboBox_Aktivitet_Traningstyp.DataSource = Databasfunktioner.GetTraningstyp();
            comboBox_Aktivitet_Lokal.DataSource = Databasfunktioner.GetLokal();
            comboBox_Aktivitet_Instruktor.DataSource = Databasfunktioner.GetInstruktorer();

        }
        private void HämtaLokaler()
        {
            comboBox_Administration_Lokaler.DataSource = Databasfunktioner.GetLokal();
            comboBox_Administration_LokalUtrustning.Text = "Välj";
        }
        private void Hämta_Användare_Admin()
        {
            comboBox_Administration_Användare.DataSource = Databasfunktioner.GetUsers();
        }
        private void UppdateraAntalInbokade()
        {
            listBox_Bokning_PassetsInbokadeMedlemmar.DataSource = Databasfunktioner.HämtaMedlemsID(bokningsmarkeradAktivitet.Passnummer);
            BindingList<Medlem> antal = Databasfunktioner.HämtaMedlemsID(bokningsmarkeradAktivitet.Passnummer);
            int count = 0;
            foreach (Medlem m in antal)
            {
                count++;
            }
            textBoxAntalInbokade.Text = "Antal inbokade: " + count.ToString();
        }
        private void HämtaMedlemsBokningar()
        {
            listBox_Bokning_MedlemmensBokningar.DataSource = Databasfunktioner.HämtaMedlemsBokning(bokningsmarkeradMedlem.Medlemsnummer);
            räknaHämtaMedlemmarsBokadePass++;         
        }

        private DateTime[] VeckoDagarna()
        {
            DateTime idag = new DateTime();
            idag = DateTime.Now.Date;

            DateTime måndag = idag;
            DateTime tisdag = idag;
            DateTime onsdag = idag;
            DateTime torsdag = idag;
            DateTime fredag = idag;
            DateTime lördag = idag;
            DateTime söndag = idag;

            if (idag.DayOfWeek == DayOfWeek.Monday)
            {
                måndag = idag;
                tisdag = idag.AddDays(1);
                onsdag = idag.AddDays(2);
                torsdag = idag.AddDays(3);
                fredag = idag.AddDays(4);
                lördag = idag.AddDays(5);
                söndag = idag.AddDays(6);
            }
            else if (idag.DayOfWeek == DayOfWeek.Tuesday)
            {
                måndag = idag.AddDays(-1);
                tisdag = idag;
                onsdag = idag.AddDays(1);
                torsdag = idag.AddDays(2);
                fredag = idag.AddDays(3);
                lördag = idag.AddDays(4);
                söndag = idag.AddDays(5);
            }
            else if (idag.DayOfWeek == DayOfWeek.Wednesday)
            {
                måndag = idag.AddDays(-2);
                tisdag = idag.AddDays(-1);
                onsdag = idag;
                torsdag = idag.AddDays(1);
                fredag = idag.AddDays(2);
                lördag = idag.AddDays(3);
                söndag = idag.AddDays(4);
            }
            else if (idag.DayOfWeek == DayOfWeek.Thursday)
            {
                måndag = idag.AddDays(-3);
                tisdag = idag.AddDays(-2);
                onsdag = idag.AddDays(-1);
                torsdag = idag;
                fredag = idag.AddDays(1);
                lördag = idag.AddDays(2);
                söndag = idag.AddDays(3);
            }
            else if (idag.DayOfWeek == DayOfWeek.Friday)
            {
                måndag = idag.AddDays(-4);
                tisdag = idag.AddDays(-3);
                onsdag = idag.AddDays(-2);
                torsdag = idag.AddDays(-1);
                fredag = idag;
                lördag = idag.AddDays(1);
                söndag = idag.AddDays(2);
            }
            else if (idag.DayOfWeek == DayOfWeek.Saturday)
            {
                måndag = idag.AddDays(-5);
                tisdag = idag.AddDays(-4);
                onsdag = idag.AddDays(-3);
                torsdag = idag.AddDays(-2);
                fredag = idag.AddDays(-1);
                lördag = idag;
                söndag = idag.AddDays(1);
            }
            else if (idag.DayOfWeek == DayOfWeek.Sunday)
            {
                måndag = idag.AddDays(-6);
                tisdag = idag.AddDays(-5);
                onsdag = idag.AddDays(-4);
                torsdag = idag.AddDays(-3);
                fredag = idag.AddDays(-2);
                lördag = idag.AddDays(-1);
                söndag = idag;
            }

            string strängStart = måndag.ToShortDateString();
            DateTime dtBool;
            DateTime.TryParse(strängStart, out dtBool);
            måndag = dtBool;

            strängStart = tisdag.ToShortDateString();
            DateTime.TryParse(strängStart, out dtBool);
            tisdag = dtBool;

            strängStart = onsdag.ToShortDateString();
            DateTime.TryParse(strängStart, out dtBool);
            onsdag = dtBool;

            strängStart = torsdag.ToShortDateString();
            DateTime.TryParse(strängStart, out dtBool);
            torsdag = dtBool;

            strängStart = fredag.ToShortDateString();
            DateTime.TryParse(strängStart, out dtBool);
            fredag = dtBool;

            strängStart = lördag.ToShortDateString();
            DateTime.TryParse(strängStart, out dtBool);
            lördag = dtBool;

            strängStart = söndag.ToShortDateString();
            DateTime.TryParse(strängStart, out dtBool);
            söndag = dtBool;

            return new[] { måndag, tisdag, onsdag, torsdag, fredag, lördag, söndag };
        }

        private void Rensa_Textboxar_Medlem()
        {
            textBox_Medlem_Medlemsnummer.Clear();
            textBox_Medlem_Fornamn.Clear();
            textBox_Medlem_Efternamn.Clear();
            textBox_Medlem_Gatuadress.Clear();
            textBox_Medlem_Postnummer.Clear();
            textBox_Medlem_Ort.Clear();
            textBox_Medlem_Mailadress.Clear();
            textBox_Medlem_Telefon.Clear();
            textBox_Medlem_Giltig.Clear();
            dateTimePicker_Medlem_Startdatum.ResetText();

        }
        
        private void Rensa_Textboxar_Instruktor()
        {
            textBox_Instruktor_Personnummer.Clear();
            textBox_Instruktor_Fornamn.Clear();
            textBox_Instruktor_Efternamn.Clear();
            textBox_Instruktor_Gatuadress.Clear();
            textBox_Instruktor_Postnummer.Clear();
            textBox_Instruktor_Ort.Clear();
            textBox_Instruktor_Mailadress.Clear();
            textBox_Instruktor_Telefonnummer.Clear();
        }
        
        private void RensaTextboxarBokning()
        {
            textBox_Bokning_SokMedlem.Clear();
            textBox_Bokning_SokPass.Clear();

            Uppdatera_Aktivitetslista();
            Uppdatera_Bokningslista();

            listBox_Bokning_Medlem.DataSource = Databasfunktioner.GetMedlemmar();
        }
        private void ÄndraMedlemTextboxar()
        {
            bool kontroll = false;

            if (aktuellInlogg.Behorighet == "Ägare" || aktuellInlogg.Behorighet == "Reception")
            {
                kontroll = true;
            }
            else if (aktuellInlogg.Behorighet == "Medlem" && aktuellInlogg.Anvandarnamn == aktuellMedlem.Medlemsnummer)
            {                
                kontroll = true;
            }
            else
            {
                MessageBox.Show("Behörighet otillräckligt för ändring av " + aktuellMedlem.Fornamn + "s personuppgifter");
            }

            if (kontroll == true)
            {
                textBox_Medlem_Fornamn.ReadOnly = false;
                textBox_Medlem_Efternamn.ReadOnly = false;
                textBox_Medlem_Gatuadress.ReadOnly = false;
                textBox_Medlem_Postnummer.ReadOnly = false;
                textBox_Medlem_Ort.ReadOnly = false;
                textBox_Medlem_Mailadress.ReadOnly = false;
                textBox_Medlem_Telefon.ReadOnly = false;
                dateTimePicker_Medlem_Startdatum.Enabled = true;    
                textBox_Medlem_Giltig.ReadOnly = false;            
                textBox_Medlem_Fornamn.BackColor = SystemColors.Window;
                textBox_Medlem_Efternamn.BackColor = SystemColors.Window;
                textBox_Medlem_Gatuadress.BackColor = SystemColors.Window;
                textBox_Medlem_Postnummer.BackColor = SystemColors.Window;
                textBox_Medlem_Ort.BackColor = SystemColors.Window;
                textBox_Medlem_Mailadress.BackColor = SystemColors.Window;
                textBox_Medlem_Telefon.BackColor = SystemColors.Window;
                comboBox_Medlem_MedlemstypNamn.BackColor = SystemColors.Window;
                textBox_Medlem_Giltig.BackColor = SystemColors.Window;
            }
        }
        private void LåsMedlemTextboxar()
        {
            textBox_Medlem_Medlemsnummer.ReadOnly = true;
            textBox_Medlem_Fornamn.ReadOnly = true;
            textBox_Medlem_Efternamn.ReadOnly = true;
            textBox_Medlem_Gatuadress.ReadOnly = true;
            textBox_Medlem_Postnummer.ReadOnly = true;
            textBox_Medlem_Ort.ReadOnly = true;
            textBox_Medlem_Mailadress.ReadOnly = true;
            textBox_Medlem_Telefon.ReadOnly = true;
            dateTimePicker_Medlem_Startdatum.Enabled = false;               
            comboBox_Medlem_MedlemstypNamn.DataSource = Databasfunktioner.GetAktuellMedlemstyp(aktuellMedlem.Medlemsnummer);
            textBox_Medlem_Giltig.ReadOnly = true;

            textBox_Medlem_Medlemsnummer.BackColor = SystemColors.ControlLight;
            textBox_Medlem_Fornamn.BackColor = SystemColors.ControlLight;
            textBox_Medlem_Efternamn.BackColor = SystemColors.ControlLight;
            textBox_Medlem_Gatuadress.BackColor = SystemColors.ControlLight;
            textBox_Medlem_Postnummer.BackColor = SystemColors.ControlLight;
            textBox_Medlem_Ort.BackColor = SystemColors.ControlLight;
            textBox_Medlem_Mailadress.BackColor = SystemColors.ControlLight;
            textBox_Medlem_Telefon.BackColor = SystemColors.ControlLight;
            comboBox_Medlem_MedlemstypNamn.BackColor = SystemColors.ControlLight;
            textBox_Medlem_Giltig.BackColor = SystemColors.ControlLight;
        }
        private void ÄndraInstruktörsTextBoxar()
        {
            bool kontroll = false;

            if (aktuellInlogg.Behorighet == "Ägare" || aktuellInlogg.Behorighet == "Reception")
            {
                kontroll = true;
            }
            else if (aktuellInlogg.Behorighet == "Instruktör" && aktuellInlogg.Anvandarnamn == aktuellInstruktor.Instruktorsnummer)
            {                
                kontroll = true;
            }
            else
            {
                MessageBox.Show("Behörighet otillräckligt för ändring av " + aktuellInstruktor.Fornamn + "s personuppgifter");
            }

            if (kontroll == true)
            {
                textBox_Instruktor_Fornamn.ReadOnly = false;
                textBox_Instruktor_Efternamn.ReadOnly = false;
                textBox_Instruktor_Gatuadress.ReadOnly = false;
                textBox_Instruktor_Postnummer.ReadOnly = false;
                textBox_Instruktor_Ort.ReadOnly = false;
                textBox_Instruktor_Mailadress.ReadOnly = false;
                textBox_Instruktor_Telefonnummer.ReadOnly = false;

                textBox_Instruktor_Fornamn.BackColor = SystemColors.Window;
                textBox_Instruktor_Efternamn.BackColor = SystemColors.Window;
                textBox_Instruktor_Gatuadress.BackColor = SystemColors.Window;
                textBox_Instruktor_Postnummer.BackColor = SystemColors.Window;
                textBox_Instruktor_Ort.BackColor = SystemColors.Window;
                textBox_Instruktor_Mailadress.BackColor = SystemColors.Window;
                textBox_Instruktor_Telefonnummer.BackColor = SystemColors.Window;
            }            
        }
        private void LåsInstruktörsTextBoxar()
        {
            textBox_Instruktor_Personnummer.ReadOnly = true;
            textBox_Instruktor_Fornamn.ReadOnly = true;
            textBox_Instruktor_Efternamn.ReadOnly = true;
            textBox_Instruktor_Gatuadress.ReadOnly = true;
            textBox_Instruktor_Postnummer.ReadOnly = true;
            textBox_Instruktor_Ort.ReadOnly = true;
            textBox_Instruktor_Mailadress.ReadOnly = true;
            textBox_Instruktor_Telefonnummer.ReadOnly = true;

            textBox_Instruktor_Personnummer.BackColor = SystemColors.ControlLight;
            textBox_Instruktor_Fornamn.BackColor = SystemColors.ControlLight;
            textBox_Instruktor_Efternamn.BackColor = SystemColors.ControlLight;
            textBox_Instruktor_Gatuadress.BackColor = SystemColors.ControlLight;
            textBox_Instruktor_Postnummer.BackColor = SystemColors.ControlLight;
            textBox_Instruktor_Ort.BackColor = SystemColors.ControlLight;
            textBox_Instruktor_Mailadress.BackColor = SystemColors.ControlLight;
            textBox_Instruktor_Telefonnummer.BackColor = SystemColors.ControlLight;
        }
        
        private void UpdateSchema(DataGridViewRow row, Aktivitet pass)
        {
            int index = 0;            
            
            row = (DataGridViewRow)dataGridView_Schema.Rows[index].Clone();

            if (pass.Datum.DayOfWeek == DayOfWeek.Monday)
            {
                row.Cells[0].Value = pass;
            }
            else if (pass.Datum.DayOfWeek == DayOfWeek.Tuesday)
            {
                row.Cells[1].Value = pass;
            }
            else if (pass.Datum.DayOfWeek == DayOfWeek.Wednesday)
            {
                row.Cells[2].Value = pass;
            }
            else if (pass.Datum.DayOfWeek == DayOfWeek.Thursday)
            {
                row.Cells[3].Value = pass;
            }
            else if (pass.Datum.DayOfWeek == DayOfWeek.Friday)
            {
                row.Cells[4].Value = pass;
            }
            else if (pass.Datum.DayOfWeek == DayOfWeek.Saturday)
            {
                row.Cells[5].Value = pass;
            }
            else if (pass.Datum.DayOfWeek == DayOfWeek.Sunday)
            {
                row.Cells[6].Value = pass;
            }
            else
            {
                MessageBox.Show("Time to debug");
            }
            
            dataGridView_Schema.Rows.Add(row);
                                    
            row.DataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;            
            DataGridViewColumn column = dataGridView_Schema.Columns[index];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            
        }
        public void Set_Template_RowHeight(DataGridView dgv)
        {
            dataGridView_Schema.RowTemplate.Height = 80;
            //column width
            foreach (DataGridViewRow row in dataGridView_Schema.Rows)
            {
                dataGridView_Schema.RowTemplate.Height = 80;
                //column width
                row.Height = 80;
                //dataGridView_Schema.Rows.GetRowsHeight();
            }
        }
        public void Schema()
        {
            dataGridView_Schema.Rows.Clear();
            BindingList<Aktivitet> lista = new BindingList<Aktivitet>();

            DateTime toDay = new DateTime();
            toDay = DateTime.Now;
            DateTime m, t, o, to, f, l, s = toDay;            

            DateTime[] days = new DateTime[6];
            days = VeckoDagarna();

            m = days[0];
            t = days[1];
            o = days[2];
            to = days[3];
            f = days[4];
            l = days[5];
            s = days[6];

            //lista = Databasfunktioner.GetAktiviteter2();
            lista = Databasfunktioner.GetAktiviteter1(m, t, o, to, f, l, s);

            int i = 0;            
            foreach(Aktivitet aktivitetsobjekt in lista)
            {
                if(i < lista.Count)
                {
                    //Aktivitet akt2 = new Aktivitet
                    ////Aktivitet aktivitet = new Aktivitet
                    //{
                    //    Datum = aktivitetsobjekt.Datum,
                    //    Passnummer = aktivitetsobjekt.Passnummer,
                    //    TidFrån = aktivitetsobjekt.TidFrån,
                    //    TidTill = aktivitetsobjekt.TidTill,
                    //    Ledande_Instruktor = aktivitetsobjekt.Ledande_Instruktor,
                    //    Traningstyp = aktivitetsobjekt.Traningstyp,
                    //    Lokal = aktivitetsobjekt.Lokal,
                    //    MaxAntal = aktivitetsobjekt.MaxAntal
                    //};
                    try
                    {                        
                        dataGridView_Schema.RowTemplate.Height = 80;
                        //column width
                        UpdateSchema(dataGridView_Schema.Rows[i], aktivitetsobjekt);
                        
                        i++;                        
                    }
                    catch (Exception ex)
                    {
                       MessageBox.Show(ex.Message); //index out of range-exceptions ifall ingen rad existerar (index-1)
                    }                 
                }                
            }           
        }


        //KNAPPAR & KONTROLLER
        //**************************************************************************************************************
        //**************************************************************************************************************
        //Allmänna
        private void button_Form1_LoggaUt_Click(object sender, EventArgs e)
        {           
            DialogResult dialogResult = MessageBox.Show("Vill du Logga ut " + aktuellInlogg.Anvandarnamn + "?", "Logga ut?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                utloggningsKontroll = true;
                Program.CloseControl = false;
                Close();
            }
            else
                utloggningsKontroll = false;
        }
        
        //**************************************************************************************************************
        //**************************************************************************************************************
        //Medlemmar
        
        private void button_Medlem_Spara_Click(object sender, EventArgs e)
        {
            if (aktuellInlogg.Behorighet == "Ägare" || aktuellInlogg.Behorighet == "Reception" || aktuellInlogg.Anvandarnamn == aktuellMedlem.Medlemsnummer)
            {
                try
                {
                    textBox_Medlem_Giltig.ReadOnly = false;
                    Databasfunktioner.AddMedlem(textBox_Medlem_Medlemsnummer.Text, textBox_Medlem_Fornamn.Text, textBox_Medlem_Efternamn.Text, textBox_Medlem_Gatuadress.Text, textBox_Medlem_Postnummer.Text, textBox_Medlem_Ort.Text, textBox_Medlem_Telefon.Text,
                                       textBox_Medlem_Mailadress.Text, aktuellMedlemstyp.MedlemstypId, dateTimePicker_Medlem_Startdatum.Value, Convert.ToDateTime(textBox_Medlem_Giltig.Text));

                    Databasfunktioner.UpdateMedlem(textBox_Medlem_Medlemsnummer.Text, textBox_Medlem_Fornamn.Text, textBox_Medlem_Efternamn.Text, textBox_Medlem_Gatuadress.Text, textBox_Medlem_Postnummer.Text, textBox_Medlem_Ort.Text, textBox_Medlem_Telefon.Text,
                                           textBox_Medlem_Mailadress.Text, aktuellMedlemstyp.MedlemstypId, dateTimePicker_Medlem_Startdatum.Value, Convert.ToDateTime(textBox_Medlem_Giltig.Text));

                    MessageBox.Show(textBox_Medlem_Fornamn.Text + " " + textBox_Medlem_Efternamn.Text + "s uppgifter är nu sparade");
                    
                    Uppdatera_Medlemslista();
                    LåsMedlemTextboxar();  
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Behörighet otillräckligt för att spara " + aktuellMedlem.Fornamn + "s personuppgifter");
            }

        }
        private void listBox_Medlem_SelectedIndexChanged(object sender, EventArgs e)
        {
            aktuellMedlem = (Medlem)listBox_Medlem.SelectedItem;
            if (tabControl_Form1.SelectedTab == tabPage_Medlemmar && aktuellInlogg != null)
            {
                Rensa_Textboxar_Medlem();
                if (aktuellInlogg.Behorighet == "Ägare" || aktuellInlogg.Behorighet == "Reception" ||
                    aktuellInlogg.Anvandarnamn == aktuellMedlem.Medlemsnummer || aktuellInlogg.Anvandarnamn + " " == aktuellMedlem.Medlemsnummer || aktuellInlogg.Anvandarnamn + "  " == aktuellMedlem.Medlemsnummer)
                {
                    try
                    {                        
                        textBox_Medlem_Medlemsnummer.Text = aktuellMedlem.Medlemsnummer;
                        textBox_Medlem_Fornamn.Text = aktuellMedlem.Fornamn;
                        textBox_Medlem_Efternamn.Text = aktuellMedlem.Efternamn;
                        textBox_Medlem_Gatuadress.Text = aktuellMedlem.Gatuadress;
                        textBox_Medlem_Postnummer.Text = aktuellMedlem.Postnummer;
                        textBox_Medlem_Ort.Text = aktuellMedlem.Postadress;
                        textBox_Medlem_Telefon.Text = aktuellMedlem.Telefonummer;
                        textBox_Medlem_Mailadress.Text = aktuellMedlem.Mailadress;
                        dateTimePicker_Medlem_Startdatum.Text = aktuellMedlem.Startdatum.ToShortDateString();
                        comboBox_Medlem_MedlemstypNamn.DataSource = Databasfunktioner.GetAktuellMedlemstyp(aktuellMedlem.Medlemsnummer);

                        textBox_Medlem_Giltig.ReadOnly = false;
                        textBox_Medlem_Giltig.Text = aktuellMedlem.Slutdatum.ToShortDateString();
                        textBox_Medlem_Giltig.ReadOnly = true;
                        LåsMedlemTextboxar();       //för att edit-knapps-kontrollen ska slå in igen
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    //fail-safe
                }
            }            
        }

        private void button_Medlem_Tabort_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Vill du ta bort " + aktuellMedlem + "?", "Avsluta?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    Databasfunktioner.RemoveMedlem(aktuellMedlem.Medlemsnummer);
                    MessageBox.Show(aktuellMedlem + "s uppgifter är nu borttagna ur systemet");
                    Uppdatera_Medlemslista();
                    LåsMedlemTextboxar();
                }
                catch (Exception)
                {
                    MessageBox.Show(aktuellMedlem.Fornamn + " " + aktuellMedlem.Efternamn + " är inbokad på pass, avboka före borttagning");
                }
            }
        }
        private void button_Medlem_Ny_Click(object sender, EventArgs e) // Nytt namn, lägg till ny medlemknapp
        {
            Uppdatera_Medlemslista();
            Rensa_Textboxar_Medlem();
            Uppdatera_Medlemskapstypslista();

            ÄndraMedlemTextboxar();
            textBox_Medlem_Medlemsnummer.ReadOnly = false;
            textBox_Medlem_Medlemsnummer.BackColor = SystemColors.Control;
        }
        private void button_Medlem_Sök_Click(object sender, EventArgs e)
        {
            listBox_Medlem.DataSource = Databasfunktioner.searchMedlemmar(textBox_Medlem_Sok.Text);
        }

        private void button_medlem_ändra_Click(object sender, EventArgs e)
        {
            ÄndraMedlemTextboxar();            
        }
        private void comboBox_Medlem_MedlemstypNamn_MouseClick(object sender, MouseEventArgs e)
        {            
            if (textBox_Medlem_Fornamn.ReadOnly == false)    //mao ifall ändra-knappen har tryckts ner & textboxar är upplåsta
            {
                Uppdatera_Medlemskapstypslista();
            }            
        }

        private void comboBox_Medlem_MedlemstypNamn_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_Medlem_Giltig.ReadOnly = false;
            aktuellMedlemstyp = (Medlemtyp)comboBox_Medlem_MedlemstypNamn.SelectedItem;

            DateTime startDatum = dateTimePicker_Medlem_Startdatum.Value;

            string giltighet_en_manad = startDatum.AddMonths(1).ToShortDateString();
            string giltighet_tre_manad = startDatum.AddMonths(3).ToShortDateString();
            string giltighet_sex_manad = startDatum.AddMonths(6).ToShortDateString();
            string giltighet_ett_ar = startDatum.AddYears(1).ToShortDateString();

            if (aktuellMedlemstyp.Medlemskapsnamn == "1-mån")
            {
                textBox_Medlem_Giltig.Text = giltighet_en_manad;
            }
            else if (aktuellMedlemstyp.Medlemskapsnamn == "3-mån")
            {
                textBox_Medlem_Giltig.Text = giltighet_tre_manad;
            }
            else if (aktuellMedlemstyp.Medlemskapsnamn == "6-mån")
            {
                textBox_Medlem_Giltig.Text = giltighet_sex_manad;
            }
            else if (aktuellMedlemstyp.TypAvMedlemskap == "Fryst" && aktuellMedlemstyp.Medlemskapsnamn == "1-mån")
            {
                textBox_Medlem_Giltig.Text = aktuellMedlem.Slutdatum.AddMonths(1).ToShortDateString();
            }
            else if (aktuellMedlemstyp.TypAvMedlemskap == "Fryst" && aktuellMedlemstyp.Medlemskapsnamn == "2-mån")
            {
                textBox_Medlem_Giltig.Text = aktuellMedlem.Slutdatum.AddMonths(2).ToShortDateString();
            }
            else if (aktuellMedlemstyp.TypAvMedlemskap == "Fryst" && aktuellMedlemstyp.Medlemskapsnamn == "3-mån")
            {
                textBox_Medlem_Giltig.Text = aktuellMedlem.Slutdatum.AddMonths(3).ToShortDateString();
            }
            else
            {
                textBox_Medlem_Giltig.Text = giltighet_ett_ar;
            }

            textBox_Medlem_Giltig.ReadOnly = true;
        }



        //**************************************************************************************************************************************
        //**************************************************************************************************************************************
        //Instruktörer

        private void button_Instruktor_Spara_Click(object sender, EventArgs e)
        {
            if (aktuellInlogg.Behorighet == "Ägare" || aktuellInlogg.Behorighet == "Reception" || aktuellInlogg.Anvandarnamn == aktuellInstruktor.Instruktorsnummer)
            {
                try
                {                    
                    Databasfunktioner.AddInstruktor(textBox_Instruktor_Personnummer.Text, textBox_Instruktor_Fornamn.Text, textBox_Instruktor_Efternamn.Text,
                                    textBox_Instruktor_Gatuadress.Text, textBox_Instruktor_Postnummer.Text, textBox_Instruktor_Ort.Text, textBox_Instruktor_Telefonnummer.Text, textBox_Instruktor_Mailadress.Text);

                    Databasfunktioner.UpdateInstruktor(textBox_Instruktor_Personnummer.Text, textBox_Instruktor_Fornamn.Text, textBox_Instruktor_Efternamn.Text, textBox_Instruktor_Gatuadress.Text,
                                        textBox_Instruktor_Postnummer.Text, textBox_Instruktor_Ort.Text, textBox_Instruktor_Telefonnummer.Text, textBox_Instruktor_Mailadress.Text);
                    MessageBox.Show(textBox_Instruktor_Fornamn.Text + " " + textBox_Instruktor_Efternamn.Text + "s uppgifter är nu sparade i systemet");

                    Uppdatera_Instruktorslista();
                    LåsInstruktörsTextBoxar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void listBox_Instruktor_SelectedIndexChanged(object sender, EventArgs e)
        {
            aktuellInstruktor = (Instruktor)listBox_Instruktor.SelectedItem;

            textBox_Instruktor_Personnummer.Text = aktuellInstruktor.Instruktorsnummer;
            textBox_Instruktor_Fornamn.Text = aktuellInstruktor.Fornamn;
            textBox_Instruktor_Efternamn.Text = aktuellInstruktor.Efternamn;
            textBox_Instruktor_Gatuadress.Text = aktuellInstruktor.Gatuadress;
            textBox_Instruktor_Postnummer.Text = aktuellInstruktor.Postnummer;
            textBox_Instruktor_Ort.Text = aktuellInstruktor.Postadress;
            textBox_Instruktor_Telefonnummer.Text = aktuellInstruktor.Telefonnummer;
            textBox_Instruktor_Mailadress.Text = aktuellInstruktor.Mailadress;

            listBoxKompetens.DataSource = Databasfunktioner.HämtaKompetenser(aktuellInstruktor.Instruktorsnummer);
            listBoxSchemalagdaPass.DataSource = Databasfunktioner.HämtaInstruktorsSchemalagdaPass(aktuellInstruktor.Instruktorsnummer);
        }

        private void button_Instruktor_TaBort_Click(object sender, EventArgs e)
        {
            try
            {
                Databasfunktioner.RemoveInstruktor(aktuellInstruktor.Instruktorsnummer);
                MessageBox.Show(aktuellInstruktor + " är nu borttagen ur systemet");
                Uppdatera_Instruktorslista();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void button_Instruktör_ändra_Click(object sender, EventArgs e)
        {
            ÄndraInstruktörsTextBoxar();
        }

        private void button_Instruktor_Ny_Click(object sender, EventArgs e)
        {
            Uppdatera_Instruktorslista();
            Rensa_Textboxar_Instruktor();
            ÄndraInstruktörsTextBoxar();
            textBox_Instruktor_Personnummer.ReadOnly = false;
            textBox_Instruktor_Personnummer.BackColor = SystemColors.Control;
        }
        private void Button_Instruktor_Sok_Click(object sender, EventArgs e)
        {
            listBox_Instruktor.DataSource = Databasfunktioner.searchInstruktorer(textBoxSokInstruktor.Text);
        }
        private void button_Instruktor_Kompetens_Click(object sender, EventArgs e)
        {
            try
            {
                Databasfunktioner.AddKompetens(aktuellInstruktor.Instruktorsnummer, comboBox_Instruktor_Kompetens.Text);
                MessageBox.Show(comboBox_Instruktor_Kompetens.Text + " har lagts till som kompetens för " + aktuellInstruktor);
                listBoxKompetens.DataSource = Databasfunktioner.HämtaKompetenser(aktuellInstruktor.Instruktorsnummer);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        //**************************************************************************************************************************************
        //**************************************************************************************************************************************
        //Aktiviteter
        private void button_Aktivitet_LäggTillPass_Click(object sender, EventArgs e)
        {           
            try
            {                
                Databasfunktioner.AddAktiviter(aktuellInstruktor.Instruktorsnummer, aktuellKompetens.InstruktorsKompetens, aktuellLokal.Rumsnummer, dateTimePicker_Aktivitet_LäggTillPass.Value, dateTimePicker1.Value, dateTimePicker2.Value, Convert.ToInt16(textBox_MaxAntal.Text));
                MessageBox.Show(" Nytt pass har lagts till i schemaläggning ");
               
                Uppdatera_Aktivitetslista();
                Schema();                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button_Ändra_Pass_Click(object sender, EventArgs e)
        {            
            try
            {                  
                try
                {
                    comboBox_Aktivitet_Traningstyp.DataSource = Databasfunktioner.GetTraningstyp();
                    comboBox_Aktivitet_Traningstyp.Text = aktuellAktivitet.Traningstyp;                 
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                int ci = dataGridView_Schema.CurrentCell.ColumnIndex;
                int ri = dataGridView_Schema.CurrentCell.RowIndex;
                
                Databasfunktioner.UpdateAktivitet(aktuellInstruktor.Instruktorsnummer, aktuellKompetens.InstruktorsKompetens, aktuellLokal.Rumsnummer, dateTimePicker_Aktivitet_LäggTillPass.Value, dateTimePicker1.Value, dateTimePicker2.Value, Convert.ToInt16(textBox_MaxAntal.Text), aktuellAktivitet.Passnummer);
                MessageBox.Show("Passet har ändrats");
                Uppdatera_Aktivitetslista();
                Schema();

                this.dataGridView_Schema.CurrentCell = this.dataGridView_Schema[ci, ri];
                comboBox_Aktivitet_Traningstyp.Text = aktuellAktivitet.Traningstyp;    
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void comboBox_Aktivitet_Traningstyp_SelectedIndexChanged(object sender, EventArgs e)
        {
            aktuellKompetens = (Kompetens)comboBox_Aktivitet_Traningstyp.SelectedItem;
            //comboBox_Aktivitet_Instruktor.DataSource = Databasfunktioner.HämtaInstruktorerMedVissKompetens(aktuellKompetens.InstruktorsKompetens);
        }

        private void comboBox_Aktivitet_Instruktor_SelectedIndexChanged(object sender, EventArgs e)
        {
            aktuellInstruktor = (Instruktor)comboBox_Aktivitet_Instruktor.SelectedItem;
        }

        private void comboBox_Aktivitet_Lokal_SelectedIndexChanged(object sender, EventArgs e)
        {
            aktuellLokal = (Lokal)comboBox_Aktivitet_Lokal.SelectedItem;
        } 

        private void button_TaBortPass_Click(object sender, EventArgs e)   //DBF line 666
        {
            try
            {
                Databasfunktioner.RemovePass(aktuellAktivitet.Passnummer);
                MessageBox.Show("Pass borttaget");
                Uppdatera_Aktivitetslista();
            }
            catch (Exception)
            {
                MessageBox.Show("Det går ej att ta bort pass där det finns inbokade medlemmar");
            }
        }
        private void button_Aktivitet_NyttPass_Click(object sender, EventArgs e)
        {
            Uppdatera_Schema_Kontroller();            
        }

        private void dataGridView_Schema_VisibleChanged(object sender, EventArgs e)
        {
             if (sender is DataGridView && ((DataGridView)sender).Visible) { DataGridView dgv = sender as DataGridView; Set_Template_RowHeight(dgv); }
             {
                 Set_Template_RowHeight(dgv: dataGridView_Schema);
             }
        }

        private void dataGridView_Schema_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            aktuellAktivitet = (Aktivitet)dataGridView_Schema.SelectedCells[0].Value;
            if(aktuellAktivitet != null)
            {
                dateTimePicker_Aktivitet_LäggTillPass.Value = aktuellAktivitet.Datum;

                string strängStart = aktuellAktivitet.TidFrån.TimeOfDay.ToString();
                DateTime dtBool;
                DateTime.TryParse(strängStart, out dtBool);
                aktuellAktivitet.TidFrån = dtBool;
                dateTimePicker1.Value = aktuellAktivitet.TidFrån;

                strängStart = aktuellAktivitet.TidTill.TimeOfDay.ToString();
                DateTime.TryParse(strängStart, out dtBool);
                aktuellAktivitet.TidTill = dtBool;
                dateTimePicker2.Value = aktuellAktivitet.TidTill;

                comboBox_Aktivitet_Instruktor.Text = aktuellAktivitet.Ledande_Instruktor;
                comboBox_Aktivitet_Lokal.Text = aktuellAktivitet.Lokal.ToString();
                comboBox_Aktivitet_Traningstyp.Text = aktuellAktivitet.Traningstyp;
                textBox_MaxAntal.Text = aktuellAktivitet.MaxAntal.ToString();
            }
        }

        private void dataGridView_Schema_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("" + aktuellAktivitet.ToString() + "\n" + "Vill du boka " + aktuellAktivitet.Traningstyp + "?", "Boka?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    tabControl_Form1.SelectedTab = tabPage_Bokning;
                    int i = 0;
                    Uppdatera_Bokningslista();
                    foreach (Aktivitet at in listBox_Bokning_Aktivitet.Items)
                    {
                        if (at.Passnummer == aktuellAktivitet.Passnummer)
                        {
                            i = listBox_Bokning_Aktivitet.Items.IndexOf(at);
                        }
                    }
                    listBox_Bokning_Aktivitet.SetSelected(i, true);         //markerar vald aktivitet från schema men på bokningstabben
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                //do nothing, default close
            }
        }

        

        //**************************************************************************************************************************************
        //**************************************************************************************************************************************
        //Bokningar
        private void listBox_Bokning_Medlem_SelectedIndexChanged(object sender, EventArgs e)
        {
            bokningsmarkeradMedlem = (Medlem)listBox_Bokning_Medlem.SelectedItem;
            HämtaMedlemsBokningar();            
        }
        private void listBox_Bokning_Aktivitet_SelectedIndexChanged(object sender, EventArgs e)
        {
            bokningsmarkeradAktivitet = (Aktivitet)listBox_Bokning_Aktivitet.SelectedItem;
            UppdateraAntalInbokade();
        }        
        

        private void button_Bokning_Boka_Click(object sender, EventArgs e)
        {
            try
            {
                BindingList<Medlem> antal = Databasfunktioner.HämtaMedlemsID(bokningsmarkeradAktivitet.Passnummer);
                int count = 0;
                foreach (Medlem m in antal)
                {
                    count++;
                }
                if (count >= bokningsmarkeradAktivitet.MaxAntal)
                {
                    MessageBox.Show("Passet är fullbokat");
                }
                else
                {
                    Databasfunktioner.AddBokning(bokningsmarkeradAktivitet.Passnummer, bokningsmarkeradMedlem.Medlemsnummer);
                    MessageBox.Show(bokningsmarkeradMedlem + " är nu inbokad på " + bokningsmarkeradAktivitet);
                    Uppdatera_Bokningslista();
                    UppdateraAntalInbokade();
                    HämtaMedlemsBokningar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button_Bokning_Avboka_Click(object sender, EventArgs e)
        {
            bool success = false;
            if (aktuellAktivitetsBokning.Passnummer == bokningsmarkeradAktivitet.Passnummer)
            {
                success = true;
            }
            else
            {
                success = false;
            }
            
            if(success == true)
            {
                try
                {
                    Databasfunktioner.RemoveBokning(bokningsmarkeradAktivitet.Passnummer, bokningsmarkeradMedlem.Medlemsnummer);
                    MessageBox.Show(bokningsmarkeradMedlem.Fornamn + " " + bokningsmarkeradMedlem.Efternamn + " är nu avbokad från passet");
                    Uppdatera_Bokningslista();
                    UppdateraAntalInbokade();
                    räknaHämtaMedlemmarsBokadePass = 0;
                    HämtaMedlemsBokningar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                try
                {
                    Databasfunktioner.RemoveBokning(aktuellAktivitetsBokning.Passnummer, bokningsmarkeradMedlem.Medlemsnummer);
                    MessageBox.Show(bokningsmarkeradMedlem.Fornamn + " " + bokningsmarkeradMedlem.Efternamn + " är nu avbokad från passet");
                    Uppdatera_Bokningslista();
                    UppdateraAntalInbokade();
                    räknaHämtaMedlemmarsBokadePass = 0;
                    HämtaMedlemsBokningar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
           
            
        }
        private void button_Bokning_SokMedlem_Click(object sender, EventArgs e)
        {
            try
            {
                listBox_Bokning_Medlem.DataSource = Databasfunktioner.searchMedlemmar(textBox_Bokning_SokMedlem.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Sökningen gav inga resultat");
            }
        }

        private void button_Bokning_SokPass_Click(object sender, EventArgs e)
        {
            try
            {
                listBox_Bokning_Aktivitet.DataSource = Databasfunktioner.searchAktivitet(textBox_Bokning_SokPass.Text);
            }
            catch (Exception ex)
            {
                    MessageBox.Show(ex.Message);
                //MessageBox.Show("Sökningen gav inga resultat");
            }
        }

        private void button_Bokning_Rensa_Click(object sender, EventArgs e)
        {
            RensaTextboxarBokning();
        }
        private void listBox_Bokning_PassetsInbokadeMedlemmar_SelectedIndexChanged(object sender, EventArgs e)
        {
            aktuellMedlem = (Medlem)listBox_Bokning_PassetsInbokadeMedlemmar.SelectedItem;
        }
        private void listBox_Bokning_MedlemmensBokningar_SelectedIndexChanged(object sender, EventArgs e)
        {

            aktuellAktivitetsBokning = (Aktivitet)listBox_Bokning_MedlemmensBokningar.SelectedItem;
            DateTime[] days = new DateTime[6];
                days = VeckoDagarna();
            DateTime måndag = days[0];
            DateTime söndag = days[6];

            //kontroll för att endast köra efter initialization 
            if (räknaHämtaMedlemmarsBokadePass >= 2)
            {
                if (aktuellAktivitetsBokning.Datum.Date >= måndag.Date)
                {
                    if (aktuellAktivitetsBokning.Datum.Date <= söndag.Date)
                    {
                        int index = 0;
                        try
                        {
                            foreach (Aktivitet aktiv in listBox_Bokning_Aktivitet.Items)
                            {
                                if (aktiv.Passnummer == aktuellAktivitetsBokning.Passnummer)
                                {
                                    index = listBox_Bokning_Aktivitet.Items.IndexOf(aktiv);
                                }
                            }
                            listBox_Bokning_Aktivitet.SetSelected(index, true);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Bokningen är för långt fram, finns inte i aktivitetslistan");
                    }
                }
                else
                {
                    MessageBox.Show("Bokningen är för gammal, finns inte i aktivitetslistan");
                }
            }            
            //bokningsmarkeradAktivitet vs. aktuellAktivitet? 
            //Ingen motsvarande aktivitet i aktivitetslistan = felaktig aktuellAktivitet = ogiltig avbokningsmetod           
            
        }
        //**************************************************************************************************************************************
        //**************************************************************************************************************************************
        //Administration
        private void button_Administration_SetUserLevel_Click(object sender, EventArgs e)
        {
            try
            {
                Databasfunktioner.CreateUser(textBox_Administration_UserName.Text, textBox_Administration_Password.Text, comboBox_Administration_UserLevel.Text);
                MessageBox.Show("Ny användare skapad");
                Hämta_Användare_Admin();
                textBox_Administration_UserName.Clear();
                textBox_Administration_Password.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }   
        } 

        private void button_Administration_LaggTillKompetens_Click(object sender, EventArgs e)
        {
            try
            {
                comboBox_Instruktor_Kompetens.Items.Add(textBox_Administration_LaggTillKompetens.Text);
                MessageBox.Show(textBox_Administration_LaggTillKompetens.Text + " är nu tillagt i kompetenslistan");
                textBox_Administration_LaggTillKompetens.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
       

        private void button_Administration_LäggTill_Medlemskapstyp_Click(object sender, EventArgs e)
        {
            try
            {
                Databasfunktioner.AddMedlemskapstyp(textBox_Administration_LäggTill_Namn_Medlemskapstyp.Text, textBox_Administration_TypAv_Medlemskap.Text, Convert.ToInt16(textBox_Administration_pris.Text));
            Uppdatera_Medlemskapstypslista();
                HämtaMedlemstypAdminCombobox();
            MessageBox.Show(textBox_Administration_LäggTill_Namn_Medlemskapstyp.Text + " har lagts till som medlemskapstyp");
            textBox_Administration_LäggTill_Namn_Medlemskapstyp.Clear();
                textBox_Administration_TypAv_Medlemskap.Clear();
                textBox_Administration_pris.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView_Schema_SelectionChanged(object sender, EventArgs e)
        {         
             //nej.          
        }

        

        private void button_Administration_LäggTillLokal_Click(object sender, EventArgs e)
        {
            try
            {
                Databasfunktioner.AddLokal(Convert.ToInt16(textBox_Administration_Rumsnummer.Text), comboBox_Administration_LokalUtrustning.Text, textBox_Administration_LokalStorlek.Text);
                MessageBox.Show("Lokal " + textBox_Administration_Rumsnummer.Text + " är nu inlagd");
                HämtaLokaler();
                textBox_Administration_Rumsnummer.Clear();
                comboBox_Administration_LokalUtrustning.Text = "Välj";
                textBox_Administration_LokalStorlek.Clear();
            }
            catch (Exception)
            {
                MessageBox.Show("Du har förmodligen skrivit för många tecken i storlek eftersom den bara är charvar 6 i databasen och jag orkar inte ändra");
            }
        }

        private void comboBox_Administration_Lokaler_SelectedIndexChanged(object sender, EventArgs e)
        {
            aktuellLokal = (Lokal)comboBox_Administration_Lokaler.SelectedItem;
        }

        private void button_Administration_TaBortLokal_Click(object sender, EventArgs e)
        {
            try
            {
                Databasfunktioner.RemoveLokal(aktuellLokal.Rumsnummer);
                MessageBox.Show("Lokal " + aktuellLokal.Rumsnummer + " är nu borttagen");
                HämtaLokaler();
            }
            catch (Exception )
            {
                MessageBox.Show("Lokalen kan inte tas bort för det finns inbokade pass i lokalen");
            }
        }

        private void comboBox_Administration_Medlemskapstyp_SelectedIndexChanged(object sender, EventArgs e)
            {
                aktuellMedlemstyp = (Medlemtyp)comboBox_Administration_Medlemskapstyp.SelectedItem;
            }

        private void button_Administration_TaBortMedlemskapstyp_Click(object sender, EventArgs e)
        {
            try
            {
                Databasfunktioner.RemoveMedlemskapstyp(aktuellMedlemstyp.MedlemstypId);
                MessageBox.Show("Medlemskapstypen är nu borttagen");
                HämtaMedlemstypAdminCombobox();
            }
            catch (Exception)
            {
                MessageBox.Show("Det går inte att ta bort medlemskapstypen eftersom det finns medlemmar som fortfarande har aktiva medlemskap med den typen av medlemskap");
            }

        }
        private void button_Administration_TaBortAnvändare_Click(object sender, EventArgs e)
        {
            try
            {
                Databasfunktioner.DeleteUser(aktuellInlogg.Anvandarnamn);
                MessageBox.Show("Användaren är borttagen");
                Hämta_Användare_Admin();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void comboBox_Administration_Användare_SelectedIndexChanged(object sender, EventArgs e)
        {
            aktuellInlogg = (Inloggning)comboBox_Administration_Användare.SelectedItem; //Backdoor
            textBox_Administration_NyttAnvändarnamn.Text = aktuellInlogg.Anvandarnamn;
            textBox_Administration_NyttLösen.Text = aktuellInlogg.Losenord;
            comboBox_Administration_NyBehörighet.Text = aktuellInlogg.Behorighet;
        }

       

        private void button_administration_ÄndraLösen_Click(object sender, EventArgs e)
        {
            try
            {
                Databasfunktioner.UpdateUser(textBox_Administration_NyttAnvändarnamn.Text, textBox_Administration_NyttLösen.Text, comboBox_Administration_NyBehörighet.Text, aktuellInlogg.Anvandarnamn);
                MessageBox.Show("Användarens uppgifter är uppdaterade");
                Hämta_Användare_Admin();
                textBox_Administration_NyttLösen.Clear();
                textBox_Administration_NyttAnvändarnamn.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }


        private void button_Administration_TaBortPass_Click(object sender, EventArgs e)
        {
            button_TaBortPass_Click(sender, e);
        }

        private void listBox_Administration_Aktivitet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabControl_Form1.SelectedTab == tabPage_Administration )
            {
                aktuellAktivitet = (Aktivitet)listBox_Administration_Aktivitet.SelectedItem;
            }
        }



        //skräp nedan
        private void dataGridView_Schema_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //nej.
        }

       

       

       

        







        
        






        
















    }

}


        
        

       


