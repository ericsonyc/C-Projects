using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vision
{
    class Picture
    {
        public List<int[]> way0 = new List<int[]>();
        public List<int[]> way1 = new List<int[]>();
        public List<int[]> way2 = new List<int[]>();
        public List<int[]> way3 = new List<int[]>();
        public string id = null;
        public Picture(string id)
        {
            this.id = id;
        }
    }
}
