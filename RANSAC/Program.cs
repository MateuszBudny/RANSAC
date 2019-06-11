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
            string secondImageName = "marcin2";

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            AffineRegionsPairs affineRegionsPairs = new AffineRegionsPairs(firstImageName + ".png.haraff.sift", secondImageName + ".png.haraff.sift");
            stopwatch.Stop();
            Console.WriteLine("Liczba par: " + affineRegionsPairs.Pairs.Count + ", czas wyznaczania par: " + stopwatch.ElapsedMilliseconds / 1000d + "s");

            Image firstImage = Image.FromFile(firstImageName + ".png");
            Image secondImage = Image.FromFile(secondImageName + ".png");
            List<(Point, Point)> lines = affineRegionsPairs.Pairs.Select(p => (new Point((int)p.FirstImageRegion.X, (int)p.FirstImageRegion.Y),
                new Point((int)p.SecondImageRegion.X, (int)p.SecondImageRegion.Y))).ToList();
            Application.EnableVisualStyles();
            Application.Run(new ImagesDisplay(firstImage, secondImage, lines));
        }
    }
}
