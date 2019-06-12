using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RANSAC {
    class Loader {
        public static List<AffineRegion> GetAffineRegions(string pathToFile) {
            List<AffineRegion> affineRegions = new List<AffineRegion>();
            string[] lines = File.ReadAllLines(pathToFile);
            lines = lines.Skip(2).ToArray();
            foreach(string line in lines) {
                string[] splittedLine = line.Split(' ');
                int x = (int) double.Parse(splittedLine[0], CultureInfo.InvariantCulture);
                int y = (int) double.Parse(splittedLine[1], CultureInfo.InvariantCulture);
                List<int> parameters = new List<int>();
                for(int i = 5; i < 133; i++) {
                    parameters.Add(int.Parse(splittedLine[i]));
                }

                affineRegions.Add(new AffineRegion(x, y, parameters));
            }

            return affineRegions;
        } 
    }
}
