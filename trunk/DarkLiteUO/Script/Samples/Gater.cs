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
public class Script : IScriptInterface
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
                docast = true;
                HashSet<Item> gates = ST.Client.Items.byType(ref gatetype); // get a list of all gates
                if (gates != null)
                {
                    foreach (Item i in gates) // Loop thru all the found gates
                    {
                        if ((i.X == ST.Client.Player.X) && (i.Y == ST.Client.Player.Y))
                        {
                            ST.GUI.UpdateLog("Gate found Waiting"); Thread.Sleep(2000);
                            docast = false; // if the gates x/y is same as players dont cast.
                        }

                    }
                }
                   
                
                if (docast)
                {
                    ST.Tools.Speak(mymsg);
                    ST.Client.Targeting = false; // Seems to fix some random issues
                    ST.Client.CastSpell(UOLite2.Enums.Spell.GateTravel);
                    Thread.Sleep(5000);
                    //while (!ST.Client.Targeting) { Thread.Sleep(10); } // wait for the target cursor
                    if (ST.Client.Targeting)
                    {
                        ST.GUI.UpdateLog("Targeting runebook");
                        ST.Client.Target(Runebookserial);
                    }
                    Thread.Sleep(5000);
                }
                Thread.Sleep(100);
            }
            ST.GUI.UpdateLog("Script ending");
        }

        
    }
}