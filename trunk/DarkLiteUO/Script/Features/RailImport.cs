using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using UOLite2;
using System.Windows.Forms;
using Ultima;
using System.Drawing;
namespace DarkLiteUO
{

    public partial class _ScriptTools
    {
        public List<Point> ImportRail(String filename)
        {
            List<Point> rail = new List<Point>();
            string[] text = File.ReadAllLines(filename);
            for(int i = 0; i <= text.Length - 1;i++)
            {
                string line = text[i];
                if(line.ToUpper().Contains("SET %X"))
                {
                    rail.Add(new Point(Convert.ToInt32(line.Substring(6)) ,Convert.ToInt32(text[i+1].Substring(6))));
                }
            }
            return rail;
        }
    
    }
}
