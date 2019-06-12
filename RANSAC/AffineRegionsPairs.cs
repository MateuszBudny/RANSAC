using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RANSAC {
    class AffineRegionsPairs {
        public List<AffineRegion> FirstImageRegions { get; set; }
        public List<AffineRegion> SecondImageRegions { get; set; }
        public List<AffineRegionsPair> Pairs { get; set; }
        private Random rand = new Random();

        public AffineRegionsPairs(string pathToFirstImage, string pathToSecondImage) {
            FirstImageRegions = Loader.GetAffineRegions(pathToFirstImage);
            SecondImageRegions = Loader.GetAffineRegions(pathToSecondImage);
            CalculatePairs();
        }

        private void CalculatePairs() {
            List<AffineRegionsPair> pairsOfFirstImage = GetPairsOfOneSide(FirstImageRegions, SecondImageRegions);
            List<AffineRegionsPair> pairsOfSecondImage = GetPairsOfOneSide(SecondImageRegions, FirstImageRegions);

            Pairs = GetOnlyMutualPairs(pairsOfFirstImage, pairsOfSecondImage);
        }

        private List<AffineRegionsPair> GetPairsOfOneSide(List<AffineRegion> mainRegions, List<AffineRegion> compareToRegions) {

            List<AffineRegionsPair> pairsOfMainSide = new List<AffineRegionsPair>();

            foreach (AffineRegion m in mainRegions) {
                double bestSimilarity = -1; // by definition, cosine similarity varies between -1 and 1
                AffineRegion mostSimilarRegion = null;
                foreach (AffineRegion c in compareToRegions) {
                    double similarity = m.Params.GetCosineSimilarity(c.Params);
                    if (similarity > bestSimilarity && similarity > 0.95) { // heuristic
                        bestSimilarity = similarity;
                        mostSimilarRegion = c;
                    }
                }
                pairsOfMainSide.Add(new AffineRegionsPair(m, mostSimilarRegion));
            }

            return pairsOfMainSide;
        }

        private List<AffineRegionsPair> GetOnlyMutualPairs(List<AffineRegionsPair> pairsOfFirstImage, List<AffineRegionsPair> pairsOfSecondImage) {
            List<AffineRegionsPair> mutualPairs = new List<AffineRegionsPair>();

            foreach(AffineRegionsPair f in pairsOfFirstImage) {
                List<AffineRegionsPair> secondImagePairsWhichHasFirstImageRegion = pairsOfSecondImage.FindAll(p => p.SecondImageRegion == f.FirstImageRegion);
                foreach (AffineRegionsPair s in secondImagePairsWhichHasFirstImageRegion) {
                    if (f.SecondImageRegion == s.FirstImageRegion) {
                        mutualPairs.Add(f);
                    }
                }
            }

            return mutualPairs;
        }

        public AffineRegionsPair GetRandom() {
            return Pairs[rand.Next(Pairs.Count)];
        }
    }
}
