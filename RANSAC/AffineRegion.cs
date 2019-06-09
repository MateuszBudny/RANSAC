using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RANSAC {
    class AffineRegion {
        public double X { get; set; }
        public double Y { get; set; }
        public List<int> Params { get; set; }

        public AffineRegion(double x, double y, List<int> parameters) {
            X = x;
            Y = y;
            Params = parameters;
        }
    }
}
