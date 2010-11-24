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
        private Serial _vendorid;
        private ushort[] _items;
        public void Buy(Serial VendorID, ushort[] Itemtypes)
        {
            if (!VendorID.IsValid) { return; }
            _vendorid = null;
            _items = Itemtypes;
            Client.onPacketReceive += new LiteClient.onPacketReceiveEventHandler(HandleBuyWindowOpen);
            Speak(Client.Mobiles.get_Mobile(VendorID).Name + " Buy");
        }
        public void Buy(Serial VendorID, ushort Itemtype)
        {
            if (!VendorID.IsValid) { return; }
            _vendorid = null;
            _items = new ushort[] { Itemtype };
            Client.onPacketReceive += new LiteClient.onPacketReceiveEventHandler(HandleBuyWindowOpen);
            Speak(Client.Mobiles.get_Mobile(VendorID).Name + " Buy");
        }
        private void HandleBuyWindowOpen(ref LiteClient Client, ref byte[] bytes)
        {
            if (bytes[0] != 0x74) { return; }
             UOLite2.SupportClasses.BufferHandler buff = new UOLite2.SupportClasses.BufferHandler(bytes);
             buff.Position = 3;
             _vendorid = buff.readuint();
            List<Item> Items = new List<Item>();
             if (_vendorid.IsValid)
             {
                 Mobile vendor = Client.Mobiles.get_Mobile(_vendorid);
                 foreach (Item i in vendor.Contents.Items)
                 {
                     if(_items.Contains<ushort>(i.Type))
                     {
                         Items.Add(i);
                     }
                 }
                 BuyItems(_vendorid, Items);
             }
             Client.onPacketReceive -= HandleBuyWindowOpen;
        }


        private void BuyItem(Serial VendorID, Item Item)
        {
            UOLite2.SupportClasses.BufferHandler buff = new UOLite2.SupportClasses.BufferHandler(14);
            
            buff.WriteByte(0x3b);
            buff.writeushort(14);
            buff.writeuint(VendorID.Value);
            buff.WriteByte(0x02);
            buff.WriteByte(0x1A);
            buff.writeuint(Item.Serial.Value);
            buff.writeushort(Item.Amount);
            byte[] bf = buff.buffer;

            
        }
        private void BuyItems(Serial VendorID, List<Item> Items)
        {
            
            UOLite2.SupportClasses.BufferHandler buff = new UOLite2.SupportClasses.BufferHandler((7+(Items.Count * 7)));

            buff.WriteByte(0x3b);
            buff.writeushort((ushort)(7 + (Items.Count * 7)));
            buff.writeuint(VendorID.Value);
            buff.WriteByte(0x02);
            foreach (Item i in Items)
            {
                buff.WriteByte(0x1A);
                buff.writeuint(i.Serial.Value);
                buff.writeushort(i.Amount);
            }
                byte[] bf = buff.buffer;


        }
 
    
    }
}
