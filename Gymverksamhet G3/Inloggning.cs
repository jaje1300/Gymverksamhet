using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gymverksamhet_G3
{
    class Inloggning
    {
        public string Anvandarnamn { get; set; }
        public string Losenord { get; set; }
        public string Behorighet { get; set; }
        public static Inloggning DenInloggadeAnvändaren
        {
            get;
            set;
        }

        public Inloggning ()
        { }

        public Inloggning(string a, string l, string b)
        {
            Anvandarnamn = a;
            Losenord = l;
            Behorighet = b;
        }
        //METODER

        public override string ToString()
        {
            return Anvandarnamn + "\t    " + Losenord + "\t    " + Behorighet;
        }


    }
}
