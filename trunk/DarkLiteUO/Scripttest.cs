/*using System;
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