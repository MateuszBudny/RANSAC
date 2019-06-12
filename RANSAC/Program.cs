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

            string firstImageName = "podloga300";
            string secondImageName = "stol300";

            // pairs with whichever similarity
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            AffineRegionsPairs affineRegionsPairs = new AffineRegionsPairs(firstImageName + ".png.haraff.sift", secondImageName + ".png.haraff.sift");
            stopwatch.Stop();
            Console.WriteLine("Liczba par: " + affineRegionsPairs.Pairs.Count + ", czas wyznaczania par: " + stopwatch.ElapsedMilliseconds / 1000d + "s");

            List<(Point, Point)> lines = affineRegionsPairs.GetPairsAsPointsTuples();

            // pairs with high similarity
            AffineRegionsPairs highSimilarityPairs = affineRegionsPairs.GetPairsWithHighSimilarity(0.95);
            Console.WriteLine("Liczba par z wysokim podobieństwem: " + highSimilarityPairs.Pairs.Count);

            stopwatch.Restart();
            Matrix<double> affineModel = RANSAC.GetBestModel(highSimilarityPairs, new AffineTransformation());
            List<AffineRegionsPair> properAffinePairs = RANSAC.GetProperPairs(affineRegionsPairs, affineModel, new AffineTransformation());
            stopwatch.Stop();
            Console.WriteLine("Liczba poprawnych par (według transformaty afinicznej): " + properAffinePairs.Count + ", czas sprawdzania par: " + stopwatch.ElapsedMilliseconds / 1000d + "s");

            stopwatch.Restart();
            Matrix<double> homographyModel = RANSAC.GetBestModel(highSimilarityPairs, new HomographyTransformation());
            List<AffineRegionsPair> properHomographyPairs = RANSAC.GetProperPairs(affineRegionsPairs, homographyModel, new HomographyTransformation());
            stopwatch.Stop();
            Console.WriteLine("Liczba poprawnych par (według transformaty homograficznej): " + properHomographyPairs.Count + ", czas sprawdzania par: " + stopwatch.ElapsedMilliseconds / 1000d + "s");



            List<(Point, Point)> properAffineLines = properAffinePairs.Select(p => (new Point((int)p.FirstImageRegion.X, (int)p.FirstImageRegion.Y),
                new Point((int)p.SecondImageRegion.X, (int)p.SecondImageRegion.Y))).ToList();

            List<(Point, Point)> properHomographyLines = properHomographyPairs.Select(p => (new Point((int)p.FirstImageRegion.X, (int)p.FirstImageRegion.Y),
                new Point((int)p.SecondImageRegion.X, (int)p.SecondImageRegion.Y))).ToList();

            Image firstImage = Image.FromFile(firstImageName + ".png");
            Image secondImage = Image.FromFile(secondImageName + ".png");

            Application.EnableVisualStyles();
            Application.Run(new ImagesDisplay(firstImage, secondImage,
                lines,
                properAffineLines,
                properHomographyLines
             ));
        }
    }
}
