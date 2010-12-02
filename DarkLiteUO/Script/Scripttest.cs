using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using UOLite2;
using System.Windows.Forms;
using UOLite2.SupportClasses;
using Ultima;
using DarkLiteUO;
namespace DarkLiteUO
{
    public class Script2 : IScriptInterface
    {
        ScriptTools Tools;
        
        public void Main()
        {


            _ScriptTools T = Tools.Tools;
            LiteClient C = Tools.Client; // these just save a bit of typing,


            Serial backpackid = C.Player.Layers.BackPack.Serial;
            Item myaxe = T.Finditem("NBK",backpackid);// Sample Finditem usage find by an EUO type inside a container

            ushort[] itemstobuy = new ushort[] { T.EUOToUshort("WZF"), T.EUOToUshort("XXX") }; // add two items to our buylist
            Serial vendorid = new Serial(T.EUOToInt("LZ"));
            // Before calling buy you need to have the serial of the vendor you want to buy off
            T.Buy(vendorid, itemstobuy);// Calling this will send a buymenu req, which then adds an event handler so when you receive the buy menu it auto responds with what you want to buy
            // if vendor does not exist, or items are not their nothing will happen. Add a wait after each buy as its non blocking so give 2-3 seconds for the server to reply etc
            // Both buy and sell will do so at the max amount the vendor has or you have.
            //T.Sell(vendorid, itemstobuy); // Same as above.
            //ScriptTools.Coordinate[] myrail = T.ImportRail("myrail.txt"); // imports the rail from text really no need to load Z as pathfinding doesnt take Z anyway
            //foreach (ScriptTools.Coordinate c in myrail)
            //{
            //    T.Pathfind(c.X, c.Y, 0);
            //}
            
        }
        public void Start(ref ScriptTools ST)
        {
            this.Tools = ST;
        }
        public void Stop()
        {

        }
    }
}
/*    public partial class Script : IScriptInterface
    {
        Serial Runebookserial;
        ushort gatetype;

        public void Main()
        {
            GUI.UpdateLog("Script Started");
            //Pathfind();
        
            Runebookserial = new Serial(EUOToInt("LLMBNMD"));// Set our runebooks ID, using an EUO ID.
            gatetype = (ushort)EUOToInt("OTF");// set to type of gate
            string mymsg = "Gate!";
            bool docast = true;
            while (myScriptRunning)
            {
                HashSet<Item> gates = Client.Items.byType(ref gatetype); // get a list of all gates
                foreach (Item i in gates)
                {
                    if ((i.X == Client.Player.X) && (i.Y == Client.Player.Y))
                    {
                        docast = false; // if the gates x/y is same as players dont cast.
                        break;
                    }
                    docast = true;
                }

                if (docast)
                {
                    Speak(mymsg);
                    Client.CastSpell(UOLite2.Enums.Spell.GateTravel);
                    while (!Client.Targeting) { Thread.Sleep(10); } // wait for the target cursor
                    Client.Target(Runebookserial);
                    Thread.Sleep(5000);
                }
            }

        }

        
    }
}

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
    public partial class Script : IScriptInterface
    {
        private bool myScriptRunning = true;
            //ushort ScrollType = 000;
            ushort ToolType = 4031;
            uint CraftGumpType = 2002155655;
            ushort stage = 1;           
           
        public void Main()
        {
            HuedTile[] mtlist = GetStatics(Client.Player.X, Client.Player.Y);
            Tile mytile = GetLandTile(Client.Player.X, Client.Player.Y);
            GUI.UpdateLog("Script Started");

            HashSet<Item> Tools;
            Item Tool;
            Client.onNewGump +=new LiteClient.onNewGumpEventHandler(Client_onNewGump);
            while (myScriptRunning)
            {
                Tools = Client.Items.byType(ref ToolType);
                Tool = Tools.First();
                Tool.DoubleClick();
                while (Client.Items.Contains(ref Tool))
                {
                    // Gump auto reopens so sleep till tool is gone
                    // Add mat checks, Mana etc
                    Thread.Sleep(500);
                    if (Client.Player.Mana < 50)
                    {
                        Client.onNewGump -= Client_onNewGump;
                        Client.Skills[(int)UOLite2.Enums.Skills.Meditation].Use();
                        while (Client.Player.Mana < 100) { Thread.Sleep(500); }
                        Client.onNewGump += new LiteClient.onNewGumpEventHandler(Client_onNewGump);
                        Tool.DoubleClick();
                    }
                }              
            }
            GUI.UpdateLog("Script Ended");
            return;
        }

        void Client_onNewGump(ref LiteClient Client, ref UOLite2.SupportClasses.Gump Gump2)
        {
            if(Gump2.GumpID == CraftGumpType)
            {
                //GUI.UpdateLog("Responding to Gump Event Match");
                switch(stage)
                {
                    case 1:
                        GUI.UpdateLog("Case 1 Clicking Circle 6");
                        GumpMenuSelection(Gump2.Serial, Gump2.GumpID, 36);
                    stage = 2;
                    break;
                    case 2:
                    GUI.UpdateLog("Case 2 Clicking Explosion");
                        GumpMenuSelection(Gump2.Serial, Gump2.GumpID, 16);
                    stage = 3;
                    break;
                    case 3:
                    GUI.UpdateLog("Case 3 Make Last");
                        GumpMenuSelection(Gump2.Serial, Gump2.GumpID, 21);
                    break;
                }

               // GumpMenuSelection(Gump2.Serial, Gump2.GumpID, 21);
            }
        }
       
        public void Stop()
        {
            myScriptRunning = false;
        }


       
    }
}
*/