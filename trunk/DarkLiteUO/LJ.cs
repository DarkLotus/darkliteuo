#define debug
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using UOLite2;
using System.Windows.Forms;
using UOLite2.SupportClasses;
using Ultima;
using System.Drawing;

namespace DarkLiteUO
{
    public partial class Script : IScriptInterface
    {
        List<string> Journal = new List<string>();
        private bool myScriptRunning = true;
        private bool WaitForCliloc = false;
        ToFromCoords Range = new ToFromCoords();
        uint[] TreeTiles;
        Point Startloc = new Point();
        ushort AxeType;
        uint DropChest;
        private struct Tree
        {
            public ushort X;
            public ushort Y;
            public ushort Z;
            public int ID;
            public Tree(int X, int Y, int Z, int ID)
            {
                this.X = Convert.ToUInt16(X);
                this.Y = Convert.ToUInt16(Y);
                this.Z = Convert.ToUInt16(Z);
                this.ID = ID;
            }
        }
        
        private struct ToFromCoords
        {
            public Bound XRange;
            public Bound YRange;

        }
        public void Main()
        {
            Debug = true;
            GUI.UpdateLog("Script Started");
            Startloc.X = 2632;
            Startloc.Y = 907;
            AxeType = (ushort)EUOToInt("BSF");
            TreeTiles = new uint[] { 3230, 3274, 3275, 3276, 3277, 3280, 3283, 3286, 3288, 3290, 3293, 3296, 3299, 3302 };
            DropChest = EUOToInt("XXGKKMD");
            
            Range.XRange = new Bound(-20,37);
            Range.YRange = new Bound(-10,30);
            Pathfind(2649,912, 0);

            return;
            ChopLoop(); // Chops all the trees withing Range

            GUI.UpdateLog("Script Ended");
            return;

            Tile mytile = new Tile();
            //TileMatrix tm = new TileMatrix(0, 0, 6144, 4096);
          // HuedTile[][][] mylist = tm.GetStaticBlock(Client.Player.X, Client.Player.Y);

           // mytile = tm.GetLandTile(X, Y);



          // HuedTile[] mtlist = GetStatics(Client.Player.X,Client.Player.Y);
            //Tile mytile = GetLandTile(Client.Player.X,Client.Player.Y);
            HashSet<Item> Tools;
            Item Tool;


        }


        public void ChopLoop()
        {
            List<Tree> Treelist = grabTile(TreeTiles, Range); // Load up all the trees into a list

            foreach (Tree i in Treelist)
            {
                Tree _Tree = i;
                if (Get2DDistance(Client.Player.X, Client.Player.Y, _Tree.X, _Tree.Y) >= 2)
                {
                    Pathfind(_Tree.X, _Tree.Y, 1); // x/y/Tiles away from location to stop
                }
                Chop(_Tree);
                if (Client.Player.Weight > Client.Player.MaxWeight - 50) { Bank(); }
            }
           
        }



        private void Bank()
        {
            throw new NotImplementedException();
        }

        private void Chop(Tree _Tree)
        {
            Client.onCliLocSpeech += new LiteClient.onCliLocSpeechEventHandler(Client_onCliLocSpeech);
            Client.onSpeech += new LiteClient.onSpeechEventHandler(Client_onSpeech);
            while(true)
            {
                Client.Items.byType(ref AxeType).FirstOrDefault<Item>().DoubleClick();
                Thread.Sleep(1500); // Sleep to wait for target for now.
                //Thread mythread = new Thread(WaitForTarget);
               // mythread.Join(5000);
                Client.Target(_Tree.X, _Tree.Y, _Tree.Z, (ushort)_Tree.ID);
                for (int i = 0; i <= 10; i++)
                {
                    if (Journal[0].Contains("not enough wood")) { break; }

                }
                

                
            }

            Client.onCliLocSpeech -= Client_onCliLocSpeech;
            Client.onSpeech -= Client_onSpeech;
        }
        void WaitForTarget()
        {
            while (!Client.Targeting)
            {
                Thread.Sleep(50);
            }

        }
        void Client_onSpeech(ref LiteClient Client, Serial Serial, ushort BodyType, UOLite2.Enums.SpeechTypes SpeechType, ushort Hue, UOLite2.Enums.Fonts Font, string Text, string Name)
        {
            Journal.Insert(0, Text);
            // todo handle additional text stuff
        }

        void Client_onCliLocSpeech(ref LiteClient Client, Serial Serial, ushort BodyType, UOLite2.Enums.SpeechTypes SpeechType, ushort Hue, UOLite2.Enums.Fonts Font, uint CliLocNumber, string Name, string ArgsString)
        {
            Journal.Insert(0, Client.CliLocStrings.get_Entry(CliLocNumber));
            //UpdateLog("Clioc: " + Name + " : " + Client.CliLocStrings.get_Entry(CliLocNumber));
        }
        void Client_onTargetRequest(ref LiteClient Client)
        {
           
            //throw new NotImplementedException();
        }
        private List<Tree> grabTile(uint[] TreeTiles, ToFromCoords range)
        {
            // takes a list of tree types and a range around char to search for them returns a list of trees
            List<Tree> _TreeList = new List<Tree>();
            for(int x = range.XRange.Lower;x <= range.XRange.Upper;x++)
            {
                for (int y = range.YRange.Lower; y <= range.YRange.Upper; y++)
                {
                    Tile temptile = Ultima.Map.Trammel.Tiles.GetLandTile(x + Startloc.X, y + Startloc.Y);
                    if (TreeTiles.Contains<uint>((uint)temptile.ID))
                    {
                        Tree mytree = new Tree(x + Startloc.X, y + Startloc.Y, temptile.Z, temptile.ID);
                        _TreeList.Add(mytree);                        
                    }
                }
            }         
            return _TreeList;
           
        }

        public struct Bound
        {
            public int Lower;
            public int Upper;
            public Bound(int low, int high)
            {
                Lower = low;
                Upper = high;
            }
        }
        public void Stop()
        {
            myScriptRunning = false;
        }


       
    }
}
