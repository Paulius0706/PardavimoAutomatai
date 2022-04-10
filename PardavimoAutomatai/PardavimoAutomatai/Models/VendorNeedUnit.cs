using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PardavimoAutomatai.Models
{
    public class VendorNeedUnit
    {

        public VendorNeedUnit() { slots = new List<VendorNeedUnitSlot>(); }

        public int id { get; set; }
        public int slotCount { get; set; }
        public string model { get; set; }
        public string state { get; set; }
        public string location { get; set; }

        public List<VendorNeedUnitSlot> slots;

    }
}