using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Collections;
using UOLite2;

namespace DarkLiteUO
{
    class Script2
    {
        UOLite2.LiteClient Client;
        public Boolean Active;
        Form1 GUI;
        public Script2(UOLite2.LiteClient _client, Form1 temp)
        {
            GUI = temp;
            Client = _client;
            Active = true;
           
        }

        public void main()
        {
            GUI.UpdateLog("Script Started");
            finditem Finditem = new finditem(ref Client);
            Item myitem = Finditem.ByType(3741);
            while(Active)
            {
                if (Finditem._FindID(myitem.Serial))
                {
                    myitem.DoubleClick();
                    Thread.Sleep(8000);
                }
                else
                {
                    myitem = Finditem.ByType(3741);
                }
            }

            GUI.UpdateLog("Script Ended");

            return;

            Active = false;
            GUI.UpdateLog("Connected");
            Event Events = new Event(Client);
            int cnt = 0;
            while (cnt < 10)
            {
                
                //Events.UseObject(1076771726);
                //while (uonet.UOClient.TargCurs == 0) { Thread.Sleep(10); }
                //Events.TargetGround(2563, 489, 0, 0);
                for (int x = 0; x < 40; x++)
                {
                    Thread.Sleep(100);
                //    String temp = (String)uonet.Journal[0];
                  //  if (temp.Contains("loosen")) { uonet.display("FoundLoosen"); break; }
                }
                //Thread.Sleep(3000);

            }
            

           

           GUI.UpdateLog("Script Ended!");
            //Events.Move(3503, 2580, 0, 5);
            // uonet.Send(Packets.Send.Packets.MoveRequestPacket(Direction.West,0,0));
            // Events.UseSkill(Skill.AnimalLore);
            //Thread.Sleep(500);
            //Events.UseSkill(Skill.Tracking);

        }



    }

    class Event
    {
        UOLite2.LiteClient Client;
        private System.Timers.Timer myTimer;
        private System.Timers.Timer myTimer2;
        public Event(UOLite2.LiteClient _client)
        {
            Client = _client;
        }
        public uint EUOToInt(String val)
        //Code by BtbN
        {
            val = val.ToUpper(); // Important!

            uint num = 0;

            for (int p = val.Length - 1; p >= 0; p--)
                num = num * 26 + (((uint)val[p]) - 65);

            num = (num - 7) ^ 0x45;

            return num;
        }
        public String IntToEUO(int num)
        //Code by BtbN
        {
            num = (num ^ 0x45) + 7;

            String res = "";

            do
            {
                res += (Char)(65 + (num % 26));
                num /= 26;
            } while (num >= 1);

            return res;
        }

      

    }
}

