using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using UOLite2;
namespace DarkLiteUO
{

    public partial class myTabPage : TabPage// : UserControl//
    {
        
        void Client_onTargetRequest(ref LiteClient Client)
        {
                        //throw new NotImplementedException();
        }

        void Client_onPlayerMove(ref LiteClient Client)
        {
           // throw new NotImplementedException();
        }

   

        void Client_onNewItem(ref LiteClient Client, Item Item)
        {
            //throw new NotImplementedException();
        }

        void Client_onNewGump(ref LiteClient Client, ref UOLite2.SupportClasses.Gump Gump)
        {
            
           // UpdateLog("GUMP OUTPUT GumpID = " + Gump.GumpID.ToString() + " Gump Serial: " + Gump.Serial.ToString());
           // throw new NotImplementedException();
        }

        void Client_onMovementBlocked(ref LiteClient Client)
        {
            UpdateLog("Move Failed");
        }

        void Client_onConnectionLoss(ref LiteClient Client)
        {
            if (ScriptThread != null)
            {
                if (ScriptThread.ThreadState == ThreadState.Running)
                {
                    ScriptThread.Suspend();
                    UpdateLog("Script Thread paused");
                }
            }
            //bConnected = false;
            UpdateLog("Dissconnected, attempting reconnect.");
            Connect();
        }

        void Client_onSkillUpdate(ref LiteClient Client, ref UOLite2.SupportClasses.Skill OldSkill, ref UOLite2.SupportClasses.Skill NewSkill)
        {

            UpdateLog("SKill: " + NewSkill.Value.ToString());
        }

        void Client_onRecievedServerList(ref UOLite2.SupportClasses.GameServerInfo[] ServerList)
        {
            byte temp = 0;
            Client.ChooseServer(ref temp);
        }

        void Client_onSpeech(ref LiteClient Client, Serial Serial, ushort BodyType, UOLite2.Enums.SpeechTypes SpeechType, ushort Hue, UOLite2.Enums.Fonts Font, string Text, string Name)
        {
            UpdateLog("Speech: " + Name + " : " + Text);
            // todo handle additional text stuff
        }

        void Client_onCliLocSpeech(ref LiteClient Client, Serial Serial, ushort BodyType, UOLite2.Enums.SpeechTypes SpeechType, ushort Hue, UOLite2.Enums.Fonts Font, uint CliLocNumber, string Name, string ArgsString)
        {
            UpdateLog("Clioc: " + Name + " : " + Client.CliLocStrings.get_Entry(CliLocNumber));
        }

        void Client_onLoginConfirm(ref Mobile Player)
        {
            UpdateLog("Login Confirmed");
        }

        void Client_onLoginDenied(ref string Reason)
        {
            this.UpdateLog("Login Denied: " + Reason);
        }
        private void Client_onCharacterListReceive(ref UOLite2.LiteClient Client, System.Collections.ArrayList CharacterList)
        {
            UpdateLog("CharList Received");
            try
            {
                UOLite2.Structures.CharListEntry temp = (UOLite2.Structures.CharListEntry)CharacterList[_config.CharSlot -1];
                Client.ChooseCharacter(ref temp.Name, ref temp.Password, temp.Slot);
            }
            catch
            {
                UpdateLog("Login Failed no Charlist entries");
            }

        }

        private void Client_onPacketSend(ref UOLite2.LiteClient Client, ref Byte[] data)
        {
           // this.UpdateLog("Sent: " + GetString(data));
        }

 
        private void Client_onLoginComplete()
        {
            UpdateLog("Login Complete");
            bConnected = true;
            if (ScriptThread != null)
            {
            
                if (ScriptThread.ThreadState == ThreadState.Suspended)
                {
                    ScriptThread.Resume();
                }
            }
        }
        private void Client_onError(ref String Description)
        {
            UpdateLog("Error: " + Description);
        }
        public static byte[] GetBytes(string text)
        {
            return ASCIIEncoding.UTF8.GetBytes(text);
        }
        public static String GetString(byte[] text)
        {
            return ASCIIEncoding.UTF8.GetString(text);
        }
        void Client_onPacketReceive(ref LiteClient Client, ref byte[] bytes)
        {
            switch (bytes[0])
            {
                case 0x98: // All Names
                    HandleAllNames(bytes);
                    break;
                default:
                    break;
            }
        }

        private void HandleAllNames(byte[] bytes)
        {
            UOLite2.SupportClasses.BufferHandler buff = new UOLite2.SupportClasses.BufferHandler(bytes, true);
            byte x = buff.readbyte();
            ushort y = buff.readushort();
            uint id = buff.readuint();
            string name = buff.readstr();
            Serial serial = new Serial(id);
            if (serial != Client.Player.Serial)
            {
                Client.Mobiles.get_Mobile(serial)._Name = name;
                //UpdateLog("Setting npc name to: " + Client.Mobiles.get_Mobile(serial).Name + id);
            }
        }

        void Client_onNewMobile(ref LiteClient Client, Mobile Mobile)
        {
            // Request the mobiles name
            UOLite2.SupportClasses.BufferHandler buff = new UOLite2.SupportClasses.BufferHandler(7,true);
            
            buff.WriteByte(0x98);
            buff.writeushort(7);
            buff.writeuint(Mobile.Serial.Value);
            Send(buff.buffer);
        }

        private void Send(byte[] data)
        {
            byte[] dat = data;
            Client.Send(ref dat);
        }

    }
}
