using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gymverksamhet_G3
{
    static class Program
    {
        private static bool closingController = false;

        public static bool CloseControl
        {
            get { return closingController; }
            set { closingController = value; }
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        { 
           
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            while(!closingController)
            {
                Login loginForm = new Login();              //Kör formuläret Login
                DialogResult dr = loginForm.ShowDialog();
                if (dr == DialogResult.OK)               //Login OK?
                {
                    Application.Run(new Form1());       //Här körs Formulär ett              
                }
                else
                {
                    // Application.Exit();               //Failsafe stänger ner applikationen
                }
            }
            
            
        }
    }
}
