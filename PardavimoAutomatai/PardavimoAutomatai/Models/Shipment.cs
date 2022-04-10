using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PardavimoAutomatai.Models
{
    public class Shipment
    {
        //SELECT `Id`, `StartDate`, `ComfirmDate`, `EndDate`, `Done`, `fk_Admin`, `fk_VendorMachine`, `fk_Supplier` FROM `shipment` WHERE 1'
        public Shipment() { needs = new List<ShipmentNeeds>(); }

        public int id { get; set; }
        public string start { get; set; }
        public string comfirm { get; set; }
        public string end { get; set; }
        public string done { get; set; }
        public int admin { get; set; }
        public int vendor { get; set; }
        public int supplier { get; set; }

        public List<ShipmentNeeds> needs;
    }
}