using System.Collections.Generic;
using System.Linq;

namespace Scorpia.Assets.Scripts.Utils
{
    public static class RandomHelper
    {
        public class WeightedValue
        {
            public WeightedValue(int value, int weight)
            {
                Value = value;
                Weight = weight;
            }

            public int Value { get; protected set; }

            public int Weight { get; protected set; }
        }

        public static int NextWithWeight(this System.Random rnd, int maxValue, IReadOnlyList<WeightedValue> weights)
        {
            var weightSum = maxValue + weights.Select(x => x.Weight - 1).Sum();
            var randomNum = rnd.Next(weightSum);

            var currentWeightSum = 0;
            for(var i = 0; i < maxValue; i++)
            {
                if(randomNum < currentWeightSum)
                {
                    currentWeightSum += weights.FirstOrDefault((x) => x.Value == i).Weight;
                }
                else
                {
                    return i;
                }
            }

            return -1;
        }
    }
}