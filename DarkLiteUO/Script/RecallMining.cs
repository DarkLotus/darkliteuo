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
    public class Script3 : IScriptInterface
    {
        ScriptTools Tools;
        bool bRunning;
        bool bDebug = true;
        uint[] MiningTiles;
        ushort[] Pickaxe;
        Serial DropChest;
        ushort[] ItemstoBank;
        ushort Tinkertools;
        Serial runebookid;
        Thread mythread;
        public void Main()
        {
            Tools.GUI.UpdateLog("Script Started");
            Tools.Client.Player.DoubleClick(); // Open paperdoll
            Pickaxe = new ushort[2] { EUOToUshort("BSF"), 3907 };
            MiningTiles = new uint[] { 3230, 3274, 3275, 3276, 3277, 3280, 3283, 3286, 3288, 3290, 3293, 3296, 3299, 3302 };
            DropChest = Tools.Tools.EUOToInt("XXGKKMD");
            ItemstoBank = new ushort[] { EUOToUshort("DWJ"), EUOToUshort("TLK"), EUOToUshort("YWS"), EUOToUshort("NWS"), EUOToUshort("BWR"), EUOToUshort("XWS"), EUOToUshort("FXS") };
            runebookid = 878787;

            while (bRunning)
            {
                for(int i=0;i <= 16;i++)
                {
                    Recall(runebookid,i);
                    mythread = new Thread(Mine); mythread.Start();
                    while(mythread.ThreadState == ThreadState.Running)
                    {
                        // hide, check for enemies etc check weight stop mining if needed
                        // say we want to bank, we would stop the mining thread, then call Bank(), once bank ends we drop out of the while and Recall again
                        //
                    }

                }
            }
        }

        private void Mine()
        {
            throw new NotImplementedException();
        }

        private void Recall(Serial runebookid, int i)
        {
            throw new NotImplementedException();
        }




        private ushort EUOToUshort(string p)
        {
            return Tools.Tools.EUOToUshort(p);
        }

        public void Start(ref ScriptTools ST)
        {
            this.Tools = ST;
            bRunning = true;
        }
        public void Stop()
        {
            bRunning = false;
        }
    }
}