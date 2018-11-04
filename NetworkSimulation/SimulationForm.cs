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
    public partial class SimulationForm : Form
    {
        public int objectId;
        public NetworkObject networkObject;

        public SimulationForm(int objectId)
        {
            this.objectId = objectId;
            this.networkObject = Settings.GetSingleton().GetObject(objectId);
            InitializeComponent();
        }

        private void SimulationForm_Load(object sender, EventArgs e)
        {
            label14.Text = networkObject.GetConnectionStateString();
            label15.Text = networkObject.GetTrueUploadMbps() + " mbps";
            label16.Text = networkObject.GetTrueDownloadMbps() + " mbps";
            label17.Text = networkObject.GetFinalPingRate() + " ms";
            label18.Text = networkObject.GetFinalPacketLossChance() + "%";
            label19.Text = networkObject.GetUploadMbpsUsage() + " mbps";
            label20.Text = networkObject.GetDownloadMbpsUsage() + " mbps";
            MbpsUsage uploadUsage = networkObject.GetTotalUploadMbpsUsage();
            MbpsUsage downloadUsage = networkObject.GetTotalDownloadMbpsUsage();
            label21.Text = uploadUsage.currentUsage + " mbps";
            label22.Text = downloadUsage.currentUsage + " mbps";
            label23.Text = uploadUsage.maxUsage + " mbps";
            label24.Text = downloadUsage.maxUsage + " mbps";

            if (uploadUsage.IsOverloaded())
            {
                label25.Text = "UPLOAD OVERLOADED";
                label25.ForeColor = Color.DarkRed;
            } else if (downloadUsage.IsOverloaded())
            {
                label25.Text = "DOWNLOAD OVERLOADED";
                label25.ForeColor = Color.DarkRed;
            }
            else if (networkObject.GetUploadMbpsUsage() > networkObject.GetTrueUploadMbps())
            {
                label25.Text = "UPLOAD NIC BOTTLENECK";
                label25.ForeColor = Color.DarkRed;
            }
            else if (networkObject.GetDownloadMbpsUsage() > networkObject.GetTrueDownloadMbps())
            {
                label25.Text = "DOWNLOAD NIC BOTTLENECK";
                label25.ForeColor = Color.DarkRed;
            } else
            {
                label25.Text = "ALL OPTIMAL";
                label25.ForeColor = Color.Green;
            }
        }
    }
}
