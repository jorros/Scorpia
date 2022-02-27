using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scorpia.Assets.Scripts.Utils
{
    public class RandomList<T>
    {
        private readonly IEnumerable<(T value, int weight)> list;
        private readonly int maxWeight;
        private readonly System.Random rnd;

        public RandomList(System.Random rnd, IEnumerable<(T value, int weight)> list)
        {
            this.rnd = rnd;
            this.list = list;
            maxWeight = list.Sum(x => x.weight);
        }

        public T Sample()
        {
            var randomNum = rnd.Next(maxWeight);
            var currentWeightSum = 0;

            foreach (var item in list)
            {
                if (randomNum >= currentWeightSum && randomNum < (currentWeightSum + item.weight))
                {
                    return item.value;
                }
                currentWeightSum += item.weight;
            }

            return default(T);
        }
    }
}