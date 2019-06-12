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

        private enum ImagesDisplayState {
            Start = 0,
            AllLinesOneByOne = 1,
            AllLinesImmediately = 2,
            ProperAffineLinesOneByOne = 3,
            ProperAffineLinesImmediately = 4,
            ProperHomographyLinesOneByOne = 5,
            ProperHomographyLinesImmediately = 6
        };

        private ImagesDisplayState state = ImagesDisplayState.Start;
        Image firstImage;
        Image secondImage;
        Rectangle firstImageRect;
        Rectangle secondImageRect;
        double imageScale = 1d;
        int WidthImageSize => (int) (firstImage.Width * imageScale);
        int HeightImageSize => (int) (firstImage.Height * imageScale);
        List<(Point, Point)> lines;
        List<(Point, Point)> properAffineLines;
        List<(Point, Point)> properHomographyLines;
        Random rand = new Random();
        List<(Point, Point)>.Enumerator linesEnumerator;
        List<(Point, Point)>.Enumerator properAffineLinesEnumerator;
        List<(Point, Point)>.Enumerator properHomographyLinesEnumerator;
        const double SCALE_CHANGE = 0.1d;

        public ImagesDisplay(Image firstImage, Image secondImage, List<(Point, Point)> lines, List<(Point, Point)> properAffineLines, List<(Point, Point)> properHomographyLines) {
            WindowState = FormWindowState.Maximized;
            InitializeComponent();
            this.firstImage = firstImage;
            this.secondImage = secondImage;

            this.lines = lines;
            this.properAffineLines = properAffineLines;
            this.properHomographyLines = properHomographyLines;
            linesEnumerator = lines.GetEnumerator();
            properAffineLinesEnumerator = properAffineLines.GetEnumerator();
            properHomographyLinesEnumerator = properHomographyLines.GetEnumerator();
        }

        private void ImagesDisplay_Load(object sender, EventArgs e) {

        }

        private void ImagesDisplay_Paint(object sender, PaintEventArgs e) {
            firstImageRect = new Rectangle(0, 0, WidthImageSize, HeightImageSize);
            secondImageRect = new Rectangle(WidthImageSize, 0, WidthImageSize, HeightImageSize);

            e.Graphics.DrawImage(firstImage, firstImageRect);
            e.Graphics.DrawImage(secondImage, secondImageRect);

            switch(state) {
                case ImagesDisplayState.Start:
                    state++;
                    break;
                case ImagesDisplayState.AllLinesOneByOne:
                    if (linesEnumerator.MoveNext()) {
                        DrawLine(linesEnumerator.Current, e.Graphics);
                    } else {
                        state++;
                    }
                    break;
                case ImagesDisplayState.AllLinesImmediately:
                    foreach ((Point, Point) line in lines) {
                        DrawLine(line, e.Graphics);
                    }
                    break;
                case ImagesDisplayState.ProperAffineLinesOneByOne:
                    if (properAffineLinesEnumerator.MoveNext()) {
                        DrawLine(properAffineLinesEnumerator.Current, e.Graphics);
                    } else {
                        state++;
                    }
                    break;
                case ImagesDisplayState.ProperAffineLinesImmediately:
                    foreach ((Point, Point) line in properAffineLines) {
                        DrawLine(line, e.Graphics);
                    }
                    break;
                case ImagesDisplayState.ProperHomographyLinesOneByOne:
                    if (properHomographyLinesEnumerator.MoveNext()) {
                        DrawLine(properHomographyLinesEnumerator.Current, e.Graphics);
                    } else {
                        state++;
                    }
                    break;
                case ImagesDisplayState.ProperHomographyLinesImmediately:
                    foreach ((Point, Point) line in properHomographyLines) {
                        DrawLine(line, e.Graphics);
                    }
                    break;
            }
        }

        private void DrawLine((Point, Point) line, Graphics graphics) {
            Pen pen = new Pen(Color.FromArgb(255, rand.Next(256), rand.Next(256), rand.Next(256)));
            System.Drawing.Point firstPoint = new System.Drawing.Point((int)(line.Item1.X * imageScale), (int)(line.Item1.Y * imageScale));
            System.Drawing.Point secondPoint = new System.Drawing.Point((int)(line.Item2.X * imageScale) + WidthImageSize, (int)(line.Item2.Y * imageScale));
            graphics.DrawLine(pen, firstPoint, secondPoint);
        }

        private void ImagesDisplay_MouseDown(object sender, MouseEventArgs e) {
            Invalidate();
        }

        private void ImagesDisplay_KeyDown(object sender, KeyEventArgs e) {
            if(e.KeyCode == Keys.Add) {
                imageScale += SCALE_CHANGE;
                Invalidate();
            } else if(e.KeyCode == Keys.Subtract) {
                imageScale -= SCALE_CHANGE;
                Invalidate();
            } else if (e.KeyCode == Keys.S) {
                state++;
                Invalidate();
            }
        }
    }
}
