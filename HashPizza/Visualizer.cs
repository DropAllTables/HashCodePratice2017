using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HashPizza
{
    public partial class Visualizer : Form
    {
        public Visualizer(Solution solution)
        {
            InitializeComponent();

            VisualizerControl control = new VisualizerControl(solution);
            control.Dock = DockStyle.Fill;

            Controls.Add(control);
        }
    }
}
