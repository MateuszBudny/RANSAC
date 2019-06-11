using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RANSAC {
    class AffineRegionsPair {
        public AffineRegion FirstImageRegion { get; set; }
        public AffineRegion SecondImageRegion { get; set; }

        public AffineRegionsPair(AffineRegion firstImageRegion, AffineRegion secondImageRegion) {
            FirstImageRegion = firstImageRegion;
            SecondImageRegion = secondImageRegion;
        }
    }
}
