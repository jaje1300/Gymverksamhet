using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gymverksamhet_G3
{
    class Medlem
    {
        //PROPERTIES
        public string Medlemsnummer { get; set; }
        public string Fornamn { get; set; }
        public string Efternamn { get; set; }
        public string Gatuadress { get; set; }
        public string Postnummer { get; set; }
        public string Postadress { get; set; }
        public string Telefonummer { get; set; }
        public string Mailadress { get; set; }
        public int Medlemstyp { get; set; }
        public DateTime Startdatum { get; set; }
        public DateTime Slutdatum { get; set; }
        
        //METODER
        public override string ToString()
        {
            return Fornamn + "\t " + Efternamn;
        }

        
    }
}
