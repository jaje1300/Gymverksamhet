using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gymverksamhet_G3
{
    class Medlemtyp: Medlem
    {
        //PROPERTIES
        public int MedlemstypId { get; set; }
        public string TypAvMedlemskap { get; set; }
        public string Medlemskapsnamn { get; set; }
        public int Pris { get; set; }

        //METODER

        public override string ToString()
        {
            return TypAvMedlemskap + "\t    " + Medlemskapsnamn + "\t    " + Pris + "kr";
        }
       
        
        
        
    }
}
