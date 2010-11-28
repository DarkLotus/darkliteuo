using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UOLite2;
using System.Windows.Forms;
using Ultima;
using System.Drawing;
namespace DarkLiteUO
{

    public partial class _ScriptTools
    {
        public void ContextMenu(Serial npc,ushort tagnum)
        {
            // We dont bother waiting for the context menu response packet, all it does is tell us the names of each item
            // tagnum is the context option you want to select, this is 0 based
           UOLite2.SupportClasses.BufferHandler buff = new UOLite2.SupportClasses.BufferHandler(9,true);
       
            buff.WriteByte(0xBF);
             buff.writeushort(9);
            buff.writeushort(0x13);
            buff.writeuint(npc.Value);
            Send(buff.buffer);
            Thread.Sleep(500);
            
            UOLite2.SupportClasses.BufferHandler b2 = new UOLite2.SupportClasses.BufferHandler(11, true);
            b2.WriteByte(0xBF);
            b2.writeushort(11);
            b2.writeushort(0x15);
            b2.writeuint(npc.Value);
            b2.writeushort(tagnum);
            Send(b2.buffer);
        }


        private void Send(byte[] data)
        {
            byte[] dat = data;
            Client.Send(ref dat);
        }
    }
}
