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

            if (gridObj == null)
            {
                if (this.dragType == ObjectType.WALL)
                {
                    Wall wall = new Wall(floor, point.X, point.Y);
                    Settings.GetSingleton().AddWall(wall);
                    Settings.SaveSettings();
                }

                ReloadPictures();
                StopDragAndDrop();
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
    }
}
