namespace Scorpia.Assets.Scripts.Map.Render
{
	public class RandomIndex
	{
        private readonly int max;
        private int current = 0;

        public RandomIndex(int max)
        {
            this.max = max;
        }

        public int Next(int from)
        {
            if (current < max)
            {
                return current++;
            }

            current = 0;
            return from + current;
        }
    }
}

