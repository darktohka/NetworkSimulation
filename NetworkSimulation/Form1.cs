using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetworkSimulation
{
    public partial class Form1 : Form
    {
        private int maxX, maxY;
        public Form1()
        {
            InitializeComponent();
        }


        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            maxX = (int)numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            maxY = (int)numericUpDown2.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Settings.GetSingleton().AddFloor(new FloorLayout(Settings.GetSingleton().GetFloorCount()+1, maxX, maxY));
        }
    }
}
