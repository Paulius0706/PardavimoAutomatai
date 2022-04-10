using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PardavimoAutomatai.Models
{
    public class ShipmentNeeds
    {
        //SELECT `Id`, `SlotName`, `Count`, `State`, `fk_Shipment`, `fk_ProductType` FROM `shipmentneeds` WHERE 1
        public int id { get; set; }
        public string slotname{ get; set; }
        public int count { get; set; }
        public string state { get; set; }
        public int shipment { get; set; }
        public int product { get; set; }
    }
}