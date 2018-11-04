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
        public Dictionary<Point, PictureBox> buttons = new Dictionary<Point, PictureBox>();
        public ObjectType dragType = ObjectType.NONE;
        public Point selectedPoint = new Point(-1, -1);

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

            foreach (Control control in groupBox2.Controls)
            {
                control.Hide();
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        public PictureBox GetButton(int i, int j)
        {
            return buttons[new Point(i, j)];
        }

        public GridObject GetGridObj(int i, int j)
        {
            return Settings.GetSingleton().GetGridObject(floor, i, j);
        }

        public void ReloadGrid()
        {
            foreach (PictureBox button in buttons.Values)
            {
                Controls.Remove(button);
            }

            buttons.Clear();

            int currentX = 280;
            int currentY = 50;
            int incrementX = (75 * 8) / layout.GetMaxY();
            int incrementY = (75 * 8) / layout.GetMaxX();

            for (int i = 0; i < layout.GetMaxX(); i++)
            {
                for (int j = 0; j < layout.GetMaxY(); j++)
                {
                    PictureBox button = new PictureBox();
                    button.MinimumSize = new Size(i, j);
                    button.Name = "grid-" + i + "-" + j;
                    button.Size = new Size(incrementX, incrementY);
                    button.Location = new Point(currentX, currentY);
                    button.BackgroundImageLayout = ImageLayout.Stretch;
                    button.Click += Button_Click;
                    Controls.Add(button);
                    buttons[new Point(i, j)] = button;
                    currentX += incrementX;
                }

                currentX = 280;
                currentY += incrementY;
            }

            ReloadPictures();
        }

        private void Button_Click(object sender, EventArgs e)
        {
            // TODO: Open up the new box
            PictureBox button = (PictureBox) sender;
            Point point = new Point(button.MinimumSize.Width, button.MinimumSize.Height);
            GridObject gridObj = GetGridObj(point.X, point.Y);

            DeselectPoint();

            if (gridObj == null)
            {
                if (this.dragType == ObjectType.WALL)
                {
                    Wall wall = new Wall(floor, point.X, point.Y);
                    Settings.GetSingleton().AddWall(wall);
                    Settings.SaveSettings();
                } else if (this.dragType != ObjectType.NONE)
                {
                    NetworkObject obj = new NetworkObject(dragType, Settings.GetSingleton().AllocateObjectId(), "Object", floor, point.X, point.Y, 100.0, 100.0, 0.0, 0.0, 5, 0, 3, new List<Action>(), ComputerType.WORKSTATION, true, 10.0);
                    Settings.GetSingleton().AddObject(obj);
                    Settings.SaveSettings();
                }
                
                ReloadPictures();
                StopDragAndDrop();
            } else
            {
                selectedPoint = point;

                button1.Show();

                if (gridObj is Wall)
                {
                    groupBox2.Text = "Wall";
                }
                else if (gridObj is NetworkObject)
                {
                    NetworkObject networkObj = ((NetworkObject)gridObj);
                    groupBox2.Text = networkObj.GetName();
                    ObjectType type = networkObj.GetObjectType();

                    if (type == ObjectType.ROUTER || type == ObjectType.WIFI_EXTENDER)
                    {
                        wifiRangeLabel.Show();
                        numericUpDown8.Show();
                        numericUpDown8.Value = (int) networkObj.GetWifiRange();
                    }

                    z.Show();
                    label1.Show();
                    label2.Show();
                    label3.Show();
                    label4.Show();
                    label5.Show();
                    label6.Show();
                    label7.Show();
                    label8.Show();
                    numericUpDown1.Show();
                    numericUpDown2.Show();
                    numericUpDown3.Show();
                    numericUpDown4.Show();
                    numericUpDown5.Show();
                    numericUpDown6.Show();
                    numericUpDown7.Show();
                    z.Text = networkObj.GetObjectTypeName();
                    numericUpDown1.Value = (int)networkObj.uploadMbps;
                    numericUpDown2.Value = (int)networkObj.downloadMbps;
                    numericUpDown3.Value = (int)networkObj.throttledUploadMbps;
                    numericUpDown4.Value = (int)networkObj.throttledDownloadMbps;
                    numericUpDown5.Value = networkObj.GetAvgPingRate();
                    numericUpDown6.Value = networkObj.GetPacketLossChance();
                    numericUpDown7.Value = networkObj.GetMaxConnections();

                    if (type == ObjectType.COMPUTER)
                    {
                        wifiEnabledLabel.Show();
                        computerTypeLabel.Show();
                        actionsLabel.Show();
                        actionsBox.Show();
                        newActionLabel.Show();
                        activityLabel.Show();
                        downloadActLabel.Show();
                        uploadActLabel.Show();
                        createActButton.Show();
                        removeActButton.Show();
                        checkBox1.Show();
                        comboBox1.Show();
                        numericUpDown9.Show();
                        numericUpDown10.Show();
                        activityName.Show();

                        checkBox1.Checked = networkObj.GetWifiEnabled();
                        comboBox1.SelectedIndex = (int) networkObj.GetComputerType();
                        numericUpDown9.Value = 0;
                        numericUpDown10.Value = 0;
                        activityName.Text = "";
                    }
                }
            }
        }

        public void ReloadPictures()
        {
            foreach (PictureBox box in buttons.Values)
            {
                UpdatePicture(box);
            }
        }

        public void UpdatePicture(PictureBox box)
        {
            GridObject obj = GetGridObj(box.MinimumSize.Width, box.MinimumSize.Height);
 
            if (obj == null)
            {
                box.BackgroundImage = Properties.Resources.empty;
            }
            else if (obj is Wall)
            {
                box.BackgroundImage = Properties.Resources.wall;
            }
            else if (obj is NetworkObject)
            {
                NetworkObject networkObj = (NetworkObject)obj;
                box.BackgroundImage = networkObj.GetImage();
            }
        }

        public void EnableDragAndDrop()
        {
            foreach (PictureBox box in buttons.Values)
            {
                if (GetGridObj(box.MinimumSize.Width, box.MinimumSize.Height) == null)
                {
                    box.BackgroundImage = Properties.Resources.free_space;
                }
            }
        }
   
        private void FloorExplorer_Click(object sender, System.EventArgs e)
        {
            StopDragAndDrop();
        }

        public void DrawHighlightText(PaintEventArgs e, string text, PictureBox box)
        {
            using (Font myFont = new Font("Arial", 14))
            {
                e.Graphics.DrawString(text, myFont, Brushes.Green, new Point(0, box.Height - 25));
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            DrawHighlightText(e, "Switch", pictureBox1);
        }
        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            DrawHighlightText(e, "Hub", pictureBox2);
        }
        private void pictureBox3_Paint(object sender, PaintEventArgs e)
        {
            DrawHighlightText(e, "Router", pictureBox3);
        }
        private void pictureBox4_Paint(object sender, PaintEventArgs e)
        {
            DrawHighlightText(e, "Modem", pictureBox4);
        }
        private void pictureBox5_Paint(object sender, PaintEventArgs e)
        {
            DrawHighlightText(e, "Computer", pictureBox5);
        }

        private void pictureBox6_Paint(object sender, PaintEventArgs e)
        {
            DrawHighlightText(e, "Powerline", pictureBox6);
        }

        private void pictureBox7_Paint(object sender, PaintEventArgs e)
        {
            DrawHighlightText(e, "Wall", pictureBox7);
        }

        public void StartDragAndDrop(ObjectType objectType)
        {
            this.dragType = objectType;
            EnableDragAndDrop();
        }

        public void StopDragAndDrop()
        {
            if (this.dragType != ObjectType.NONE)
            {
                this.dragType = ObjectType.NONE;
                ReloadPictures();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            StartDragAndDrop(ObjectType.SWITCH);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            StartDragAndDrop(ObjectType.HUB);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            StartDragAndDrop(ObjectType.MODEM);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            StartDragAndDrop(ObjectType.ROUTER);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            StartDragAndDrop(ObjectType.POWERLINE);
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            StartDragAndDrop(ObjectType.COMPUTER);
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            StartDragAndDrop(ObjectType.WALL);
        }

        private void DeselectPoint()
        {
            foreach (Control control in groupBox2.Controls)
            {
                control.Hide();
            }

            groupBox2.Text = "Object";
            selectedPoint = new Point(-1, -1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (selectedPoint.X != -1)
            {
                GridObject gridObj = GetGridObj(selectedPoint.X, selectedPoint.Y);
                Settings.GetSingleton().RemoveGridObject(gridObj);
                Settings.SaveSettings();
                ReloadPictures();
                DeselectPoint();
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void actionsBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void z_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (selectedPoint.X != -1)
            {
                NetworkObject gridObj = (NetworkObject)GetGridObj(selectedPoint.X, selectedPoint.Y);
                gridObj.SetComputerType((ComputerType) comboBox1.SelectedIndex);
                Settings.SaveSettings();
            }
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {

            if (selectedPoint.X != -1)
            {
                NetworkObject gridObj = (NetworkObject)GetGridObj(selectedPoint.X, selectedPoint.Y);
                gridObj.SetWifiRange((double)numericUpDown8.Value);
                Settings.SaveSettings();
            }
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {

            if (selectedPoint.X != -1)
            {
                NetworkObject gridObj = (NetworkObject)GetGridObj(selectedPoint.X, selectedPoint.Y);
                gridObj.SetMaxConnections((int)numericUpDown7.Value);
                Settings.SaveSettings();
            }
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {

            if (selectedPoint.X != -1)
            {
                NetworkObject gridObj = (NetworkObject)GetGridObj(selectedPoint.X, selectedPoint.Y);
                gridObj.SetPacketLossChance((int)numericUpDown6.Value);
                Settings.SaveSettings();
            }
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {

            if (selectedPoint.X != -1)
            {
                NetworkObject gridObj = (NetworkObject)GetGridObj(selectedPoint.X, selectedPoint.Y);
                gridObj.SetAvgPingRate((int) numericUpDown5.Value);
                Settings.SaveSettings();
            }
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {

            if (selectedPoint.X != -1)
            {
                NetworkObject gridObj = (NetworkObject)GetGridObj(selectedPoint.X, selectedPoint.Y);
                gridObj.throttledDownloadMbps = (double)numericUpDown4.Value;
                Settings.SaveSettings();
            }
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {

            if (selectedPoint.X != -1)
            {
                NetworkObject gridObj = (NetworkObject)GetGridObj(selectedPoint.X, selectedPoint.Y);
                gridObj.throttledUploadMbps = (double)numericUpDown3.Value;
                Settings.SaveSettings();
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

            if (selectedPoint.X != -1)
            {
                NetworkObject gridObj = (NetworkObject)GetGridObj(selectedPoint.X, selectedPoint.Y);
                gridObj.downloadMbps = (double)numericUpDown2.Value;
                Settings.SaveSettings();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

            if (selectedPoint.X != -1)
            {
                NetworkObject gridObj = (NetworkObject) GetGridObj(selectedPoint.X, selectedPoint.Y);
                gridObj.uploadMbps = (double) numericUpDown1.Value;
                Settings.SaveSettings();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            if (selectedPoint.X != -1)
            {
                NetworkObject gridObj = (NetworkObject)GetGridObj(selectedPoint.X, selectedPoint.Y);
                gridObj.SetWifiEnabled(checkBox1.Checked);
                Settings.SaveSettings();
            }
        }
    }
}
