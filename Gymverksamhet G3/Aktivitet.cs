using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gymverksamhet_G3
{
    class Aktivitet
    {
        //PROPERTIES

        public DateTime Datum { get; set; }
        public int Passnummer { get; set; }
        public string Traningstyp { get; set; }
       
        public DateTime TidFrån { get; set; }
        public DateTime TidTill { get; set; } 
        public string Ledande_Instruktor { get; set; }
        public int Lokal { get; set; }
        public int MaxAntal { get; set; }

        public DateTime SetTidFrån(DateTime Tiden)
        {
            
            TidFrån = default(DateTime).Add(Tiden.TimeOfDay);
            return TidFrån;
        }
        //METODER
        public override string ToString()
        {        
            string nl = Environment.NewLine;
            return Datum.ToShortDateString() + nl + " kl: " + TidFrån.ToShortTimeString() + "-" + TidTill.ToShortTimeString() + " " + nl + Traningstyp + "\t" + nl
                   + " Rum: " + Lokal + "\t" + nl + /*Ledande_Instruktor + "\t" + nl + "Passnummer: " + Passnummer + "\t" + nl +*/ "Max antal platser: " + MaxAntal;
        }


        }
    }

