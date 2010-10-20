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

    public partial class Script : IScriptInterface
    {
        LiteClient Client;
        myTabPage GUI;

        public void Start(ref UOLite2.LiteClient Client, myTabPage GUI)
        {
            this.Client = Client;
            this.GUI = GUI;
        }



        private void Pathfind(ushort X, ushort Y, ushort Accuracy)
        {
            GUI.UpdateLog("Finding Path to" + X.ToString() + "," + Y.ToString());
            List<Point> Mypath = FindPath(new Point(Client.Player.X,Client.Player.Y), new Point(X, Y));
            // traverse the path here
            GUI.UpdateLog("Path Found. Path steps= " + Mypath.Count);
            int i = Mypath.Count;

        }


        public List<Point> FindPath(Point start, Point end)
        {
            List<node> World = FillWorld(new Bound(32, 32), new Bound(32, 32));
            List<Point> mypath = new List<Point>();
            List<node> Openlist = new List<node>();
            List<node> Closedlist = new List<node>();
            Openlist.Add(new node(Client.Player.X,Client.Player.Y, Map.Trammel.Tiles.GetLandTile(Client.Player.X,Client.Player.Y).Z, Map.Trammel.Tiles.GetLandTile(Client.Player.X,Client.Player.Y).ID));
            
            while (Openlist.Count > 0)
            {
                
                node Current = Openlist.First<node>();
                Openlist.Remove(Current);
                Closedlist.Add(Current);

                if ((Current.X == end.X) && (Current.Y == end.Y))
                {
                    while (Current.Parent != null) {
                    mypath.Add(new Point(Current.X,Current.Y));
                        Current = (node)Current.Parent;
                    }
                    return mypath;
 
                }
                foreach( node mynode in World) {
                    if ((mynode.flags() != Ultima.TileFlag.Impassable) && (Get2DDistance(mynode.X, mynode.Y, Current.X, Current.Y) == 1) && (!Closedlist.Contains(mynode)))
                    {
                        if (Openlist.Contains(mynode))
                        {
                            
                        }
                        node tempnode = mynode;
                        tempnode.Parent = Current;
                        tempnode.G = 10 + Current.G;
                        tempnode.H = 10 * (Math.Abs(tempnode.X - end.X) + Math.Abs(tempnode.Y - end.Y));
                        tempnode.F = tempnode.G + tempnode.H;
                        //tempnode.Cost = 10 + (10 * Get2DDistance(tempnode.X,tempnode.Y,end.X,end.Y));
                        Openlist.Add(tempnode);
                    }
                }
                Openlist.Sort();

            }

            return mypath;
        }

        private int Get2DDistance(int X1, int Y1, int X2, int Y2)
        {
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
        }

        public List<node> FillWorld(Bound X, Bound Y)
        {
            List<node> mynodes = new List<node>();
            for (int x = Client.Player.X - X.Lower; x <= (Client.Player.X + X.Upper); x++)
            {
                for (int y = Client.Player.Y - Y.Lower; y <= (Client.Player.Y + Y.Upper); y++)
                {

                    Tile temptile = Ultima.Map.Trammel.Tiles.GetLandTile(x, y);                    
                    node mynode = new node(x, y, temptile.Z, temptile.ID);
                    mynodes.Add(mynode);

                }
            }
            return mynodes;
        }
        public struct node : IComparable<node>
        {
            public int X;
            public int Y;
            public int Z;
            public int ID;
            public object Parent;
            public int F;
            public int G;
            public int H;
            public node(int X, int Y, int Z, int ID)
            {
                this.X = X; this.Y = Y; this.Z = Z; this.ID = ID;
                this.F = 0;
                this.H = 0;
                this.G = 0;
                Parent = null;
            }
            public TileFlag flags()
            {
                return TileData.LandTable[ID].Flags;
            }
            public int CompareTo(node other)
            {
                return this.F.CompareTo(other.F);
            }
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
