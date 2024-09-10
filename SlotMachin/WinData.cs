using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlotMachin
{
    internal class WinData
    {

        public List<int> posList { get; set; }
        public string symbolName { get; set; }
        public float winAmount { get; set; }

        public int ways { get; set; }
        public float basePayout{ get; set; }

        public Hashtable symCountOnEachCol = new Hashtable();
    }
}
