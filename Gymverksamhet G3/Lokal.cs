using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gymverksamhet_G3
{
    class Lokal
    {
        //PROPERTIES
        public int Rumsnummer { get; set; }
        public string Utrustning { get; set; }
        public string Storlek { get; set; }
        
        //METODER
        public override string ToString()
        {
            return Convert.ToString(Rumsnummer);
        }
    }
}
