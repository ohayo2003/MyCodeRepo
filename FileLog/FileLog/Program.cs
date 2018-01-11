using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileLog
{
    class Program
    {
        /// <summary>
        /// hehhe
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            int theCount = 10000;

            for (int i = 0; i < theCount; i++)
            {
                new Thread((o) =>
                {
                    int theI = (int)o;
                    string file = System.AppDomain.CurrentDomain.BaseDirectory + (theI % 20).ToString() + ".txt";
                    for (int j = 0; j < 1000000; j++)
                    {
                        FileLogWriter.Instance.WriteLogFile(file, theI.ToString() + ":" + j.ToString());
                    }
                    FileLogWriter.Instance.WriteLogFile("d:\\a.txt", theI.ToString() + "结束!");
                }).Start(i);
            }
        }
    }
}
