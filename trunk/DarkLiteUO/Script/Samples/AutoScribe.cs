using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using UOLite2;
using System.Windows.Forms;
using UOLite2.SupportClasses;
using Ultima;
using UOLite2.Enums.Common;
namespace DarkLiteUO
{
    public class AutoScribe : IScriptInterface
    {
        //User Edit vars
        private Serial RunebookID;
        private ushort HomeruneNum = 1;
        private ushort ShopruneNum = 2;
        private Serial BankChest;
        private Serial VendorID;

        private Scroll ScrollToCraft;
        private ushort BlankScroll;
        LiteClient Client; // our liteclient
        _ScriptTools T;
        myTabPage GUI; // Gives access to UpdateLog
        private bool myScriptRunning = true;
        ushort ToolType = 4031; // Can use T.EUOtoUint("XXX") instead of the dec graphic id
        uint CraftGumpType = 2002155655;
        ushort stage = 1; // lets our event know what response to send
        private class Scroll
        {
            private ItemTypes[] reags;
            private uint CircleBtnNum;
            private uint ScrollBtnNum;
            private ushort _ScrollType;
            public ushort ScrollType { get { return _ScrollType; } set { _ScrollType = value; } }
            public ItemTypes[] Reagants { get { return reags; } set { reags = value; } }
            public uint CircleButton { get { return CircleBtnNum; } set { CircleBtnNum = value; } }
            public uint ScrollButton { get { return ScrollBtnNum; } set { ScrollBtnNum = value; } }
            public Scroll(ItemTypes[] Reags, uint CircleBtnNum, uint ScrollBtnNum, ushort scrolltype)
            {
                _ScrollType = scrolltype;
                this.reags = Reags;
                this.CircleBtnNum = CircleBtnNum;
                this.ScrollBtnNum = ScrollBtnNum;
            }

        }
        public void Start(ref ScriptTools ST)
        {
            // you could just have it set one ScriptTools item, but then its an extra name to type
            this.T = ST.Tools;
            this.Client = ST.Client;
            this.GUI = ST.GUI;
        }
        public void Stop()
        {
            myScriptRunning = false;
        }
        // todo Add gold per hour, total gold etc stats to xml
        //craft diff items depending on skill
        // 
        public void Main()
        {
            GUI.UpdateLog("Script Started");
            T.Recall(RunebookID, ShopruneNum);
            ScribeLoop();
            GUI.UpdateLog("Script Ended");

        }

       
        public void ScribeLoop()
        {
            VendorID = T.EUOToInt("LAB");
            RunebookID = new Serial(T.EUOToInt("XDKSKMD"));
            BankChest = new Serial(T.EUOToInt("XXGKKMD"));
            BlankScroll = T.EUOToUshort("DPF");
            Item Pen; // 
            Client.onNewGump += new LiteClient.onNewGumpEventHandler(Client_onNewGump); // Grab the event fired on new gumps
            while (true)
            {
                
                Pen = Client.Items.byType(ref ToolType).First(); // sets our pen to the first found object
                if (Pen == null) { break; } // no pens we exit script
                Pen.DoubleClick();
                while (Client.Items.Contains(ref Pen)) // Use the same pen till its gone
                {
                    ScrollToCraft = ChooseScroll();
                    Check();
                    // Gump auto reopens so our event fires each time so we can sleep the thread
                    // Add mat checks, Mana etc
                    Thread.Sleep(500);
                    if (Client.Player.Mana < 50)
                    {
                        Client.onNewGump -= Client_onNewGump; // stop crafting
                        Client.Skills[(int)UOLite2.Enums.Skills.Meditation].Use(); // Use med
                        while (Client.Player.Mana < Client.Player.ManaMax) { Thread.Sleep(500); } // sleep till mana is full
                        Client.onNewGump += new LiteClient.onNewGumpEventHandler(Client_onNewGump); // Hook our event again
                        Pen.DoubleClick();// re open the gump
                    }
                }
            }
            
            return;
        }

        private Scroll ChooseScroll()
        {
            ushort skillval = Client.Skills[(int)UOLite2.Enums.Skills.Inscription].Value;
            if (skillval < 300) { GUI.UpdateLog(" Skill below 30 please train first"); myScriptRunning = false; }
            if (skillval > 700) { return new Scroll(new ItemTypes[] { ItemTypes.MandrakeRoot, ItemTypes.BloodMoss },36,16,T.EUOToUshort("FUL"));}
            if (skillval > 500) { }
            if (skillval > 300) { return new Scroll(new ItemTypes[] { ItemTypes.Nightshade }, 1, 1,0); }
            return null;
        }

        private void Check()
        {

            try { if (T.Finditem(ScrollToCraft.ScrollType, Client.Player.Layers.BackPack.Serial).Amount > 25) { T.Sell(VendorID, ScrollToCraft.ScrollType); } }
            catch { }

            try { if (T.Finditem((ushort)ItemTypes.GoldCoins, Client.Player.Layers.BackPack.Serial).Amount > 7500) { Bank(); } }
            catch { }
            foreach (ItemTypes I in ScrollToCraft.Reagants)
            {
                if (T.Finditem((ushort)I, Client.Player.Layers.BackPack.Serial) == null)
                {
                    T.Buy(VendorID, (ushort)I, 25);
                }
            }
            if (T.Finditem(BlankScroll, Client.Player.Layers.BackPack.Serial) == null)
            {
                T.Buy(VendorID, BlankScroll, 25);
            }
            if (T.Finditem(ToolType, Client.Player.Layers.BackPack.Serial) == null)
            {
                T.Buy(VendorID, ToolType, 25);
            }
        }


        private void Bank()
        {
            T.Recall(RunebookID, HomeruneNum );
            T.Finditem((ushort)ItemTypes.GoldCoins, Client.Player.Layers.BackPack.Serial).Move(ref BankChest);
            T.Recall(RunebookID, ShopruneNum);
        }

        void Client_onNewGump(ref LiteClient Client, ref UOLite2.SupportClasses.Gump Gump2)
        {
            // this gets called on every new gump
            if (Gump2.GumpID == CraftGumpType) // check if the gump is the crafting gump
            {
                //GUI.UpdateLog("Responding to Gump Event Match");
                switch (stage)
                {
                    case 1: // This is the inital button we hit, in this case Circle 6 Button numbers can be got by recording a razor macro
                        // Button numbers are mostly the same, IE 21 is make last on craft gumps
                        GUI.UpdateLog("Case 1 Clicking Circle 6");
                        T.GumpMenuSelection(Gump2.Serial, Gump2.GumpID, ScrollToCraft.CircleButton);//36
                        stage = 2;
                        break;
                    case 2:
                        GUI.UpdateLog("Case 2 Clicking Explosion");
                        T.GumpMenuSelection(Gump2.Serial, Gump2.GumpID, ScrollToCraft.ScrollButton);//16
                        stage = 3;
                        break;
                    case 3:// After the inital crafting we only press make last
                        GUI.UpdateLog("Case 3 Make Last");
                        T.GumpMenuSelection(Gump2.Serial, Gump2.GumpID, 21);
                        break;
                }


            }
        }

        
    }

}