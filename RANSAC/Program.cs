using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RANSAC {
    class Program {
        static void Main(string[] args) {
            List<AffineRegion> affineRegions = Loader.GetAffineRegions("lozko300.png.haraff.sift");
            //Console.WriteLine(affineRegions[0].X + ", " + affineRegions[0].Y + ", " + affineRegions[0].Params.Count + ", " + affineRegions[0].Params[0] + ", " + affineRegions[0].Params[100] + ", " + affineRegions[0].Params[127]);

        }
    }
}
