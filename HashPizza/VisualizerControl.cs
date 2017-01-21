using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HashPizza
{
    public partial class VisualizerControl : UserControl
    {
        private Solution solution;

        public VisualizerControl(Solution solution)
        {
            InitializeComponent();

            DoubleBuffered = true;
            ResizeRedraw = true;

            this.solution = solution;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.Clear(Color.Yellow);

            float cellW = Width / (float)solution.pizza.NumCols;
            float cellH = Height / (float)solution.pizza.NumRows;

            foreach (var slice in solution.Slices)
            {
                int x1 = (int)(slice.Left * cellW);
                int y1 = (int)(slice.Top * cellH);
                int x2 = (int)((slice.Right + 1) * cellW);
                int y2 = (int)((slice.Bottom + 1)* cellH);

                int width = x2 - x1;
                int height = y2 - y1;

                e.Graphics.FillRectangle(Brushes.Red, new Rectangle(x1, y1, width, height));

                if (cellW > 10 && cellH > 10)
                {
                    e.Graphics.DrawRectangle(Pens.Black, new Rectangle(x1, y1, width, height));
                }
            }
        }
    }
}
