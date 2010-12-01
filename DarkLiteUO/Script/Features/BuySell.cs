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
        private Mobile _mobile;
        private ushort[] _items;
        private ushort _quanity;
        #region Sell
        internal void Sell(Serial VendorID, ushort[] Itemtypes)
        {
            _mobile = Client.Mobiles.get_Mobile(VendorID);
            if (_mobile == null) { return; }         
            _items = Itemtypes;
            Client.onPacketReceive += new LiteClient.onPacketReceiveEventHandler(HandleSellList);
            ContextMenu(_mobile.Serial, 2);
            //Speak(npc.Name + " Sell");
        }
        internal void Sell(Serial VendorID, ushort Itemtypes)
        {
            _mobile = Client.Mobiles.get_Mobile(VendorID);
            if (_mobile == null) { return; }
            _items = new ushort[] { Itemtypes };
            Client.onPacketReceive += new LiteClient.onPacketReceiveEventHandler(HandleSellList);
            ContextMenu(_mobile.Serial, 2);
            //Speak(npc.Name + " Sell");
        }
        void HandleSellList(ref LiteClient Client, ref byte[] bytes)
        {
            if (bytes[0] != 0x9E) { return; }
            UOLite2.SupportClasses.BufferHandler buff = new UOLite2.SupportClasses.BufferHandler(bytes,true);
            buff.Position = 7;// or 8?
            //_vendorid = buff.readuint();
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
                if (_items.Contains(model))
                {
                    Items.Add(id);
                    itemquants.Add(amount);
                }

             }


            SellItems(_mobile.Serial, Items, itemquants);
            Client.onPacketReceive -= HandleSellList;
            }
            

        private void SellItems(Serial VendorID, List<uint> Items, List<ushort> itemquants)
        {

            UOLite2.SupportClasses.BufferHandler buff = new UOLite2.SupportClasses.BufferHandler((uint)(9 + (Items.Count * 6)),true);

            buff.WriteByte(0x9F);
            buff.writeushort((ushort)(9 + (Items.Count * 6)));
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
            _quanity = 0;
            _mobile = Client.Mobiles.get_Mobile(VendorID);
            if (_mobile == null) { return; }
            _items = Itemtypes;
            Client.onPacketReceive += new LiteClient.onPacketReceiveEventHandler(HandleBuyWindowOpen);
            ContextMenu(_mobile.Serial, 1);
            //Speak(npc.Name + " Buy");
        }
        public void Buy(Serial VendorID, ushort Itemtype)
        {
            _quanity = 0;
            _mobile = Client.Mobiles.get_Mobile(VendorID);
            if (_mobile == null) { return; }
            _items = new ushort[] { Itemtype };
            Client.onPacketReceive += new LiteClient.onPacketReceiveEventHandler(HandleBuyWindowOpen);
            ContextMenu(_mobile.Serial, 1);
        }
        public void Buy(Serial VendorID, ushort Itemtype, ushort Quantity)
        {
            _quanity = Quantity;
            _mobile = Client.Mobiles.get_Mobile(VendorID);
            if (_mobile == null) { return; }
            _items = new ushort[] { Itemtype };
            Client.onPacketReceive += new LiteClient.onPacketReceiveEventHandler(HandleBuyWindowOpen);
            ContextMenu(_mobile.Serial, 1);
        }
        private void HandleBuyWindowOpen(ref LiteClient Client, ref byte[] bytes)
        {
            /*if (bytes[0] == 0x3C)
            {
                UOLite2.SupportClasses.BufferHandler buff = new UOLite2.SupportClasses.BufferHandler(bytes, true);
                buff.Position = 3;
                ushort numitems = buff.readushort();
                for (int i = 0; i <= numitems; i++)
                {
                    Serial serial = new Serial(buff.readuint());
                    ushort type = buff.readushort();
                    buff.readbyte();
                    ushort count = buff.readushort();
                    buff.readushort(); // X
                    buff.readushort();// Y
                    Serial Container = (buff.readuint() | 0x40000000);
                    GUI.UpdateLog(Container.Value + " Container serial");
                }
            }*/
            if (bytes[0] == 0x74)
            {
                UOLite2.SupportClasses.BufferHandler buff = new UOLite2.SupportClasses.BufferHandler(bytes, true);
                buff.Position = 3;
                Serial vendorid = new Serial(buff.readuint());
                GUI.UpdateLog("Vendors crazy ID = " + vendorid);

                // assuming 3c has been parsed as it should be before this packet is handled.
                List<Item> Items = new List<Item>();

                foreach (Item i in Finditem(_items,vendorid))
                {
                   
                        if (_items.Contains<ushort>(i.Type))
                        {
                            GUI.UpdateLog("Found item to buy" + i.TypeName + "  stack:" + i.Amount);
                            Items.Add(i);
                        }
                    
                }
                BuyItems(_mobile.Serial, Items);
                Client.onPacketReceive -= HandleBuyWindowOpen;
            }
        }
        private void BuyItems(Serial VendorID, List<Item> Items)
        {
            
            UOLite2.SupportClasses.BufferHandler buff = new UOLite2.SupportClasses.BufferHandler((uint)(8+(Items.Count * 7)),true);
            buff.WriteByte(0x3B);
            buff.writeushort((ushort)(8 + (Items.Count * 7)));
            buff.writeuint(VendorID.Value);
            buff.WriteByte(0x02);
            foreach (Item i in Items)
            {
                buff.WriteByte(0x1A);
                buff.writeuint(i.Serial.Value);
                if (_quanity == 0)
                {
                    buff.writeushort(i.Amount);
                }
                else 
                {
                    if (_quanity > i.Amount) { buff.writeushort(i.Amount); }
                    else{buff.writeushort(_quanity); }
                }
            }
                byte[] bf = buff.buffer;
                Client.Send(ref bf);
        }



       
    }
        #endregion
}
