using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAD_Weather
{
    public class Classifier
    {

        public List<PieceOfData> Playing { get; set; }
        public List<PieceOfData> NotPlaying { get; set; }
        public List<PieceOfData> Unknown { get; set; }

        public Classifier()
        {
            Playing = new List<PieceOfData>();
            NotPlaying = new List<PieceOfData>();
            Unknown = new List<PieceOfData>();
        }
            

        public void Train(List<PieceOfData> pattern)
        {
            Playing.AddRange(pattern.Where(item => item.Play == "yes").ToList());
            NotPlaying.AddRange(pattern.Where(item => item.Play == "no").ToList());
            Unknown.AddRange(pattern.Where(item => item.Play != "no" || item.Play != "yes").ToList());

        }

        public List<PieceOfData> Classify(List<PieceOfData> data)
        {

            foreach(var d in data)
            {
                var playing = false;
                var notPlaying = false;

                if (Playing.Any(item => item.Compare(d)))
                {
                    playing = true;
                }
                else if (NotPlaying.Any(item => item.Compare(d)))
                {
                    notPlaying = true;
                }

                if((playing && notPlaying) || (!playing && !notPlaying))
                {
                    d.Play = "unknown";
                }
                else if (playing)
                {
                    d.Play = "yes";
                }
                else if (notPlaying)
                {
                    d.Play = "no";
                }
            }
            

            return data;
        }



    }
}
