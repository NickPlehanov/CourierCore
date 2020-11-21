using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourierCore.Models {
    public class Guests_GuestDeliveries_Clients {
        public Guid GestID{ get; set; }
        public DateTime? GestDateOpen{ get; set; }
        public DateTime? GestDateClose{ get; set; }
        public string GestName{ get; set; }
        public string GestComment{ get; set; }
        public Guid? CourierID { get; set; }
        public int DlvrstID { get; set; }
        public Guid? ClntID { get; set; }
        public string ClntName { get; set; }
        public string ClntPhones { get; set; }
    }
}
