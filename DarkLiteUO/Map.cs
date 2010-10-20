using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Ultima;
namespace DarkLiteUO
{
    
    public class Map
    {
        public ushort[,] _Map = new ushort[6145, 4097];
        public HashSet<config> Profileslist = new HashSet<config>();

        public Map()
        {
            

        }

        public static void Serialize(string file, Map c)
        {
            System.Xml.Serialization.XmlSerializer xs
               = new System.Xml.Serialization.XmlSerializer(c.GetType());
            StreamWriter writer = File.CreateText(file);
            xs.Serialize(writer, c);
            writer.Flush();
            writer.Close();
        }
        public static Map Deserialize(string file)
        {
            System.Xml.Serialization.XmlSerializer xs
               = new System.Xml.Serialization.XmlSerializer(
                  typeof(Profiles));
            StreamReader reader = File.OpenText(file);
            Map c = (Map)xs.Deserialize(reader);
            reader.Close();
            return c;
        }
        public static Dictionary<String,ushort> Get32x32(int startx, int starty, int endx, int endy)
    {
        Dictionary<String, ushort> _map = new Dictionary<String,ushort>();
        for (int x = startx; x <= endx; x++)
        {
            for (int y = starty; y <= endy; y++)
            {
                Tile temptile = Ultima.Map.Felucca.Tiles.GetLandTile(x, y);
                _map.Add(x.ToString() + "_" + y.ToString(), (ushort)temptile.ID);
            }
        }
        return _map;


    }
        public static Dictionary<int, ushort> GetTiles(int range, UOLite2.LiteClient client)
        {
            Dictionary<int, ushort> _map = new Dictionary<int, ushort>();
            for (int x = client.Player.X - range; x <= client.Player.X + range; x++)
            {
                for (int y = client.Player.Y - range; y <= client.Player.X + range; y++)
                {
                    Tile temptile = Ultima.Map.Felucca.Tiles.GetLandTile(x, y);
                    _map.Add(x * y, (ushort)temptile.ID);
                }
            }
            return _map;


        }
        public void Add()
        {
            for(int x = 0; x <= Ultima.Map.Felucca.Width;x++)
            {
                for(int y = 0; y <= Ultima.Map.Felucca.Height;y++)
                {
                Tile temptile = Ultima.Map.Felucca.Tiles.GetLandTile(x,y);
                    _Map[x,y] = (ushort)temptile.ID;
                }
            }
            



        }
    }
}