namespace Map
{
    public class River
    {
        public Direction? From { get; set; }

        public Direction? To { get; set; }

        public bool IsDirection(Direction? a, Direction? b)
        {
            return (From == a && To == b) || (From == b && To == a);
        }
    }
}