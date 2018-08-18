using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAD_Weather
{
    public class ID3
    {
        public string Name { get; set; }
        public ID3 Parent { get; set; }                    // Null if root
        public int Index { get; set; }
        public List<ID3> Children { get; set; }
        public string Target { get; set; }
        public string Best { get; set; }
        public string Value { get; set; }
        public bool IsAttribute { get; set; }
        public bool IsValue { get; set; }
        public string Label { get; set; }

        public List<PieceOfData> TrainData { get; set; }

        private int GetValueCount(List<PieceOfData> data, string att, string value)
        {
            return data.Select(x => x.Attributes[GetIndex(att)]).Where(x => x == value).Count();
        }


        private decimal Gain(List<PieceOfData> data, string name, string target)
        {
            decimal subsetEntropy = 0;

            var values = data.Select(x => x.Attributes[GetIndex(name)]).Distinct().ToArray();
            var freq = new Dictionary<string, decimal>();

            foreach (var val in values)
            {
                freq.Add(val, GetValueCount(data, name, val));
            }


            foreach (var val in freq)
            {
                decimal valProb = val.Value / freq.Sum(x => x.Value);
                var dataSubset = data.Where(x => x.Attributes[GetIndex(name)] == val.Key).ToList();
                var ent = Entropy(dataSubset, target);
                subsetEntropy += valProb * Entropy(dataSubset, target);
            }

            return (Entropy(data, target) - subsetEntropy);
        }


        private decimal Entropy(List<PieceOfData> data, string name)
        {
            var values = data.Select(x => x.Attributes[GetIndex(name)]).Distinct().ToArray();
            var freq = new Dictionary<string, double>();

            foreach (var val in values)
            {
                freq.Add(val, GetValueCount(data, name, val));
            }


            double totalCount = freq.Sum(x => x.Value);

            var valRatios = new Dictionary<string, double>();

            foreach (var val in values)
            {
                double f = 0;
                freq.TryGetValue(val, out f);
                valRatios.Add(val, f / totalCount);
            }

            double result = 0;
            foreach (var val in valRatios)
            {
                if (val.Value != 0)
                    result += -(val.Value) * Math.Log(val.Value, 2);
            }


            return (decimal)result;
        }

        public ID3(List<PieceOfData> data, string target)
        {
            TrainData = data;
            Target = target;
            Children = new List<ID3>();
            Split(TrainData, target);
        }

        public ID3(string name, ID3 parent)
        {
            Name = name;
            Parent = parent;
            Children = new List<ID3>();
        }

        public ID3(string val, ID3 parent, List<PieceOfData> data)
        {
            Value = val;
            Parent = parent;
            TrainData = data;
            Children = new List<ID3>();
        }

        public int GetIndex(string name)
        {
            switch (name)
            {
                case "Outlook":
                    return 0; ;
                case "Temperature":
                    return 1;
                case "Humidity":
                    return 2;
                case "Windy":
                    return 3;
                case "Play":
                    return 4;
                default:
                    return -1;
            }
        }

        public string GetName(int index)
        {
            switch (index)
            {
                case 0:
                    return "Outlook";
                case 1:
                    return "Temperature";
                case 2:
                    return "Humidity";
                case 3:
                    return "Windy";
                case 4:
                    return "Play";
                default:
                    return null;
            }
        }

        private string GetBest(List<PieceOfData> data, string target)
        {
            decimal maxG = 0;
            string result = "";

            for (int i = 0; i < data.First().Attributes.Length; i++)
            {
                var name = GetName(i);
                if (name == target)
                    continue;
                decimal gain = Gain(data, name, target);
                if (gain > maxG)
                {
                    maxG = gain;
                    result = name;
                }

            }

            return result;
        }

        public void Split(List<PieceOfData> data, string target)
        {

            Best = GetBest(data, target);

            var possibleValues = TrainData.Select(x => x.Attributes[GetIndex(Best)]).Distinct().ToArray();

            ID3 parentNode = null;

            if (Parent != null)
            {
                parentNode = new ID3(Best, this);
                parentNode.Best = Best;
                parentNode.IsAttribute = true;
                parentNode.IsValue = false;
                Children.Add(parentNode);
            }
            else
            {
                parentNode = this;
                IsAttribute = true;
            }


            foreach (var v in possibleValues)
            {
                var child = new ID3(v, parentNode, TrainData);
                child.IsValue = true;
                child.IsAttribute = false;
                parentNode.Children.Add(child);
                child.Eval(data, target, Best);

            }

            return;
        }

        public void Eval(List<PieceOfData> data, string target, string best)
        {
            var subset = data.Where(x => x.Attributes[GetIndex(best)] == Value).ToList();

            bool isPure = false;
            bool playing = false;
            bool notPlaying = false;

            foreach (var s in subset)
            {
                if (s.Attributes[GetIndex(target)] == "yes")
                    playing = true;

                if (s.Attributes[GetIndex(target)] == "no")
                    notPlaying = true;

            }

            if ((playing && !notPlaying)) {
                isPure = true;
                Label = "yes";
            }
            
            if(!playing && notPlaying){
                isPure = true;
                Label = "no";
            }

            if (isPure)
                return;
            else if (subset.Any())
            {
                Split(subset, target);
            }
            else
                return;


        }

        public void Print(ID3 node, string result)
        {
            if (node.Parent == null)
            {
                result += "If " +node.Best;
            }

            if (node.Children == null || node.Children.Count == 0)
            {
                Console.WriteLine(result += ", THEN Play == " + node.Label + ".");
            }

            foreach (var child in node.Children)
            {
                var oldResult = result;
                if (child.IsAttribute)
                {
                    result += " AND " + child.Best;
                }
                if (child.IsValue)
                {
                   result += " == " + child.Value;
                }
                 
                Print(child, result);
                result = oldResult;
                
            }
        }


    }
}
