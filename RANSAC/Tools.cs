using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RANSAC {
    public static class Tools {
        public static int GetDotProduct(this List<int> listA, List<int> listB) {
            return listA.Zip(listB, (a, b) => a * b).Sum();
        }

        public static double GetMagnitude(this List<int> list) {
            return Math.Sqrt(list.Select(n => n *= n).Sum());
        }

        public static double GetCosineSimilarity(this List<int> listA, List<int> listB) {
            return listA.GetDotProduct(listB) / (listA.GetMagnitude() * listB.GetMagnitude());
        }
    }
}
