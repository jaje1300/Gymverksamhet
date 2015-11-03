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
using System.Configuration;

namespace Gymverksamhet_G3
{
    public partial class Login : Form
    {
        //private const string conString = "grp3vt14";        
        bool isLoggedIn = false;
        public Login()
        {
            InitializeComponent();
        }       
        

        private void button_Login_Click(object sender, EventArgs e)
        {
            try
            {
                string user = textBox_Login_User.Text;
                string pass = textBox_Login_Pass.Text;
                
                Inloggning aktuellInlog = new Inloggning();
                aktuellInlog = Databasfunktioner.GetLogin(user, pass); 
                                
                if (aktuellInlog.Anvandarnamn == user && aktuellInlog.Losenord == pass)
                {
                    Inloggning.DenInloggadeAnvändaren = (Inloggning)aktuellInlog;                                           //lagra objektets värden
                    isLoggedIn = true;
                    this.DialogResult = DialogResult.OK;                                                                    //Skicka resultat OK
                    MessageBox.Show("Välkommen " + aktuellInlog.Behorighet);
                    this.Close(); 
                }
                else                                                                                                        //Lösen EJ ok
                {
                    isLoggedIn = false;
                    MessageBox.Show("Du har angivit fel lösenord eller användarnamn");
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button_Login_Avsluta_Click(object sender, EventArgs e)
        {
            Program.CloseControl = true;
            isLoggedIn = true;
            Close();
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(isLoggedIn == false)
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    DialogResult result = MessageBox.Show("Vill du verkligen stänga av programmet?", "Warning", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {                        
                        Program.CloseControl = true;
                        Close();
                    }
                    else
                    {
                        Program.CloseControl = false;
                    }
                }
                else
                {
                    Program.CloseControl = false;
                }
            }
            
        }



        //METODER

    }
}
