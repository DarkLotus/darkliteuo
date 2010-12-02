using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UOLite2;
using Ultima;
using System.Drawing;
namespace DarkLiteUO
{

    public partial class _ScriptTools
    {
        public void Pathfind(ushort X, ushort Y, ushort Accuracy)
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            while (Get2DDistance(Client.Player.X, Client.Player.Y, X, Y) > Accuracy)
            {
                GUI.UpdateLog("Distance to target: " + Get2DDistance(Client.Player.X, Client.Player.Y, X, Y).ToString());
                List<Point> Mypath = FindPath(new Point(Client.Player.X, Client.Player.Y), new Point(X, Y), Accuracy);
                UOLite2.Enums.Direction DirRunning;
                DirRunning = GetDirection(Client.Player.X, Client.Player.Y, Convert.ToUInt16(Mypath[0].X), Convert.ToUInt16(Mypath[0].Y));
                uint steps = 1;
                Client.Walk(ref DirRunning, ref steps);
                timer.Restart();
                while (timer.ElapsedMilliseconds < 300)
                {
                    Thread.Sleep(5);
                }
            }
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

        public Map Getmap(LiteClient Client)
        {
            Map mymap;
            switch (Client.Player.Facet)
            {
                case UOLite2.Enums.Facets.Felucca:
                    mymap = Map.Felucca;
                    break;
                case UOLite2.Enums.Facets.Trammel:
                    mymap = Map.Trammel;
                    break;
                case UOLite2.Enums.Facets.Ilshenar:
                    mymap = Map.Ilshenar;
                    break;
                case UOLite2.Enums.Facets.Malas:
                    mymap = Map.Malas;
                    break;
                case UOLite2.Enums.Facets.Tokuno:
                    mymap = Map.Tokuno;
                    break;
                case UOLite2.Enums.Facets.Termur:
                    mymap = Map.TerMur;
                    break;

                default:
                    mymap = Map.Felucca;
                    break;
            }
            return mymap;
        }
        private List<Point> FindPath(Point start, Point end, ushort accuracy)
        {
            //List<node> World = FillWorld(new Bound(32, 32), new Bound(32, 32));
            Map mymap = Getmap(Client);

            List<node> Neighbours;
            List<Point> mypath = new List<Point>();
            List<node> Openlist = new List<node>();
            List<node> Closedlist = new List<node>();
            //Add our location to the open list
            Openlist.Add(new node(Client.Player.X, Client.Player.Y, mymap.Tiles.GetLandTile(Client.Player.X, Client.Player.Y).Z, mymap.Tiles.GetLandTile(Client.Player.X, Client.Player.Y).ID, 0));
            //Openlist.Add(new node(Client.Player.X, Client.Player.Y, mymap.Tiles.GetLandTile(Client.Player.X, Client.Player.Y).Z, mymap.Tiles.GetLandTile(Client.Player.X, Client.Player.Y).ID, 0));

            while (Openlist.Count > 0)
            {

                node Current = Openlist.First<node>();
                Openlist.Remove(Current);
                Closedlist.Add(Current);

                //if ((Current.X == end.X) && (Current.Y == end.Y))
                // returns the path once we are close enough to target
                if (Get2DDistance(Current.X, Current.Y, end.X, end.Y) <= accuracy)
                {
                    while (Current.Parent != null)
                    {
                        mypath.Insert(0, new Point(Current.X, Current.Y));
                        Current = (node)Current.Parent;
                    }
                    return mypath;

                }
                // gets the 4-8 walkable tiles around the current node
                Neighbours = GetNeighbours(Current.X, Current.Y, ref mymap);
                foreach (node mynode in Neighbours)
                {

                    // Hack for pathfinding to a blocked location like a tree. Will ignore the last tile being blocked if the accuracy is > 1
                    if (accuracy >= 1 && ((mynode.X == end.X) && (mynode.Y == end.Y)))
                    {
                        node tempnode = mynode;
                        tempnode.Parent = Current;
                        tempnode.G = tempnode.G + Current.G;
                        tempnode.H = 10 * (Math.Abs(tempnode.X - end.X) + Math.Abs(tempnode.Y - end.Y));
                        tempnode.F = tempnode.G + tempnode.H;
                        //tempnode.Cost = 10 + (10 * Get2DDistance(tempnode.X,tempnode.Y,end.X,end.Y));
                        Openlist.Add(tempnode);
                    }
                    // if the node isnt blocked
                    if ((!mynode.Blocked(ref Client)) && (!Closedlist.Contains(mynode)))
                    {
                        // If the node is already in the openlist, check to see if our current path to the tile is better than the old one
                        if (Openlist.Exists(delegate(node Mynode) { return ((Mynode.X == mynode.X) && (Mynode.Y == mynode.Y)); }))
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

                        }// just add it like normal
                        else
                        {
                            node tempnode = mynode;
                            tempnode.Parent = Current;
                            tempnode.G = tempnode.G + Current.G;
                            tempnode.H = 10 * (Math.Abs(tempnode.X - end.X) + Math.Abs(tempnode.Y - end.Y)); // Manhattan distance
                            tempnode.F = tempnode.G + tempnode.H;
                            //tempnode.Cost = 10 + (10 * Get2DDistance(tempnode.X,tempnode.Y,end.X,end.Y));
                            Openlist.Add(tempnode);
                        }

                    }
                    else { Closedlist.Add(mynode); }
                }
                // sort the open list so the lowest path cost is at the top.
                // very very slow on large open lists, for long paths we need to split the route up, or preferably cull nodes that have very bad scores, ie tiles in wrong direction.
                Openlist.Sort();
                //if (Openlist.Count > 1000) { Openlist.RemoveRange(1000, Openlist.Count - 1001); }

            }
            // will return empty path
            return mypath;
        }


