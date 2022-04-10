using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PardavimoAutomatai.Models
{
    public class VendorNeedUnitSlot
    {
        //`Id`, `Price`, `Count`, `Name`, `fk_ProductType`, `fk_VendorMachine`
        public VendorNeedUnitSlot() { }

        public int id { get; set; }
        public string name { get; set; }
        public string product { get; set; }
        public int productID { get; set; }
        public int vendor { get; set; }
        public int count { get; set; }
        public int need { get; set; }
        public int left { get; set; }

    }
}