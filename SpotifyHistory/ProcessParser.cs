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
        private List<string> songList;
        public List<string> SongList
        {
            get { return songList; }
        }

        public ProcessParser()
        {
            songList = new List<string>();
        }

        public void ReadFile(string path)
        {
            if (!File.Exists(path))
                throw new IOException("File not found: " + path);

            string[] songs = File.ReadAllLines(path);
            foreach (string s in songs)
            {
                songList.Add(s);
            }
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
                lastSong = songList.Last();
            return (songName != lastSong) ? songName : null;
        }

        public void UpdateList()
        {
            string song = GetSongName();
            string now = GetCurrentDate();
            if(song != null)
                songList.Add("[" + now + "]: " + song);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in songList)
                sb.Append(s + "\n");
            
            return sb.ToString();
        }

        private string GetCurrentDate()
        {
            DateTime Now = DateTime.Now;
            return Now.ToShortDateString() + ", " + Now.ToLongTimeString();
        }
    }
}
