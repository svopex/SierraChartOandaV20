using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkonkwoOandaV20
{
    public class Log
    {
        private static object lockObject = new object();

        public static void WriteException(Exception ex)
        {
            string filePath = @"C:\\Log\\SierraChart.txt";
            lock (lockObject)
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine("Message :" + ex.Message + "\r\n" + Environment.NewLine + "StackTrace :" + ex.StackTrace + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                    writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                }
            }
        }

        public static void WriteMessage(string message)
        {
            string filePath = @"C:\\Log\\SierraChart.txt";
            lock (lockObject)
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine("Message :" + message);
                    writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                }
            }
        }
    }
}
