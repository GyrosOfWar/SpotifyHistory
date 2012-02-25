using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Globalization;

namespace SpotifyHistory
{
    public class ProcessParser
    {
        private Dictionary<DateTime, string> songList;
        public Dictionary<DateTime, string> SongList
        {
            get { return songList; }
        }

        public ProcessParser()
        {
            songList = new Dictionary<DateTime, string>();
        }

        public void ReadFile(string path)
        {
            if (!File.Exists(path))
                throw new IOException("File not found: " + path);

            string[] lines = File.ReadAllLines(path);
            string[] songs = ParseSongs(lines);
            DateTime[] timestamps = ParseDates(lines);
            for (int i = 0; i < lines.Length; i++)
            {
                songList.Add(timestamps[i], songs[i]);
            }

        }

        private DateTime[] ParseDates(string[] lines)
        {
            DateTime[] retVal = new DateTime[lines.Length];

            for(int i = 0; i < lines.Length; i++)
            {
                string date = lines[i].Substring(1, 19);
                DateTime dt = DateTime.Parse(date);
                retVal[i] = dt;
            }

            return retVal;
        }

        private string[] ParseSongs(string[] lines)
        {
            string[] retVal = new string[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                retVal[i] = lines[i].Substring(23);
            }
            return retVal;
        }

        private string GetSongName()
        {
            Process[] pList = Process.GetProcesses();
            string windowName = null;
            for (int i = 0; i < pList.Length; i++)
            {
                string name = pList[i].MainWindowTitle;
                if (name.StartsWith("Spotify - "))
                    windowName = pList[i].MainWindowTitle;
            }
            string songName = (windowName == null) ? null : windowName.Substring(10);
            string lastSong = "";
            if (songList.Count > 0)
                lastSong = songList.Last().Value;
            return (songName != lastSong) ? songName : null;
        }

        public void UpdateList()
        {
            string song = GetSongName();
            DateTime now = GetCurrentDate();
            if(song != null)
                songList.Add(now, song);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<DateTime, string> s in songList)
                sb.Append("[" + s.Key.ToString() + "]: " + s.Value + "\n");
            return sb.ToString();
        }

        private DateTime GetCurrentDate()
        {
            DateTime Now = DateTime.Now;
            return Now;
        }
    }
}
