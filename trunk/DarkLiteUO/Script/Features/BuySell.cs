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
        // Todo add Buy Sell for single items with quantity
        private Serial _vendorid;
        private ushort[] _items;
        #region Sell
        public void Sell(Serial VendorID, ushort[] Itemtypes)
        {
            if (!VendorID.IsValid) { return; }
            _vendorid = null;
            _items = Itemtypes;
            Client.onPacketReceive += new LiteClient.onPacketReceiveEventHandler(HandleSellList);
            Speak(Client.Mobiles.get_Mobile(VendorID).Name + " Sell");
        }

        void HandleSellList(ref LiteClient Client, ref byte[] bytes)
        {
            if (bytes[0] != 0x9E) { return; }
            UOLite2.SupportClasses.BufferHandler buff = new UOLite2.SupportClasses.BufferHandler(bytes);
            buff.Position = 3;
            _vendorid = buff.readuint();
            ushort items = buff.readushort();
            List<uint> Items = new List<uint>();
            List<ushort> itemquants = new List<ushort>();
            for (int i = 0; i <= items; i++)
            {
                uint id = buff.readuint();
                ushort model = buff.readushort();
                ushort hue = buff.readushort();
                ushort amount = buff.readushort();
                ushort value = buff.readushort();
                string name = "";
                while (buff.buffer[0] != 0x00)
                {
                    name = name + buff.readchar();
                }
                buff.Position += 1;
                Items.Add(id);
                itemquants.Add(amount);
            }
            
            
                SellItems(_vendorid, Items, itemquants);
            Client.onPacketReceive -= HandleSellList;
            }
            

        private void SellItems(Serial VendorID, List<uint> Items, List<ushort> itemquants)
        {

            UOLite2.SupportClasses.BufferHandler buff = new UOLite2.SupportClasses.BufferHandler((8 + (Items.Count * 6)));

            buff.WriteByte(0x9F);
            buff.writeushort((ushort)(8 + (Items.Count * 6)));
            buff.writeuint(VendorID.Value);
            buff.writeushort((ushort)Items.Count);
            for (int i = 0; i <= Items.Count -1;i++)
            {
                buff.writeuint(Items[i]);
                buff.writeushort(itemquants[i]);
            }
            byte[] bf = buff.buffer;
            Client.Send(ref bf);
        }
        #endregion

        #region Buy
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
        private void BuyItems(Serial VendorID, List<Item> Items)
        {
            
            UOLite2.SupportClasses.BufferHandler buff = new UOLite2.SupportClasses.BufferHandler((7+(Items.Count * 7)));

            buff.WriteByte(0x3B);
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
                Client.Send(ref bf);
        }


    }
        #endregion
}
