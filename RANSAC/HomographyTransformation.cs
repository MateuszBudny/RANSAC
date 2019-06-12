using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace RANSAC {
    class HomographyTransformation : Transformation {
        public override Matrix<double> CalculateModel(List<AffineRegionsPair> pairs) {
            int[] x, y, u, v;
            x = new int[4];
            y = new int[4];
            u = new int[4];
            v = new int[4];
            for (int i = 0; i < 4; i++) {
                (x[i], y[i], u[i], v[i]) = pairs[i].GetPairsCoordinates();
            }

            Matrix<double> basePairsMatrix = DenseMatrix.OfArray(new double[,] {
                { x[0], y[0], 1, 0, 0, 0, -u[0]*x[0], -u[0]*y[0] },
                { x[1], y[1], 1, 0, 0, 0, -u[1]*x[1], -u[1]*y[1] },
                { x[2], y[2], 1, 0, 0, 0, -u[2]*x[2], -u[2]*y[2] },
                { x[3], y[3], 1, 0, 0, 0, -u[3]*x[3], -u[3]*y[3] },
                { 0, 0, 0, x[0], y[0], 1, -v[0]*x[0], -v[0]*y[0] },
                { 0, 0, 0, x[1], y[1], 1, -v[1]*x[1], -v[1]*y[1] },
                { 0, 0, 0, x[2], y[2], 1, -v[2]*x[2], -v[2]*y[2] },
                { 0, 0, 0, x[3], y[3], 1, -v[3]*x[3], -v[3]*y[3] },
            });

            Vector<double> transformedVector = DenseVector.OfArray(new double[] {
                u[0],
                u[1],
                u[2],
                u[3],
                v[0],
                v[1],
                v[2],
                v[3]
            });

            Matrix<double> resultMatrix = basePairsMatrix.Inverse() * transformedVector.ToColumnMatrix();

            return GetHomographyMatrixOfVector(resultMatrix.Column(0));
        }

        private Matrix<double> GetHomographyMatrixOfVector(Vector<double> abcdefgh) {
            return DenseMatrix.OfArray(new double[,] {
                { abcdefgh[0], abcdefgh[1], abcdefgh[2] },
                { abcdefgh[3], abcdefgh[4], abcdefgh[5] },
                { abcdefgh[6], abcdefgh[7], 1 }
            });
        }

        public override List<AffineRegionsPair> GetRandomSample(AffineRegionsPairs pairs) {
            return GetRandomSample(pairs, 4);
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

            double t = transformedVector[2];
            int u = (int) (transformedVector[0] / t);
            int v = (int) (transformedVector[1] / t);

            return new Point(u, v);
        }
    }
}
