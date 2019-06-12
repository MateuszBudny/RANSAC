using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RANSAC {
    class AffineRegion : Point {

        public List<int> Params { get; set; }

        public AffineRegion(int x, int y, List<int> parameters) : base(x, y) {
            Params = parameters;
        }
    }
}
