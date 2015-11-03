using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gymverksamhet_G3
{
    class Kompetens
    {
        public string InstruktorsKompetens { get; set; }
        public string InstruktorsID { get; set; }

        public override string ToString()
        {
            return InstruktorsKompetens;
        }




    }


}
