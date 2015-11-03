using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gymverksamhet_G3
{
    class Bokning
    {
        //PROPERTIES
        public int PassnummerID { get; set; }
        public string MedlemsID { get; set; }

        //METODER
        public override string ToString()
        {
            return PassnummerID + "\t" + MedlemsID;
        }
      
    }
}
