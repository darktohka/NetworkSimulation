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
    public partial class FloorExplorer : Form
    {
        public FloorLayout layout;
        public int floor;
        public List<Button> buttons;

        public FloorExplorer(int floor)
        {
            this.floor = floor;
            this.layout = Settings.GetSingleton().GetFloor(floor);
            InitializeComponent();
        }

        private void FloorExplorer_Load(object sender, EventArgs e)
        {
            this.Text = "Floor #" + (floor + 1);
            ReloadGrid();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        public void ReloadGrid()
        {

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            using (Font myFont = new Font("Arial", 14))
            {
                e.Graphics.DrawString("Switch", myFont, Brushes.Green, new Point(0, pictureBox1.Height - 25));
            }
        }
        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            using (Font myFont = new Font("Arial", 14))
            {
                e.Graphics.DrawString("Hub", myFont, Brushes.Green, new Point(0, pictureBox1.Height - 25));
            }
        }
        private void pictureBox3_Paint(object sender, PaintEventArgs e)
        {
            using (Font myFont = new Font("Arial", 14))
            {
                e.Graphics.DrawString("Router", myFont, Brushes.Green, new Point(0, pictureBox1.Height - 25));
            }
        }
        private void pictureBox4_Paint(object sender, PaintEventArgs e)
        {
            using (Font myFont = new Font("Arial", 14))
            {
                e.Graphics.DrawString("Modem", myFont, Brushes.Green, new Point(0, pictureBox1.Height - 25));
            }
        }
        private void pictureBox5_Paint(object sender, PaintEventArgs e)
        {
            using (Font myFont = new Font("Arial", 14))
            {
                e.Graphics.DrawString("Computer", myFont, Brushes.Green, new Point(0, pictureBox1.Height - 25));
            }
        }
        private void pictureBox6_Paint(object sender, PaintEventArgs e)
        {
            using (Font myFont = new Font("Arial", 14))
            {
                e.Graphics.DrawString("Powerline", myFont, Brushes.Green, new Point(0, pictureBox1.Height - 25));
            }
        }
        private void pictureBox7_Paint(object sender, PaintEventArgs e)
        {
            using (Font myFont = new Font("Arial", 14))
            {
                e.Graphics.DrawString("Wall", myFont, Brushes.Green, new Point(0, pictureBox1.Height - 25));
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
