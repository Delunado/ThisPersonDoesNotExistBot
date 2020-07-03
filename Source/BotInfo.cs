using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DoesNotExistBot
{
    class BotInfo
    {
        long actualPublication;
        public long ActualPublication { get => actualPublication; set => actualPublication = value; }

        string pathToInfo;

        public BotInfo(string pathToInfo)
        {
            this.pathToInfo = pathToInfo;
            string[] info = File.ReadAllLines(pathToInfo);
            actualPublication = long.Parse(info[0]);
        }

        public void UpdateInfo()
        {
            actualPublication++;
            string[] arrLine = File.ReadAllLines(pathToInfo);
            arrLine[0] = actualPublication.ToString();
            File.WriteAllLines(pathToInfo, arrLine);
        }
    }
}
