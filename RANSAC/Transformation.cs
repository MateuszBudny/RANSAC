using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RANSAC {
    abstract class Transformation {

        public abstract List<AffineRegionsPair> GetRandomSample(AffineRegionsPairs pairs);

        public abstract Matrix<double> CalculateModel(List<AffineRegionsPair> pairs);

        public List<AffineRegionsPair> GetRandomSample(AffineRegionsPairs pairs, int samplesNum) {
            List<AffineRegionsPair> samples = new List<AffineRegionsPair>();

            for (int i = 0; i < samplesNum; i++) {

                AffineRegionsPair pair;
                do {
                    pair = pairs.GetRandom();
                } while (samples.Contains(pair));
                samples.Add(pair);
            }

            return samples;
        }

        public double ModelError(Matrix<double> model, AffineRegionsPair pair) {
            AffineRegion baseRegion = pair.SecondImageRegion;
            Point transformedRegion = Transform(pair.FirstImageRegion, model);

            return baseRegion.CalculateDistance(transformedRegion);
        }

        protected abstract Point Transform(AffineRegion region, Matrix<double> model);
    }
}
