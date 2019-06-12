using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RANSAC {
    class RANSAC {

        const int ITERATION_NUM = 10000;
        const double MAX_ERROR = 5;

        public static Matrix<double> GetBestModel(AffineRegionsPairs pairs, Transformation transformation) {
            Matrix<double> bestModel = null;
            double bestScore = 0;

            for (int i = 0; i < ITERATION_NUM; i++) {
                List<AffineRegionsPair> samples = transformation.GetRandomSample(pairs);
                Matrix<double> model = transformation.CalculateModel(samples);
                double score = 0;

                foreach(AffineRegionsPair pair in pairs.Pairs) {
                    double error = transformation.ModelError(model, pair);
                    if(error < MAX_ERROR) {
                        score++;
                    }
                }

                if(score > bestScore) {
                    bestScore = score;
                    bestModel = model;
                }
            }

            return bestModel;
        }

        public static List<AffineRegionsPair> GetProperPairs(AffineRegionsPairs pairs, Matrix<double> model, Transformation transformation) {
            List<AffineRegionsPair> properPairs = new List<AffineRegionsPair>();

            foreach (AffineRegionsPair pair in pairs.Pairs) {
                double error = transformation.ModelError(model, pair);
                if (error < MAX_ERROR) {
                    properPairs.Add(pair);
                }
            }

            return properPairs;
        }
    }
}
