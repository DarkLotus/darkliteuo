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

        public UOLite2.Item Finditem(ushort type)
        {
            foreach (UOLite2.Item i in Client.Items.Items)
            {
                if(i.Type == type)
                {return i; }
            }
            return null;
        }
        public UOLite2.Item Finditem(string type)
        {
            ushort _type = EUOToUshort(type);
            foreach (UOLite2.Item i in Client.Items.Items)
            {
                if (i.Type == _type)
                { return i; }
            }
            return null;
        }
        public UOLite2.Item Finditem(ushort type,Serial Container)
        {
            foreach (UOLite2.Item i in Client.Items.Items)
            {
                if((i.Type == type) && ( i.Container == Container))
                {return i; }
            }
            return null;
        }
        public UOLite2.Item Finditem(string type, Serial Container)
        {
            ushort _type = EUOToUshort(type);
            foreach (UOLite2.Item i in Client.Items.Items)
            {
                if ((i.Type == _type) && (i.Container == Container))
                { return i; }
            }
            return null;
        }

        public UOLite2.Item[] Finditem(ushort[] type, Serial Container)
        {
            List<Item> items = new List<Item>();
            foreach (UOLite2.Item i in Client.Items.Items)
            {
                if ((type.Contains(i.Type)) && (i.Container == Container))
                { items.Add(i); }
            }
            return items.ToArray();
        }
    }
}
