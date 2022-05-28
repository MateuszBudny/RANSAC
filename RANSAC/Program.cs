using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RANSAC {
    internal class Program {

        [STAThread]
        private static void Main(string[] args) {

            string firstImageName = "podloga300";
            string secondImageName = "stol300";

            // pairs with whichever similarity
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            AffineRegionsPairs affineRegionsPairs = new AffineRegionsPairs(firstImageName + ".png.haraff.sift", secondImageName + ".png.haraff.sift");
            stopwatch.Stop();
            Console.WriteLine("Pairs num: " + affineRegionsPairs.Pairs.Count + ", pairing time: " + stopwatch.ElapsedMilliseconds / 1000d + "s");

            List<(Point, Point)> lines = affineRegionsPairs.GetPairsAsPointsTuples();

            // pairs with high similarity
            AffineRegionsPairs highSimilarityPairs = affineRegionsPairs.GetPairsWithHighSimilarity(0.95);
            Console.WriteLine("Pairs num with high probability: " + highSimilarityPairs.Pairs.Count);

            stopwatch.Restart();
            Matrix<double> affineModel = RANSAC.GetBestModel(highSimilarityPairs, new AffineTransformation());
            List<AffineRegionsPair> properAffinePairs = RANSAC.GetProperPairs(affineRegionsPairs, affineModel, new AffineTransformation());
            stopwatch.Stop();
            Console.WriteLine("Correct pairs num (according to affine transformation): " + properAffinePairs.Count + ", checking pairs time: " + stopwatch.ElapsedMilliseconds / 1000d + "s");

            stopwatch.Restart();
            Matrix<double> homographyModel = RANSAC.GetBestModel(highSimilarityPairs, new HomographyTransformation());
            List<AffineRegionsPair> properHomographyPairs = RANSAC.GetProperPairs(affineRegionsPairs, homographyModel, new HomographyTransformation());
            stopwatch.Stop();
            Console.WriteLine("Correct pairs num (according to affine transformation): " + properHomographyPairs.Count + ", checking pairs time: " + stopwatch.ElapsedMilliseconds / 1000d + "s");



            List<(Point, Point)> properAffineLines = properAffinePairs.Select(p => (new Point(p.FirstImageRegion.X, p.FirstImageRegion.Y),
                new Point(p.SecondImageRegion.X, p.SecondImageRegion.Y))).ToList();

            List<(Point, Point)> properHomographyLines = properHomographyPairs.Select(p => (new Point(p.FirstImageRegion.X, p.FirstImageRegion.Y),
                new Point(p.SecondImageRegion.X, p.SecondImageRegion.Y))).ToList();

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
