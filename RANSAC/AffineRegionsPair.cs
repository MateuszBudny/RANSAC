using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RANSAC {
    class AffineRegionsPair {
        public AffineRegion FirstImageRegion { get; set; }
        public AffineRegion SecondImageRegion { get; set; }
        public double Similarity { get; set; }

        public AffineRegionsPair(AffineRegion firstImageRegion, AffineRegion secondImageRegion, double similarity) {
            FirstImageRegion = firstImageRegion;
            SecondImageRegion = secondImageRegion;
            Similarity = similarity;
        }

        public (int x, int y, int u, int v) GetPairsCoordinates() {
            (int x, int y) = FirstImageRegion.GetPairCoordinates();
            (int u, int v) = SecondImageRegion.GetPairCoordinates();

            return (x, y, u, v);
        }
    }
}
