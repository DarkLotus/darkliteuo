using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using UOLite2;
using System.Windows.Forms;
using UOLite2.SupportClasses;
using Ultima;
namespace DarkLiteUO
{
    public class Scribe : IScriptInterface
    {
        LiteClient Client; // our liteclient
        _ScriptTools T;
        myTabPage GUI; // Gives access to UpdateLog
        private bool myScriptRunning = true;
        ushort ToolType = 4031; // Can use T.EUOtoUint("XXX") instead of the dec graphic id
        uint CraftGumpType = 2002155655;
        ushort stage = 1; // lets our event know what response to send
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
        public void Main()
        {
            GUI.UpdateLog("Script Started");
            Item Pen; // 
            Client.onNewGump += new LiteClient.onNewGumpEventHandler(Client_onNewGump); // Grab the event fired on new gumps
            while (myScriptRunning)
            {  
                Pen = Client.Items.byType(ref ToolType).First(); // sets our pen to the first found object
                if (Pen == null) { break; } // no pens we exit script
                Pen.DoubleClick();
                while (Client.Items.Contains(ref Pen)) // Use the same pen till its gone
                {
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
            GUI.UpdateLog("Script Ended");
            return;
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
                        T.GumpMenuSelection(Gump2.Serial, Gump2.GumpID, 36);
                        stage = 2;
                        break;
                    case 2:
                        GUI.UpdateLog("Case 2 Clicking Explosion");
                        T.GumpMenuSelection(Gump2.Serial, Gump2.GumpID, 16);
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

    public partial class Gater : IScriptInterface
    {
        ScriptTools ST;
        Serial Runebookserial;
        ushort gatetype;
        bool myScriptRunning;
         public void Start(ref ScriptTools ST)
        {
            this.ST = ST;
             myScriptRunning = true;
        }
        public void Stop()
        {
            myScriptRunning = false;
        }
        public void Main()
        {
            ST.GUI.UpdateLog("Script Started");
            //Pathfind();
        
            Runebookserial = new Serial(ST.Tools.EUOToInt("LLMBNMD"));// Set our runebooks ID, using an EUO ID.
            gatetype = (ushort)ST.Tools.EUOToInt("OTF");// set to type of gate
            string mymsg = "Gate!";
            bool docast = true;
            while (myScriptRunning)
            {
                HashSet<Item> gates = ST.Client.Items.byType(ref gatetype); // get a list of all gates
                if (gates != null)
                {
                    foreach (Item i in gates) // Lopp thru all the found gates
                    {
                        if ((i.X == ST.Client.Player.X) && (i.Y == ST.Client.Player.Y))
                        {
                            docast = false; // if the gates x/y is same as players dont cast.
                            break;
                        }
                        docast = true; // No gates under us found so we can cast
                    }
                }
                if (docast)
                {
                    ST.Tools.Speak(mymsg);
                    ST.Client.Targeting = false;
                    ST.Client.CastSpell(UOLite2.Enums.Spell.GateTravel);
                    while (!ST.Client.Targeting) { Thread.Sleep(10); } // wait for the target cursor
                    ST.Client.Target(Runebookserial);
                    Thread.Sleep(5000);
                }
            }

        }

        
    }
}