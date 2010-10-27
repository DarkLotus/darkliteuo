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
        Boolean Debug = false;
        LiteClient Client;
        myTabPage GUI;

        public void Start(ref UOLite2.LiteClient Client, myTabPage GUI)
        {
            this.Client = Client;
            this.GUI = GUI;
        }



        private void Pathfind(ushort X, ushort Y, ushort Accuracy)
        {
            if (Debug) { GUI.UpdateLog("Finding Path to" + X.ToString() + "," + Y.ToString()); }
            List<Point> Mypath = FindPath(new Point(Client.Player.X,Client.Player.Y), new Point(X, Y));
            // traverse the path here
            if (Debug) { GUI.UpdateLog("Path Found. Path steps= " + Mypath.Count); }
            int temp = Mypath.Count - Accuracy - 1;
            for (int i = 0; i <= temp; i++) // 
            {
                    UOLite2.Enums.Direction DirRunning = GetDirection(Client.Player.X, Client.Player.Y, Convert.ToUInt16(Mypath[i].X), Convert.ToUInt16(Mypath[i].Y));
                    if (i >= 1)
                    {
                        DirRunning = GetDirection(Convert.ToUInt16(Mypath[i - 1].X), Convert.ToUInt16(Mypath[i - 1].Y), Convert.ToUInt16(Mypath[i].X), Convert.ToUInt16(Mypath[i].Y));
                    }
                    if ((DirRunning != UOLite2.Enums.Direction.None) && ((Client.Player.X == Mypath[i].X) && (Client.Player.Y == Mypath[i].Y)))
                    {

                uint steps = 1;
                int m = (int)DirRunning ^ 0x80;
                UOLite2.Enums.Direction DirWalking = (UOLite2.Enums.Direction)m;
                if ((Client.Player.Direction != DirRunning) && (Client.Player.Direction != DirWalking) && ( i == 0)) { steps = 2; }
                if (Debug) { GUI.UpdateLog("Taking step: " + DirRunning.ToString() + " Objective: " + Mypath[i].X.ToString() + "-" + Mypath[i].Y.ToString()); }
                Client.Walk(ref DirRunning, ref steps);
                for (int t = 0; t < 50; t++)
                {
                    if((Client.Player.X == Mypath[i].X) && (Client.Player.Y == Mypath[i].Y))
                    {
                        break;
                    }
                    Thread.Sleep(10);
                }

                if ((Client.Player.X != Mypath[i].X) && (Client.Player.Y != Mypath[i].Y))
                {
                    i = -1;
                }
                    }
            }
           /* foreach (Point p in Mypath)
            {
               
                UOLite2.Enums.Direction DirRunning = GetDirection(Client.Player.X,Client.Player.Y,Convert.ToUInt16(p.X),Convert.ToUInt16(p.Y));
                uint steps = 1;
                int m = (int)DirRunning ^ 0x80;
                UOLite2.Enums.Direction DirWalking = (UOLite2.Enums.Direction)m;
                //if ((Client.Player.Direction != DirRunning) && (Client.Player.Direction != DirWalking)) { steps = 2; }
                GUI.UpdateLog("Taking step: " + DirRunning.ToString() + " Objective: " + p.X.ToString() + "-" + p.Y.ToString());
                Client.Walk(ref DirRunning, ref steps);
                Thread.Sleep(5);
                
            }*/

        }

        public UOLite2.Enums.Direction GetDirection(ushort X1, ushort Y1, ushort X2, ushort Y2)
        {
            if (X1 == X2 & Y1 == Y2)
            {
                return UOLite2.Enums.Direction.None;
            }
            else if (X1 == X2 & Y1 < Y2)
            {
                return UOLite2.Enums.Direction.SouthRunning;
            }
            else if (X1 == X2 & Y1 > Y2)
            {
                return UOLite2.Enums.Direction.NorthRunning;
            }
            else if (X1 > X2 & Y1 == Y2)
            {
                return UOLite2.Enums.Direction.WestRunning;
            }
            else if (X1 > X2 & Y1 < Y2)
            {
                return UOLite2.Enums.Direction.SouthWestRunning;
            }
            else if (X1 > X2 & Y1 > Y2)
            {
                return UOLite2.Enums.Direction.NorthWestRunning;
            }
            else if (X1 < X2 & Y1 == Y2)
            {
                return UOLite2.Enums.Direction.EastRunning;
            }
            else if (X1 < X2 & Y1 < Y2)
            {
                return UOLite2.Enums.Direction.SouthEastRunning;
                //If X1 < X2 And Y1 > Y2 Then
            }
            else
            {
                return UOLite2.Enums.Direction.NorthEastRunning;
            }
        }
        public List<Point> FindPath(Point start, Point end)
        {
            //List<node> World = FillWorld(new Bound(32, 32), new Bound(32, 32));
            List<node> Neighbours;
            List<Point> mypath = new List<Point>();
            List<node> Openlist = new List<node>();
            List<node> Closedlist = new List<node>();
            Openlist.Add(new node(Client.Player.X,Client.Player.Y, Map.Trammel.Tiles.GetLandTile(Client.Player.X,Client.Player.Y).Z, Map.Trammel.Tiles.GetLandTile(Client.Player.X,Client.Player.Y).ID,0));
            
            while (Openlist.Count > 0)
            {
                
                node Current = Openlist.First<node>();
                Openlist.Remove(Current);
                Closedlist.Add(Current);
                if ((Current.X == end.X) && (Current.Y == end.Y))
                {
                    while (Current.Parent != null) {
                    mypath.Insert(0,new Point(Current.X,Current.Y));
                        Current = (node)Current.Parent;
                    }
                    return mypath;
 
                }
                Neighbours = GetNeighbours(Current.X, Current.Y);
                foreach( node mynode in Neighbours) {
                    if ((!mynode.Blocked()) && (!Closedlist.Contains(mynode)))
                    {
                        if (Openlist.Exists(delegate(node Mynode) { return ((Mynode.X == mynode.X) && (Mynode.Y == mynode.Y)); } ))
                        {
                            node nn = Openlist.Find(delegate(node Mynode) { return ((Mynode.X == mynode.X) && (Mynode.Y == mynode.Y)); });
                            if (nn.G > mynode.G + Current.G)
                            {
                                Openlist.Remove(nn);
                                nn.Parent = Current;
                                nn.G = mynode.G + Current.G;
                                nn.H = 10 * (Math.Abs(nn.X - end.X) + Math.Abs(nn.Y - end.Y));
                                nn.F = nn.G + nn.H;
                                Openlist.Add(nn);
                            }

                        }
                        else
                        {
                            node tempnode = mynode;
                            tempnode.Parent = Current;
                            tempnode.G = tempnode.G + Current.G;
                            tempnode.H = 10 * (Math.Abs(tempnode.X - end.X) + Math.Abs(tempnode.Y - end.Y));
                            tempnode.F = tempnode.G + tempnode.H;
                            //tempnode.Cost = 10 + (10 * Get2DDistance(tempnode.X,tempnode.Y,end.X,end.Y));
                            Openlist.Add(tempnode);
                        }

                        }
                    else { Closedlist.Add(mynode); } // Maybe this works? we only need x/y in closed list.
                }
                Openlist.Sort();
               //if (Openlist.Count > 1000) { Openlist.RemoveRange(1000, Openlist.Count - 1001); }

            }
            // will return empty path
            return mypath;
        }


        private List<node> GetNeighbours(int x, int y)
        {
            List<node> mynodes = new List<node>(8);
            int diagcost = 14;
            int normcost = 10;
            mynodes.Add(new node(x + 1, y, Ultima.Map.Trammel.Tiles.GetLandTile(x + 1, y).Z, Ultima.Map.Trammel.Tiles.GetLandTile(x + 1, y).ID,normcost)); // east
            mynodes.Add(new node(x - 1, y, Ultima.Map.Trammel.Tiles.GetLandTile(x - 1, y).Z, Ultima.Map.Trammel.Tiles.GetLandTile(x - 1, y).ID, normcost)); // west

            mynodes.Add(new node(x, y + 1, Ultima.Map.Trammel.Tiles.GetLandTile(x, y + 1).Z, Ultima.Map.Trammel.Tiles.GetLandTile(x, y + 1).ID, normcost)); //south
            mynodes.Add(new node(x, y - 1, Ultima.Map.Trammel.Tiles.GetLandTile(x, y - 1).Z, Ultima.Map.Trammel.Tiles.GetLandTile(x, y - 1).ID, normcost)); //north
            if ((!mynodes[0].Blocked()) && (!mynodes[3].Blocked()))
            {
                mynodes.Add(new node(x + 1, y - 1, Ultima.Map.Trammel.Tiles.GetLandTile(x + 1, y - 1).Z, Ultima.Map.Trammel.Tiles.GetLandTile(x + 1, y - 1).ID, diagcost)); // NE
            }
            else
            {
                mynodes.Add(new node(x + 1, y - 1, Ultima.Map.Trammel.Tiles.GetLandTile(x + 1, y - 1).Z, Ultima.Map.Trammel.Tiles.GetLandTile(x + 1, y - 1).ID, 1000)); // NE
            }

            if ((!mynodes[1].Blocked()) && (!mynodes[2].Blocked()))
            {
                mynodes.Add(new node(x - 1, y + 1, Ultima.Map.Trammel.Tiles.GetLandTile(x - 1, y + 1).Z, Ultima.Map.Trammel.Tiles.GetLandTile(x - 1, y + 1).ID, diagcost)); // SW
            }
            else
            {
                mynodes.Add(new node(x - 1, y + 1, Ultima.Map.Trammel.Tiles.GetLandTile(x - 1, y + 1).Z, Ultima.Map.Trammel.Tiles.GetLandTile(x - 1, y + 1).ID, 1000)); // SW
            }

            if ((!mynodes[0].Blocked()) && (!mynodes[2].Blocked()))
            {
                mynodes.Add(new node(x + 1, y + 1, Ultima.Map.Trammel.Tiles.GetLandTile(x + 1, y + 1).Z, Ultima.Map.Trammel.Tiles.GetLandTile(x + 1, y + 1).ID, diagcost)); //SE
            }
            else
            {
                mynodes.Add(new node(x + 1, y + 1, Ultima.Map.Trammel.Tiles.GetLandTile(x + 1, y + 1).Z, Ultima.Map.Trammel.Tiles.GetLandTile(x + 1, y + 1).ID, 1000)); //SE
            }

            if ((!mynodes[1].Blocked()) && (!mynodes[3].Blocked()))
            {
                mynodes.Add(new node(x - 1, y - 1, Ultima.Map.Trammel.Tiles.GetLandTile(x - 1, y - 1).Z, Ultima.Map.Trammel.Tiles.GetLandTile(x - 1, y - 1).ID, diagcost));//NW
            }
            else
            {
                mynodes.Add(new node(x - 1, y - 1, Ultima.Map.Trammel.Tiles.GetLandTile(x - 1, y - 1).Z, Ultima.Map.Trammel.Tiles.GetLandTile(x - 1, y - 1).ID, 1000));//NW
            }
            return mynodes;
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
                    node mynode = new node(x, y, temptile.Z, temptile.ID,10);
                    mynodes.Add(mynode);

                }
            }
            return mynodes;
        }
        public struct node : IComparable
        {
            public int X;
            public int Y;
            public int Z;
            public int ID;
            public object Parent;
            public int F;
            public int G;
            public int H;
            public node(int X, int Y, int Z, int ID, int G)
            {
                this.X = X; this.Y = Y; this.Z = Z; this.ID = ID;
                this.F = 0;
                this.H = 0;
                this.G = G;
                Parent = null;
            }
            public static nodeComparer GetComparer()
            {
                return new node.nodeComparer();
            }
            public TileFlag flags()
            {
                return TileData.LandTable[ID].Flags;
            }
            public bool Blocked()
            {
                HuedTile[] temptiles = Ultima.Map.Trammel.Tiles.GetStaticTiles(X, Y);
                HuedTile[][][] mm = Ultima.Map.Trammel.Tiles.GetStaticBlock(X, Y);

                foreach (HuedTile p in temptiles)
                {
                     if (TileData.ItemTable[p.ID].Impassable) { return true; }
                  
                 }
                if (TileData.LandTable[ID].Flags == TileFlag.Impassable) { return true; }
                return false;
            }
            public int CompareTo(object other)
            {
                node newnode = (node)other;
                return this.F.CompareTo(newnode.F);
            }
            public int CompareTo(node other, node.nodeComparer.ComparisonType which)
            {
                switch (which)
                {
                    case node.nodeComparer.ComparisonType.Coords:
                        return this.X.CompareTo(other.X);
                    case node.nodeComparer.ComparisonType.F:
                        return this.F.CompareTo(other.F);
                }
                return 0;
            }
            public class nodeComparer : IComparer<node>
            {
                // enumeration of comparsion types
                public enum ComparisonType
                {
                    Coords,
                    F
                };
                // Tell the Employee objects to compare themselves
                public int Compare(node lhs, node rhs)
                {
                    node l = (node) lhs;
                     node r = (node) rhs;
                    return l.CompareTo(r,WhichComparison);
                }
                public node.nodeComparer.ComparisonType WhichComparison
                {
                    get    {    return whichComparison;    }
                    set { whichComparison=value; }
                }

                private node.nodeComparer.ComparisonType whichComparison;
                }
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
