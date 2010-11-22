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
    public class Script2 : IScriptInterface
    {
        bool Debug;
        List<string> Journal = new List<string>();
        private ScriptTools Tools;
        private bool WaitForCliloc = false;
        ToFromCoords Range = new ToFromCoords();
        uint[] TreeTiles;
        Point Startloc = new Point();
        ushort[] AxeType;
        Serial DropChest;
        ushort[] ItemstoBank;
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
        public void Start(ref ScriptTools ST)
        {
            this.Tools = ST;
        }
        public void Stop()
        {

        }
        public void Main()
        {

            Debug = true;
            Tools.GUI.UpdateLog("Script Started");
            Tools.Client.Player.DoubleClick(); // Open paperdoll
            Startloc.X = 2632;
            Startloc.Y = 907;
            AxeType = new ushort[2] { EUOToUshort("BSF"), 3907 };
            TreeTiles = new uint[] { 3230, 3274, 3275, 3276, 3277, 3280, 3283, 3286, 3288, 3290, 3293, 3296, 3299, 3302 };
            DropChest = Tools.Tools.EUOToInt("XXGKKMD");
            ItemstoBank = new ushort[] { EUOToUshort("ZLK"), EUOToUshort("TLK"), EUOToUshort("YWS"), EUOToUshort("NWS"), EUOToUshort("BWR"), EUOToUshort("XWS"), EUOToUshort("FXS") };

            Range.XRange = new Bound(-10, 10);
            Range.YRange = new Bound(-10, 10);
            try
            {
                while (myScriptRunning)
                {
                    ChopLoop(); // Chops all the trees withing Range
                }
            }
            catch { }
            Tools.GUI.UpdateLog("Script Ended");
        }

        private ushort EUOToUshort(string p)
        {
            return Tools.Tools.EUOToUshort(p);
        }


        public void ChopLoop()
        {
            List<Tree> Treelist = grabTile(TreeTiles, Range); // Load up all the trees into a list

            foreach (Tree i in Treelist)
            {
                Tree _Tree = i;
                if (Tools.Tools.Get2DDistance(Tools.Client.Player.X, Tools.Client.Player.Y, _Tree.X, _Tree.Y) >= 2)
                {
                    Tools.Pathfind(_Tree.X, _Tree.Y, 2); // x/y/Tiles away from location to stop
                }
                Chop(_Tree);
                Tools.Client.onCliLocSpeech -= Client_onCliLocSpeech;
                Tools.Client.onSpeech -= Client_onSpeech;
                if (Tools.Client.Player.Weight > Tools.Client.Player.MaxWeight - 50) { Bank(); }
                if (!myScriptRunning) { return; }
            }

        }
        private void Bank()
        {
            Tools.GUI.UpdateLog("Banking");
            while (Tools.Tools.Get2DDistance(Tools.Client.Player.X, Tools.Client.Player.Y, Startloc.X, Startloc.Y) >= 2)
            {
                Tools.Pathfind((ushort)Startloc.X, (ushort)Startloc.Y, 0); // x/y/Tiles away from location to stop
            }
            while (Tools.Client.Items.byType(ref ItemstoBank).Count > 1)
            {
                Tools.Client.Items.byType(ref ItemstoBank).First().Move(ref DropChest);
                Thread.Sleep(1500);
            }

        }

        private void Chop(Tree _Tree)
        {
            Tools.Client.onCliLocSpeech += new LiteClient.onCliLocSpeechEventHandler(Client_onCliLocSpeech);
            Tools.Client.onSpeech += new LiteClient.onSpeechEventHandler(Client_onSpeech);
            Tools.GUI.UpdateLog("Chopping");
            while (true)
            {
                if (Tools.Client.Player.Layers.RightHand != null)
                {
                    if (!AxeType.Contains<ushort>(Tools.Client.Player.Layers.RightHand.Type))
                    {
                        Item _myaxe = Tools.Client.Player.Layers.BackPack.Contents.byType(ref AxeType).First<Item>();
                        Serial player = (Serial)Tools.Client.Player.Serial;
                        if (_myaxe != null) { _myaxe.Equip(UOLite2.Enums.Layers.RightHand); ;}
                    }
                }
                else
                {
                        Item _myaxe = Tools.Client.Player.Layers.BackPack.Contents.byType(ref AxeType).First<Item>();
                        Serial player = (Serial)Tools.Client.Player.Serial;
                        if (_myaxe != null) { _myaxe.Equip(UOLite2.Enums.Layers.RightHand); ;}

                }
                Tools.Client.Targeting = false;
                Tools.Client.Player.Layers.RightHand.DoubleClick();

                //Client.Items.byType(ref AxeType).FirstOrDefault<Item>().DoubleClick();
                WaitForTarget(1500);
                Tools.Client.Target(_Tree.X, _Tree.Y, _Tree.Z, (ushort)_Tree.ID);
                System.Diagnostics.Stopwatch timeout = new System.Diagnostics.Stopwatch();
                timeout.Start();
                while (timeout.ElapsedMilliseconds <= 3000)
                {
                    if (Journal[0].Contains("not enough wood")) { return; }
                    if (Journal[0].Contains("you chop")) { break; }
                    if (Journal[0].Contains("you hack at the tree")) { break; }
                    if (Journal[0].Contains("found")) { break; }
                    if (Journal[0].Contains("use an axe")) { return; }
                }



            }
        }
        void WaitForTarget(long lTimeout)
        {
            System.Diagnostics.Stopwatch swatch = new System.Diagnostics.Stopwatch();
            swatch.Start();
            while ((!Tools.Client.Targeting) && (swatch.ElapsedMilliseconds <= lTimeout))
            {
                Thread.Sleep(50);
            }
            swatch.Stop();

        }
        void Client_onSpeech(ref LiteClient Client, Serial Serial, ushort BodyType, UOLite2.Enums.SpeechTypes SpeechType, ushort Hue, UOLite2.Enums.Fonts Font, string Text, string Name)
        {
            Journal.Insert(0, Text);

            if (Journal.Count > 100) { Journal.RemoveAt(99); }
            // todo handle additional text stuff
        }

        void Client_onCliLocSpeech(ref LiteClient Client, Serial Serial, ushort BodyType, UOLite2.Enums.SpeechTypes SpeechType, ushort Hue, UOLite2.Enums.Fonts Font, uint CliLocNumber, string Name, string ArgsString)
        {
            Journal.Insert(0, Client.CliLocStrings.get_Entry(CliLocNumber));
            if (Journal.Count > 100) { Journal.RemoveAt(99); }
            //UpdateLog("Clioc: " + Name + " : " + Client.CliLocStrings.get_Entry(CliLocNumber));
        }

        private List<Tree> grabTile(uint[] TreeTiles, ToFromCoords range)
        {
            // takes a list of tree types and a range around char to search for them returns a list of trees
            List<Tree> _TreeList = new List<Tree>();
            for (int x = range.XRange.Lower; x <= range.XRange.Upper; x++)
            {
                for (int y = range.YRange.Lower; y <= range.YRange.Upper; y++)
                {
                    Tile temptile = Ultima.Map.Trammel.Tiles.GetLandTile(x + Startloc.X, y + Startloc.Y);
                    HuedTile[] temptile2 = Ultima.Map.Trammel.Tiles.GetStaticTiles(x + Startloc.X, y + Startloc.Y);
                    foreach (HuedTile T in temptile2)
                    {
                        if (TreeTiles.Contains<uint>((uint)T.ID))
                        {
                            Tree mytree = new Tree(x + Startloc.X, y + Startloc.Y, temptile.Z, temptile.ID);
                            _TreeList.Add(mytree);
                        }
                    }
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





        public bool myScriptRunning = true; 
    }
}