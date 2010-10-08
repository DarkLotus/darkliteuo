using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UOLite2;
using System.Windows.Forms;
using Ultima;
namespace DarkLiteUO
{
    public partial class Script : IScriptInterface
    {
        LiteClient Client;
        myTabPage GUI;
        public void Start(ref UOLite2.LiteClient Client, myTabPage GUI)
        {
            this.Client = Client;
            this.GUI = GUI;
        }

        public Ultima.Tile GetLandTile(int X, int Y)
        {
            Tile mytile = new Tile();
            TileMatrix tm = new TileMatrix(0, 0, 6144, 4096);
            mytile = tm.GetLandTile(X,Y);
            return mytile;
        }

        public HuedTile[] GetStatics(int X, int Y)
        {
            TileMatrix tm = new TileMatrix(0,0,6144,4096);
            HuedTile[] mytiles = tm.GetStaticTiles(X, Y);
            return mytiles;
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
               
    

    public class finditem
    {
        // Create a finditem object, then find by type/id returns all details. No current filtering of any kind

        Item _Itemobject;
        uint _FindID;
        Boolean _bfind;
        ushort _FindType;
        UOLite2.LiteClient Client;
        public uint FindID { get { return _FindID; } }
        public ushort FindType { get { return _FindType; } }
        public Item Item { get { return _Itemobject; } }
        public Boolean ItemFound { get { return _bfind; } }
        public finditem(ref UOLite2.LiteClient client)
        {
            Client = client;
        }
        public void ByType(ushort type)
        {
            foreach (UOLite2.Item i in Client.Items.Items)
            {
                if (i.Type == type) { return; }
                _Itemobject = i;
                _FindID = i.Serial.Value;
                _bfind = true;
                _FindType = i.Type;

            }
            _FindID = 0;
            _FindType = 0;
            _bfind = false;
            
            // it no item is found this will crash :o what fun
        }

        // Think we need to do this to make sure items still there will use less cycles than by type?
        public Boolean ItemExists()
        {
            foreach (UOLite2.Item i in Client.Items.Items)
            {
                if (i.Serial == _FindID) { return true; }
            }
             return false;
        }

    }
}
