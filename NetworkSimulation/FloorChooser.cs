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
    public partial class FloorChooser : Form
    {
        private int maxX, maxY;
        public FloorChooser()
        {
            InitializeComponent();
        }

        private void FloorChooser_Load(object sender, EventArgs e)
        {
            ReloadFloors();
        }

        public void ReloadFloors() {
            floorBox.Items.Clear();
            int i = 0;

            foreach (FloorLayout layout in Settings.GetSingleton().GetFloors())
            {
                floorBox.Items.Add("Floor #" + (i + 1));
                i++;
            }
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if (floorBox.SelectedIndex == -1)
            {
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete floor #" + (floorBox.SelectedIndex + 1) + "?", "Hmm!", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }
        
            int floorNum = floorBox.SelectedIndex;
            Settings.GetSingleton().RemoveFloor(floorNum);
            Settings.SaveSettings();
            ReloadFloors();
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            if (floorBox.SelectedIndex == -1)
            {
                return;
            }

            FloorExplorer explorer = new FloorExplorer(floorBox.SelectedIndex);
            Hide();
            explorer.ShowDialog();
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int maxX = (int) floorWidthNum.Value;
            int maxY = (int) floorHeightNum.Value;

            Settings.GetSingleton().AddFloor(new FloorLayout(maxX, maxY));
            Settings.SaveSettings();
            ReloadFloors();
        }
    }
}
