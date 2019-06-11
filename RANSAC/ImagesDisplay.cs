using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RANSAC {
    public partial class ImagesDisplay : Form {
        Image firstImage;
        Image secondImage;
        Rectangle firstImageRect;
        Rectangle secondImageRect;
        double imageScale = 1d;
        int WidthImageSize => (int) (firstImage.Width * imageScale);
        int HeightImageSize => (int) (firstImage.Height * imageScale);
        List<(Point, Point)> lines;
        List<(Point, Point)> linesToDraw;
        Random rand = new Random();
        List<(Point, Point)>.Enumerator linesEnumerator;
        const double SCALE_CHANGE = 0.1d;
        bool endOfLinesEnumerator = false;

        public ImagesDisplay(Image firstImage, Image secondImage, List<(Point, Point)> lines) {
            WindowState = FormWindowState.Maximized;
            InitializeComponent();
            this.firstImage = firstImage;
            this.secondImage = secondImage;
            
            this.lines = lines;
            linesEnumerator = lines.GetEnumerator();
            linesToDraw = new List<(Point, Point)>();
        }

        private void ImagesDisplay_Load(object sender, EventArgs e) {

        }

        private void ImagesDisplay_Paint(object sender, PaintEventArgs e) {
            firstImageRect = new Rectangle(0, 0, WidthImageSize, HeightImageSize);
            secondImageRect = new Rectangle(WidthImageSize, 0, WidthImageSize, HeightImageSize);

            e.Graphics.DrawImage(firstImage, firstImageRect);
            e.Graphics.DrawImage(secondImage, secondImageRect);

            if(!endOfLinesEnumerator) {
                DrawLine(linesEnumerator.Current, e.Graphics);
            } else {
                foreach((Point, Point) line in lines) {
                    DrawLine(line, e.Graphics);
                }
            }
        }

        private void DrawLine((Point, Point) line, Graphics graphics) {
            Pen pen = new Pen(Color.FromArgb(255, rand.Next(256), rand.Next(256), rand.Next(256)));
            Point firstPoint = new Point((int)(line.Item1.X * imageScale), (int)(line.Item1.Y * imageScale));
            Point secondPoint = new Point((int)(line.Item2.X * imageScale) + WidthImageSize, (int)(line.Item2.Y * imageScale));
            graphics.DrawLine(pen, firstPoint, secondPoint);
        }

        private void ImagesDisplay_MouseDown(object sender, MouseEventArgs e) {
            if(!linesEnumerator.MoveNext()) {
                if(!endOfLinesEnumerator) {
                    endOfLinesEnumerator = true;
                    Invalidate();
                }
            } else {
                linesToDraw.Add(linesEnumerator.Current);
                Invalidate();
            }
        }

        private void ImagesDisplay_KeyDown(object sender, KeyEventArgs e) {
            if(e.KeyCode == Keys.Add) {
                imageScale += SCALE_CHANGE;
                Invalidate();
            } else if(e.KeyCode == Keys.Subtract) {
                imageScale -= SCALE_CHANGE;
                Invalidate();
            } else if(e.KeyCode == Keys.S) {
                endOfLinesEnumerator = true;
                Invalidate();
            }
        }
    }
}
