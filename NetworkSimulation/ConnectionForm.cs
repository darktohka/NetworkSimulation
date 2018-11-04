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
    public partial class ConnectionForm : Form
    {
        public int objectId;
        public NetworkObject from;
        public List<NetworkCable> connectedCables = new List<NetworkCable>();
        public List<NetworkObject> potentialCons = new List<NetworkObject>();

        public ConnectionForm(int objectId)
        {
            this.objectId = objectId;
            this.from = Settings.GetSingleton().GetObject(objectId);
            InitializeComponent();
        }

        private void ConnectionForm_Load(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            connectedCables.Clear();

            List<NetworkCable> conns = Settings.GetSingleton().GetConnections(objectId);
            List<int> connectedIds = new List<int>();

            foreach (int trace in from.GetGraphTrace())
            {
                connectedIds.Add(trace);
            }

            conns.Sort((x, y) => x.GetSecondObject(from).GetFloor().CompareTo(y.GetSecondObject(from).GetFloor()));

            foreach (NetworkCable cable in conns)
            {
                NetworkObject second = cable.GetSecondObject(from);
                connectedIds.Add(second.GetObjectId());
                listBox1.Items.Add("Floor " + (second.GetFloor() + 1) + "'s " + second.GetObjectTypeName() + " - " + second.GetName());
                connectedCables.Add(cable);
            }

            potentialCons.Clear();

            foreach (NetworkObject obj in Settings.GetSingleton().GetObjects()) {
                if (obj.GetObjectId() == objectId || Math.Abs(obj.GetFloor() - from.GetFloor()) > 1 || connectedIds.Contains(obj.GetObjectId()))
                {
                    continue;
                }

                potentialCons.Add(obj);
            }

            potentialCons.Sort((x, y) => x.GetFloor().CompareTo(y.GetFloor()));

            foreach (NetworkObject obj in potentialCons) {
                listBox2.Items.Add("Floor " + (obj.GetFloor() + 1) + "'s " + obj.GetObjectTypeName() + " - " + obj.GetName());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                return;
            }

            if (MessageBox.Show("Are you sure you want to remove this connection?", "Are you sure?", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            NetworkCable connectedCable = connectedCables[listBox1.SelectedIndex];
            connectedCables.RemoveAt(listBox1.SelectedIndex);
            connectedCable.RemoveCable();
            Settings.SaveSettings();
            listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            MessageBox.Show("Successfully removed connection!");
            Close();
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1)
            {
                return;
            }

            if (from.GetConnectionCount() >= from.GetMaxConnections())
            {
                MessageBox.Show("All of our ports on the device " + from.GetName() + " are filled! Please remove a connection first.");
                return;
            }

            NetworkObject obj = potentialCons[listBox2.SelectedIndex];
                
            if (obj.GetConnectionCount() >= obj.GetMaxConnections())
            {
                MessageBox.Show("All of our ports on the device " + obj.GetName() + " are filled! Please remove a connection on " + obj.GetName() + " first.");
                return;
            }
            potentialCons.RemoveAt(listBox2.SelectedIndex);
            NetworkCable cable = new NetworkCable(Settings.GetSingleton().AllocateCableId(), from.GetObjectId(), obj.GetObjectId());
            Settings.GetSingleton().AddNetworkCable(cable);
            Settings.SaveSettings();
            listBox2.Items.RemoveAt(listBox2.SelectedIndex);
            MessageBox.Show("Wow! Successfully connected to the " + obj.GetObjectTypeName() + " " + obj.GetName() + " on floor " + (obj.GetFloor() + 1) + "!");
            Close();
        }
    }
}
