using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RANSAC {
    class Program {

        [STAThread]
        static void Main(string[] args) {

            string firstImageName = "marcin1";
            string secondImageName = "podloga300";

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            AffineRegionsPairs affineRegionsPairs = new AffineRegionsPairs(firstImageName + ".png.haraff.sift", secondImageName + ".png.haraff.sift");
            stopwatch.Stop();
            Console.WriteLine("Liczba par: " + affineRegionsPairs.Pairs.Count + ", czas wyznaczania par: " + stopwatch.ElapsedMilliseconds / 1000d + "s");

            List<(Point, Point)> lines = affineRegionsPairs.Pairs.Select(p => (new Point((int)p.FirstImageRegion.X, (int)p.FirstImageRegion.Y),
                new Point((int)p.SecondImageRegion.X, (int)p.SecondImageRegion.Y))).ToList();

            stopwatch.Restart();
            Matrix<double> model = RANSAC.GetBestModel(affineRegionsPairs, new AffineTransformationMatrix());
            List<AffineRegionsPair> properPairs = RANSAC.GetProperPairs(affineRegionsPairs, model, new AffineTransformationMatrix());
            stopwatch.Stop();
            Console.WriteLine("Liczba poprawnych par (według transformaty afinicznej): " + properPairs.Count + ", czas sprawdzania par: " + stopwatch.ElapsedMilliseconds / 1000d + "s");

            List<(Point, Point)> properLines = properPairs.Select(p => (new Point((int)p.FirstImageRegion.X, (int)p.FirstImageRegion.Y),
                new Point((int)p.SecondImageRegion.X, (int)p.SecondImageRegion.Y))).ToList();

            Image firstImage = Image.FromFile(firstImageName + ".png");
            Image secondImage = Image.FromFile(secondImageName + ".png");

            Application.EnableVisualStyles();
            Application.Run(new ImagesDisplay(firstImage, secondImage, lines, properLines));
        }
    }
}
