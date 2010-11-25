using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UOLite2;
using System.Windows.Forms;
using Ultima;
using System.Drawing;
namespace DarkLiteUO
{

    public class ScriptTools
    {
        public LiteClient Client;
        public myTabPage GUI;
        public _ScriptTools Tools;

        public static struct Coordinate
        {
            public ushort X;
            public ushort Y;
            public ushort Z;
            public void Add(ushort X, ushort Y, ushort Z)
            {
                this.X = X;
                this.Y = Y;
                this.Z = Z;
            }
        }
        public ScriptTools(ref LiteClient _client, myTabPage _gui)
        {
            this.Client = _client;
            this.GUI = _gui;
            this.Tools = new _ScriptTools(ref Client, GUI);
        }

        public void Pathfind(ushort p, ushort p_2, ushort p_3)
        {
            Tools.Pathfind(p, p_2, p_3);
        }
    }
      public partial class _ScriptTools
    {
        //Boolean Debug = false;
        LiteClient Client;
        myTabPage GUI;
 

        public _ScriptTools(ref LiteClient Client, myTabPage GUI)
        {
            this.Client = Client;
            this.GUI = GUI;
            
        }




        public void Speak(String text)
        {
            UOLite2.Enums.Common.Hues myhue = UOLite2.Enums.Common.Hues.Blue;
            UOLite2.Enums.SpeechTypes myspeech = UOLite2.Enums.SpeechTypes.Regular;
            UOLite2.Enums.Fonts myfont = UOLite2.Enums.Fonts.Default;
            string mymsg = text;
            Client.Speak(ref mymsg, ref myhue, ref myspeech, ref myfont);
        }

        public uint EUOToInt(String val)
        //Code by BtbN
        {
            val = val.ToUpper(); // Important!

            uint num = 0;

            for (int p = val.Length - 1; p >= 0; p--)
                num = num * 26 + (((uint)val[p]) - 65);

            num = (num - 7) ^ 0x45;

            return num;
        }
        public ushort EUOToUshort(String val)
        //Code by BtbN
        {
            val = val.ToUpper(); // Important!

            uint num = 0;

            for (int p = val.Length - 1; p >= 0; p--)
                num = num * 26 + (((uint)val[p]) - 65);

            num = (num - 7) ^ 0x45;

            return (ushort)num;
        }
        public String IntToEUO(int num)
        //Code by BtbN
        {
            num = (num ^ 0x45) + 7;

            String res = "";

            do
            {
                res += (Char)(65 + (num % 26));
                num /= 26;
            } while (num >= 1);

            return res;
        }
        public int Get2DDistance(ushort X1, ushort Y1, ushort X2, ushort Y2)
        {
            //Taken from UOLite2
            //Whichever is greater is the distance.
            int xdif = Convert.ToInt32(X1) - Convert.ToInt32(X2);
            int ydif = Convert.ToInt32(Y1) - Convert.ToInt32(Y2);

            if (xdif < 0)
                xdif *= -1;
            if (ydif < 0)
                ydif *= -1;

            //Return the largest difference.
            if (ydif > xdif)
            {
                return ydif;
            }
            else
            {
                return xdif;
            }
        }
        public void GumpMenuSelection(uint ID, uint GumpID, uint ButtonID)
        {
            // static size atm buttons only
            byte[] packet = new byte[23];
            packet[0] = 0xb1;
            packet[1] = 0x00;
            packet[2] = 0x17;
            
            AddtoArray(3, ref packet, uintToByteArray(ID));
            AddtoArray(7, ref packet, uintToByteArray(GumpID));
            AddtoArray(11, ref packet, uintToByteArray(ButtonID));
            AddtoArray(15, ref packet, uintToByteArray(0));
            AddtoArray(19, ref packet, uintToByteArray(0));
            GUI.UpdateLog("Sending Gump reply" + GetString(packet));
            Client.Send(ref packet);
        }
        public static byte[] GetBytes(string text)
        {
            return ASCIIEncoding.UTF8.GetBytes(text);
        }
        public static String GetString(byte[] text)
        {
            return ASCIIEncoding.UTF8.GetString(text);
        }
        private static void AddtoArray(int index, ref byte[] Array, byte[] DatatoAdd)
        {
            DatatoAdd.CopyTo(Array, index);
            //DatatoAdd.CopyTo(Array, Array.Length + 1);
            //int startindex = Array.Length + 1;
            //foreach (byte i in DatatoAdd)
            //{
            //    Array[startindex + i] = DatatoAdd[i];
            //}
            //return;
        }
        private static byte[] uintToByteArray(uint value)
        {
            return new byte[] {
                (byte)(value >> 24),
                (byte)(value >> 16),
                (byte)(value >> 8),
                (byte)value};

        }


    }
               
    

   
}
