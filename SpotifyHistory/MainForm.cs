using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace SpotifyHistory
{
    public partial class MainForm : Form
    {
        private ProcessParser parser;
        private SettingsForm sf;
        private int refreshInterval;
        private bool isRefreshing;
        private const string filename = "SongList.txt";
        private Thread updateThread;
        private Thread writeToFileThread;
        private delegate void SetTextCallback(string t);

        public MainForm()
        {
            parser = new ProcessParser();
            InitializeComponent();
            if (File.Exists(filename))
                parser.ReadFile(filename);
            richTextBox1.Text = parser.ToString();
            sf = new SettingsForm();
            isRefreshing = true;
            refreshInterval = 5;
            updateThread = new Thread(new ThreadStart(UpdateLoop));
            updateThread.Start();
            writeToFileThread = new Thread(new ThreadStart(WriteLoop));
            //writeToFileThread.Start();
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            if (!isRefreshing)
            {
                updateButton.Text = "Stop Updating";
                isRefreshing = true;
            }
            else
            {
                updateButton.Text = "Start Updating";
                isRefreshing = false;
            }
        }

        private void WriteToFile()
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                foreach (string s in parser.SongList)
                {
                    sw.WriteLine(s);
                }
            }
        }

        private void WriteButton_Click(object sender, EventArgs e)
        {
            WriteToFile();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sf.Activate();
            sf.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WriteToFile();
            sf.Close();
            this.Close();
            Application.Exit();
        }

        private void UpdateLoop()
        {
            while (true)
            {
                parser.UpdateList();
                this.SetText(parser.ToString());
                Thread.Sleep(refreshInterval * 1000);
            }
        }
        private void SetText(string t)
        {
            if (this.richTextBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { t });
            }
            else
            {
                this.richTextBox1.Text = t;
            }
        }
        private void WriteLoop()
        {
            while (true)
            {
                if (!isRefreshing)
                    break;
                WriteToFile();
                Thread.Sleep(refreshInterval * 1000);
            }
        }
    }
}