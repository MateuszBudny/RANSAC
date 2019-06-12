using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RANSAC {
    class AffineTransformationMatrix : Transformation {

        public AffineTransformationMatrix() {}

        public override List<AffineRegionsPair> GetRandomSample(AffineRegionsPairs pairs) {
            return GetRandomSample(pairs, 3);
        }

        public override Matrix<double> CalculateModel(List<AffineRegionsPair> pairs) {
            int[] x, y, u, v;
            x = new int[3];
            y = new int[3];
            u = new int[3];
            v = new int[3];
            for (int i = 0; i < 3; i++) {
                (x[i], y[i], u[i], v[i]) = pairs[i].GetPairsCoordinates();
            }

            Matrix<double> basePairsMatrix = DenseMatrix.OfArray(new double[,] {
                { x[0], y[0], 1, 0, 0, 0 },
                { x[1], y[1], 1, 0, 0, 0 },
                { x[2], y[2], 1, 0, 0, 0 },
                { 0, 0, 0, x[0], y[0], 1 },
                { 0, 0, 0, x[1], y[1], 1 },
                { 0, 0, 0, x[2], y[2], 1 }
            });

            Vector<double> transformedVector = DenseVector.OfArray(new double[] {
                u[0],
                u[1],
                u[2],
                v[0],
                v[1],
                v[2]
            });

            Matrix<double> resultMatrix = basePairsMatrix.Inverse() * transformedVector.ToColumnMatrix();

            return GetAffineMatrixOfVector(resultMatrix.Column(0));
        }

        private Matrix<double> GetAffineMatrixOfVector(Vector<double> abcdef) {
            return DenseMatrix.OfArray(new double[,] {
                { abcdef[0], abcdef[1], abcdef[2] },
                { abcdef[3], abcdef[4], abcdef[5] },
                { 0, 0, 1 }
            });
        }

        protected override Point Transform(AffineRegion region, Matrix<double> model) {
            int x = region.X;
            int y = region.Y;

            Vector<double> baseVector = DenseVector.OfArray(new double[] {
                x,
                y,
                1
            });

            Vector<double> transformedVector = model * baseVector;

            return new Point((int) transformedVector[0], (int) transformedVector[1]);
        }
    }
}
