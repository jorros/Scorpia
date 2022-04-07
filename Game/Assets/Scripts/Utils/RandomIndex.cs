namespace Utils
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
                return from + current++;
            }

            current = 0;
            return from + current;
        }
    }
}

