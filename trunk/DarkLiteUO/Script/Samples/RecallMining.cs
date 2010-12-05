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
    public class Rmining : IScriptInterface
    {
        bool Debug;
        List<string> Journal = new List<string>();
        private LiteClient C;
        private _ScriptTools T;
        private myTabPage G;

        private bool WaitForCliloc = false;
        ushort[] OreTiles;
        ushort[] PickType;
        Serial RuneBookid;// Rune home should be first rune
        static ushort RunesInBook = 16; // amount of runes in the book
        Serial DropChest;
        ushort[] ItemstoBank;
        private struct MiningTile
        {
            public ushort X;
            public ushort Y;
            public ushort Z;
            public ushort ID;
            public MiningTile(int X, int Y, int Z, ushort ID)
            {
                this.X = Convert.ToUInt16(X);
                this.Y = Convert.ToUInt16(Y);
                this.Z = Convert.ToUInt16(Z);
                this.ID = ID;
            }
        }


        public void Start(ref ScriptTools ST)
        {
            this.T = ST.Tools;
            this.C = ST.Client;
            this.G = ST.GUI;
        }
        public void Stop()
        {

        }
        public void Main()
        {

            Debug = true;
            G.UpdateLog("Script Started");
            C.Player.DoubleClick(); // Open paperdoll

            PickType = new ushort[2] { EUOToUshort("BSF"), 3907 };
            OreTiles = new ushort[] { 3274, 3275, 3276, 3277, 3280, 3283, 3286, 3288, 3290, 3293, 3296, 3299, 3302 };
            DropChest = T.EUOToInt("XXGKKMD");
            ItemstoBank = new ushort[] { EUOToUshort("ZLK"), EUOToUshort("TLK"), EUOToUshort("YWS"), EUOToUshort("NWS"), EUOToUshort("BWR"), EUOToUshort("XWS"), EUOToUshort("FXS") };


            C.onCliLocSpeech += new LiteClient.onCliLocSpeechEventHandler(Client_onCliLocSpeech);
            C.onSpeech += new LiteClient.onSpeechEventHandler(Client_onSpeech);
            while (myScriptRunning)
            {
                for (ushort i = 1; i <= 16; i++)
                {
                    T.Recall(RuneBookid, i);
                    MiningLoop(); // Mines 4 tiles around player, X +- 2 and Y +- 2
                }

            }
            C.onCliLocSpeech -= Client_onCliLocSpeech;
            C.onSpeech -= Client_onSpeech;
            G.UpdateLog("Script Ended");
        }

        private ushort EUOToUshort(string p)
        {
            return T.EUOToUshort(p);
        }


        public void MiningLoop()
        {
            List<MiningTile> mTiles = grabTile();

            foreach (MiningTile i in mTiles)
            {
                MineTile(i);

                if (C.Player.Weight > C.Player.MaxWeight - 50) { Bank(); break; }
                if (!myScriptRunning) { return; }
            }

        }

        private List<MiningTile> grabTile()
        {
            List<MiningTile> mytiles = new List<MiningTile>();
            Ultima.Map mymap = T.Getmap(C);
            mytiles.Add(new MiningTile(C.Player.X + 2, C.Player.Y, C.Player.Z, mymap.Tiles.GetLandTile(C.Player.X + 2, C.Player.Y).ID));
            mytiles.Add(new MiningTile(C.Player.X - 2, C.Player.Y, C.Player.Z, mymap.Tiles.GetLandTile(C.Player.X - 2, C.Player.Y).ID));
            mytiles.Add(new MiningTile(C.Player.X, C.Player.Y - 2, C.Player.Z, mymap.Tiles.GetLandTile(C.Player.X, C.Player.Y - 2).ID));
            mytiles.Add(new MiningTile(C.Player.X, C.Player.Y + 2, C.Player.Z, mymap.Tiles.GetLandTile(C.Player.X, C.Player.Y + 2).ID));
            return mytiles;
        }
        private void Bank()
        {
            G.UpdateLog("Banking");
            while (T.Finditem(DropChest.ToEasyUOString()) == null)
            {
                T.Recall(RuneBookid, 1);
            }// this is probably a bad idea 

            while (C.Items.byType(ref ItemstoBank).Count > 1)
            {
                C.Items.byType(ref ItemstoBank).First().Move(ref DropChest);
                Thread.Sleep(1500);
            }

        }

        private void MineTile(MiningTile _miningtile)
        {

            G.UpdateLog("Mining");
            while (true)
            {
                
                if (T.Finditem(PickType, C.Player.Layers.BackPack.Serial) == null)
                {
                    // craft more picks/buy more
                    throw new NotSupportedException();
                }
                C.Targeting = false;
                T.Finditem(PickType, C.Player.Layers.BackPack.Serial).First().DoubleClick();

                WaitForTarget(5000);
                G.UpdateLog("Targeting Tree");
                C.Target(_miningtile.X, _miningtile.Y, _miningtile.Z, _miningtile.ID);

                Thread.Sleep(100);
                System.Diagnostics.Stopwatch timeout = new System.Diagnostics.Stopwatch();
                timeout.Start();
                while (timeout.ElapsedMilliseconds <= 4000)
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
            while ((!C.Targeting) && (swatch.ElapsedMilliseconds <= lTimeout))
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

   
        

        public bool myScriptRunning = true;
    }
}