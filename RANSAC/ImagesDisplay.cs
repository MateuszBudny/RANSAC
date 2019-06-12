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
            ProperLinesOneByOne = 3,
            ProperLinesImmediately = 4
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
        List<(Point, Point)> properLines;
        List<(Point, Point)> linesToDraw;
        Random rand = new Random();
        List<(Point, Point)>.Enumerator linesEnumerator;
        List<(Point, Point)>.Enumerator properLinesEnumerator;
        const double SCALE_CHANGE = 0.1d;

        public ImagesDisplay(Image firstImage, Image secondImage, List<(Point, Point)> lines, List<(Point, Point)> properLines) {
            WindowState = FormWindowState.Maximized;
            InitializeComponent();
            this.firstImage = firstImage;
            this.secondImage = secondImage;
            
            this.lines = lines;
            this.properLines = properLines;
            linesEnumerator = lines.GetEnumerator();
            properLinesEnumerator = properLines.GetEnumerator();
            linesToDraw = new List<(Point, Point)>();
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
                case ImagesDisplayState.ProperLinesOneByOne:
                    if (properLinesEnumerator.MoveNext()) {
                        DrawLine(properLinesEnumerator.Current, e.Graphics);
                    } else {
                        state++;
                    }
                    break;
                case ImagesDisplayState.ProperLinesImmediately:
                    foreach ((Point, Point) line in properLines) {
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
