using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAD_Weather
{
    public class PieceOfData
    {
        public string Outlook { get; set; }
        public string Temperature { get; set; }
        public string Humidity { get; set; }
        public string Windy { get; set; }
        public string Play { get; set; }

        public string[] Attributes { get; set; }

        public PieceOfData()
        {
            Attributes = new string[5];
            Outlook = "";
            Temperature = "";
            Humidity = "";
            Windy = "";
            Play = "";
        }

        public bool Compare(PieceOfData other)
        {
            if (Outlook == other.Outlook && Temperature == other.Temperature && Humidity == other.Humidity && Windy == other.Windy)
                return true;
            else return false;
        }

        public void Print()
        {
            Console.WriteLine("{0}, {1}, {2}, {3}, {4}", Outlook, Temperature, Humidity, Windy, Play);
        }

    }
}
