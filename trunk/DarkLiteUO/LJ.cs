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
          
           
        public void Main()
        {

            GUI.UpdateLog("Script Started");
           HuedTile[] mtlist = GetStatics(Client.Player.X,Client.Player.Y);
            Tile mytile = GetLandTile(Client.Player.X,Client.Player.Y);
            HashSet<Item> Tools;
            Item Tool;
           while (myScriptRunning)
            {
                
                 
            }
            GUI.UpdateLog("Script Ended");
            return;
        }
       
        public void Stop()
        {
            myScriptRunning = false;
        }


       
    }
}
