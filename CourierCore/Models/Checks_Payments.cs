using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourierCore.Models {
    public class Checks_Payments {
        public Guid ChckID { get; set; }
        public string PytpName { get; set; }
        public decimal ChpySum { get; set; }
    }
}