        private List<node> GetNeighbours(int x, int y, ref Map mymap)
        {
            List<node> mynodes = new List<node>(8);
            int diagcost = 14;
            int normcost = 10;
            mynodes.Add(new node(x + 1, y, mymap.Tiles.GetLandTile(x + 1, y).Z, mymap.Tiles.GetLandTile(x + 1, y).ID, normcost)); // east
            mynodes.Add(new node(x - 1, y, mymap.Tiles.GetLandTile(x - 1, y).Z, mymap.Tiles.GetLandTile(x - 1, y).ID, normcost)); // west

            mynodes.Add(new node(x, y + 1, mymap.Tiles.GetLandTile(x, y + 1).Z, mymap.Tiles.GetLandTile(x, y + 1).ID, normcost)); //south
            mynodes.Add(new node(x, y - 1, mymap.Tiles.GetLandTile(x, y - 1).Z, mymap.Tiles.GetLandTile(x, y - 1).ID, normcost)); //north

            if ((!mynodes[0].Blocked(ref Client)) && (!mynodes[3].Blocked(ref Client)))
            {
                mynodes.Add(new node(x + 1, y - 1, mymap.Tiles.GetLandTile(x + 1, y - 1).Z, mymap.Tiles.GetLandTile(x + 1, y - 1).ID, diagcost)); // NE
            }
            else
            {
                //mynodes.Add(new node(x + 1, y - 1, Ultima.Map.Trammel.Tiles.GetLandTile(x + 1, y - 1).Z, Ultima.Map.Trammel.Tiles.GetLandTile(x + 1, y - 1).ID, 1000)); // NE
            }

            if ((!mynodes[1].Blocked(ref Client)) && (!mynodes[2].Blocked(ref Client)))
            {
                mynodes.Add(new node(x - 1, y + 1, mymap.Tiles.GetLandTile(x - 1, y + 1).Z, mymap.Tiles.GetLandTile(x - 1, y + 1).ID, diagcost)); // SW
            }
            else
            {
                // mynodes.Add(new node(x - 1, y + 1, Ultima.Map.Trammel.Tiles.GetLandTile(x - 1, y + 1).Z, Ultima.Map.Trammel.Tiles.GetLandTile(x - 1, y + 1).ID, 1000)); // SW
            }

            if ((!mynodes[0].Blocked(ref Client)) && (!mynodes[2].Blocked(ref Client)))
            {
                mynodes.Add(new node(x + 1, y + 1, mymap.Tiles.GetLandTile(x + 1, y + 1).Z, mymap.Tiles.GetLandTile(x + 1, y + 1).ID, diagcost)); //SE
            }
            else
            {
                // mynodes.Add(new node(x + 1, y + 1, Ultima.Map.Trammel.Tiles.GetLandTile(x + 1, y + 1).Z, Ultima.Map.Trammel.Tiles.GetLandTile(x + 1, y + 1).ID, 1000)); //SE
            }

            if ((!mynodes[1].Blocked(ref Client)) && (!mynodes[3].Blocked(ref Client)))
            {
                mynodes.Add(new node(x - 1, y - 1, mymap.Tiles.GetLandTile(x - 1, y - 1).Z, mymap.Tiles.GetLandTile(x - 1, y - 1).ID, diagcost));//NW
            }
            else
            {
                // mynodes.Add(new node(x - 1, y - 1, Ultima.Map.Trammel.Tiles.GetLandTile(x - 1, y - 1).Z, Ultima.Map.Trammel.Tiles.GetLandTile(x - 1, y - 1).ID, 1000));//NW
            }
            return mynodes;
        }




        private struct node : IComparable
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
            public bool Blocked(ref UOLite2.LiteClient Client)
            {
                // Need to add checking for multis
                HuedTile[] temptiles = Getmap(ref Client).Tiles.GetStaticTiles(X, Y);
                //HuedTile[][][] mm = Ultima.Map.Trammel.Tiles.GetStaticBlock(X, Y);

                foreach (HuedTile p in temptiles)
                {
                    if (TileData.ItemTable[p.ID].Impassable) { return true; }

                }
                if (TileData.LandTable[ID].Flags == TileFlag.Impassable) { return true; }
                foreach (Item i in Client.Items)
                {
                    if ((i.X == X) && (i.Y == Y))
                    {
                        if (Ultima.TileData.ItemTable[i.Type].Impassable) { return true; }
                    }
                }

                return false;
            }
            public Map Getmap(ref LiteClient Client)
            {
                // returns a Map object for the players current facet
                Map mymap;
                switch (Client.Player.Facet)
                {
                    case UOLite2.Enums.Facets.Felucca:
                        mymap = Map.Felucca;
                        break;
                    case UOLite2.Enums.Facets.Trammel:
                        mymap = Map.Trammel;
                        break;
                    case UOLite2.Enums.Facets.Ilshenar:
                        mymap = Map.Ilshenar;
                        break;
                    case UOLite2.Enums.Facets.Malas:
                        mymap = Map.Malas;
                        break;
                    case UOLite2.Enums.Facets.Tokuno:
                        mymap = Map.Tokuno;
                        break;
                    case UOLite2.Enums.Facets.Termur:
                        mymap = Map.TerMur;
                        break;

                    default:
                        mymap = Map.Felucca;
                        break;
                }
                return mymap;
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
                    node l = (node)lhs;
                    node r = (node)rhs;
                    return l.CompareTo(r, WhichComparison);
                }
                public node.nodeComparer.ComparisonType WhichComparison
                {
                    get { return whichComparison; }
                    set { whichComparison = value; }
                }

                private node.nodeComparer.ComparisonType whichComparison;
            }
        }
        public int Get2DDistance(int X1, int Y1, int X2, int Y2)
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
      
    }
}
