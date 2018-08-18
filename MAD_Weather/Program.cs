using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAD_Weather
{
    class Program
    {

        public static void GenerateCombinations(List<PieceOfData> data)
        {
            #region COMBINATIONS
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        for (int h = 0; h < 3; h++)
                        {
                            var piece = new PieceOfData();

                            // Outlook
                            if (i == 0)
                            {
                                piece.Outlook = "sunny";
                            }
                            if (i == 1)
                            {
                                piece.Outlook = "rainy";
                            }
                            if (i == 2)
                            {
                                piece.Outlook = "overcast";
                            }
                            if (i == 3)
                            {
                                piece.Outlook = "null";
                            }

                            // Temperature
                            if (j == 0)
                            {
                                piece.Temperature = "hot";
                            }
                            if (j == 1)
                            {
                                piece.Temperature = "mild";
                            }
                            if (j == 2)
                            {
                                piece.Temperature = "cool";
                            }
                            if (j == 3)
                            {
                                piece.Temperature = "null";
                            }

                            // Humidity
                            if (k == 0)
                            {
                                piece.Humidity = "high";
                            }
                            if (k == 1)
                            {
                                piece.Humidity = "normal";
                            }
                            if (k == 2)
                            {
                                piece.Humidity = "null";
                            }

                            // Windy
                            if (h == 0)
                            {
                                piece.Windy = "TRUE";
                            }
                            if (h == 1)
                            {
                                piece.Windy = "FALSE";
                            }
                            if (h == 2)
                            {
                                piece.Windy = "null";
                            }

                            data.Add(piece);
                        }

                    }

                }

            }
            #endregion
            /*
                        var playing = new List<PieceOfData>();
                        var notPlaying = new List<PieceOfData>();
                        var unknown = new List<PieceOfData>();


                        foreach (var d in data)
                        {
                            // Playing Golf
                            if (d.Play == "yes")
                                playing.Add(d);
                            // Not Playing Golf
                            if (d.Play == "no")
                                notPlaying.Add(d);
                        }


                        var pattern = new List<PieceOfData>();

                        foreach (var a in allComps)
                        {
                            foreach (var s in playing)
                            {
                                if (s.Compare(a))
                                {
                                    a.Play = "yes";
                                    pattern.Add(a);
                                }

                            }

                            foreach (var s in notPlaying)
                            {
                                if (s.Compare(a))
                                {
                                    a.Play = "no";
                                    pattern.Add(a);
                                }

                            }

                        }



                        foreach (var p in pattern)
                        {
                            Console.WriteLine("{0}, {1}, {2}, {3}, {4}", p.Outlook, p.Temperature, p.Humidity, p.Windy, p.Play);
                        }*/

        }


        static void Main(string[] args)
        {
            #region DataTable
            DataTable dataTable = new DataTable("PlayGolf");

            dataTable.Columns.Add("Outlook");
            dataTable.Columns.Add("Temperature");
            dataTable.Columns.Add("Humidity");
            dataTable.Columns.Add("Wind");
            dataTable.Columns.Add("Play");

            dataTable.Rows.Add("Sunny", "Hot", "High", "Weak", "No");
            dataTable.Rows.Add("Sunny", "Hot", "High", "Strong", "No");
            dataTable.Rows.Add("Overcast", "Hot", "High", "Weak", "Yes");
            dataTable.Rows.Add("Rain", "Mild", "High", "Weak", "Yes");
            dataTable.Rows.Add("Rain", "Cool", "Normal", "Weak", "Yes");
            dataTable.Rows.Add("Rain", "Cool", "Normal", "Strong", "No");
            dataTable.Rows.Add("Overcast", "Cool", "Normal", "Strong", "Yes");
            dataTable.Rows.Add("Sunny", "Mild", "High", "Weak", "No");
            dataTable.Rows.Add("Sunny", "Cool", "Normal", "Weak", "Yes");
            dataTable.Rows.Add("Rain", "Mild", "Normal", "Weak", "Yes");
            dataTable.Rows.Add("Sunny", "Mild", "Normal", "Strong", "Yes");
            dataTable.Rows.Add("Overcast", "Mild", "High", "Strong", "Yes");
            dataTable.Rows.Add("Overcast", "Hot", "Normal", "Weak", "Yes");
            dataTable.Rows.Add("Rain", "Mild", "High", "Strong", "No");
            #endregion

            var data = new List<PieceOfData>();
            var allData = new List<PieceOfData>();

            int counter = 0;
            string line;
            string path = @"..\..\Data\data.txt";
            // Read the file and display it line by line.
            System.IO.StreamReader file =
               new System.IO.StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                var split = line.Split(',');
                var newData = new PieceOfData();

                for(int i=0; i < 5; i++)
                {

                    newData.Attributes[i] = split[i];
                }

                newData.Outlook = split[0];
                newData.Temperature = split[1];
                newData.Humidity = split[2];
                newData.Windy = split[3];
                newData.Play = split[4];

                data.Add(newData);
                counter++;
            }

            var dTree = new ID3(data, "Play");
            var result = "";
            dTree.Print(dTree, result);
            Console.ReadLine();
        }
    }
}
