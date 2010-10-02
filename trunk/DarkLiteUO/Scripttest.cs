using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using UOLite2;
using System.Windows.Forms;
using UOLite2.SupportClasses;

namespace DarkLiteUO
{
    public partial class Script : IScriptInterface
    {


        private bool myScriptRunning = true;
            ushort ScrollType = 000;
            ushort ToolType = 4031;
            uint CraftGumpType = 2002155655;

           
        public void Main()
        {


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
                    Thread.Sleep(1000);
                }
                

               
            }

            GUI.UpdateLog("Script Ended");

            return;
        }

        void Client_onNewGump(ref LiteClient Client, ref UOLite2.SupportClasses.Gump Gump2)
        {
            if(Gump2.GumpID == CraftGumpType)
            {
                GUI.UpdateLog("Responding to Gump Event Match");
                // we know make last is #22 easier than extracting the info from the gump
                
                GumpMenuSelection(Gump2.Serial, Gump2.GumpID, 21);
            }
        }
       
        public void Stop()
        {
            myScriptRunning = false;
        }


       
    }
}
