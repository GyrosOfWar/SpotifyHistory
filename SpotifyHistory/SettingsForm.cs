using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpotifyHistory
{
    public partial class SettingsForm : Form
    {
        public string filePath;
        public int refreshInterval;
        public SettingsForm()
        {
            InitializeComponent();
            refreshInterval = 10;
            numericUpDown1.Text = refreshInterval.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int number;
            bool success = int.TryParse(numericUpDown1.Text, out number);
            if (success)
                refreshInterval = number;
            this.Hide();
        }
    }
}
