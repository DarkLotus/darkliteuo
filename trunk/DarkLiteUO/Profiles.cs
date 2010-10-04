using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DarkLiteUO
{
    public class config
    {
        public string Username;
        public string Password;
        public ushort Port;
        public string IP;
        public ushort CharSlot;
        public override string ToString()
        {
            return Username;//base.ToString();
        }
    }
    public class Profiles
    {
       
        public HashSet<config> Profileslist = new HashSet<config>();

        public Profiles()
        {
            

        }

        public static void Serialize(string file, Profiles c)
        {
            System.Xml.Serialization.XmlSerializer xs
               = new System.Xml.Serialization.XmlSerializer(c.GetType());
            StreamWriter writer = File.CreateText(file);
            xs.Serialize(writer, c);
            writer.Flush();
            writer.Close();
        }
        public static Profiles Deserialize(string file)
        {
            System.Xml.Serialization.XmlSerializer xs
               = new System.Xml.Serialization.XmlSerializer(
                  typeof(Profiles));
            StreamReader reader = File.OpenText(file);
            Profiles c = (Profiles)xs.Deserialize(reader);
            reader.Close();
            return c;
        }

        public void Add(String user, String pass, String IP, String Port, String charslot)
        {
            config newprofile = new config();

            newprofile.Username = user;
            newprofile.Password = pass;
            newprofile.IP = IP;
            newprofile.Port = Convert.ToUInt16(Port);
            newprofile.CharSlot = Convert.ToUInt16(charslot);
            Profileslist.Add(newprofile);
        }
    }
}