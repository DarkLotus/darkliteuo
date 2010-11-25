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

        public ScriptTools.Coordinate[] ImportRail(String filename)
        {
            string[] text = File.ReadAllLines(filename);
            int spotcount = 0;
            foreach(string s in text)
            {
                if(s.ToUpper().Contains("SET %X"))
                { spotcount +=1;}
            }
            ScriptTools.Coordinate[] rail = new ScriptTools.Coordinate[spotcount];
 
            for(int i = 0; i <= text.Length - 1;i++)
            {
                string line = text[i];
                if(line.ToUpper().Contains("SET %X"))
                {
                    if (text[i + 2].ToUpper().Contains("SET %Z"))
                    {
                        rail[i].Add(Convert.ToUInt16(line.Substring(6)), Convert.ToUInt16(text[i + 1].Substring(6)), Convert.ToUInt16(text[i + 2].Substring(6)));
                    }
                    else
                    {
                        rail[i].Add(Convert.ToUInt16(line.Substring(6)), Convert.ToUInt16(text[i + 1].Substring(6)),(ushort)Getmap(Client).Tiles.GetLandTile(Convert.ToInt32(line.Substring(6)),Convert.ToInt32(text[i+1].Substring(6))).Z);
                    }
                }
            }      
            return rail;
        }
    
    }
}
