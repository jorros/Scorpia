namespace Scorpia.Assets.Scripts.Map.Render
{
	public class RiverIndexSelector
	{
        private readonly RandomIndex index2;
        private readonly RandomIndex index3;

        protected RiverIndexSelector()
        {
            index2 = new RandomIndex(2);
            index3 = new RandomIndex(3);
        }

		protected int GetIndex(MapTile tile)
        {
            var river = tile.River;
            if (river.IsDirection(Direction.East, null))
            {
                // 0, 1
                return index2.Next(0);
            }
            else if (river.IsDirection(Direction.NorthEast, null))
            {
                // 2, 3, 4
                return index3.Next(2);
            }
            else if (river.IsDirection(Direction.NorthWest, null))
            {
                // 5, 6, 7
                return index3.Next(5);
            }
            else if (river.IsDirection(Direction.SouthEast, null))
            {
                // 8, 9, 10
                return index3.Next(8);
            }
            else if (river.IsDirection(Direction.SouthWest, null))
            {
                // 11, 12, 13
                return index3.Next(11);
            }
            else if (river.IsDirection(Direction.West, null))
            {
                // 14, 15
                return index2.Next(14);
            }
            else if (river.IsDirection(Direction.East, Direction.SouthEast))
            {
                // 16, 17
                return index2.Next(16);
            }
            else if (river.IsDirection(Direction.East, Direction.SouthWest))
            {
                // 18, 19
                return index2.Next(18);
            }
            else if (river.IsDirection(Direction.East, Direction.West))
            {
                // 20, 21, 22
                return index3.Next(20);
            }
            else if (river.IsDirection(Direction.NorthEast, Direction.East))
            {
                // 23, 24, 25
                return index3.Next(23);
            }
            else if (river.IsDirection(Direction.NorthEast, Direction.SouthEast))
            {
                // 26, 27, 28
                return index3.Next(26);
            }
            else if (river.IsDirection(Direction.NorthEast, Direction.SouthWest))
            {
                // 29, 30, 31
                return index3.Next(29);
            }
            else if (river.IsDirection(Direction.NorthEast, Direction.West))
            {
                // 32, 33, 34
                return index3.Next(32);
            }
            else if (river.IsDirection(Direction.NorthWest, Direction.East))
            {
                // 35, 36, 37
                return index3.Next(35);
            }
            else if (river.IsDirection(Direction.NorthWest, Direction.NorthEast))
            {
                // 38, 39
                return index2.Next(38);
            }
            else if (river.IsDirection(Direction.NorthWest, Direction.SouthEast))
            {
                // 40, 41, 42
                return index3.Next(40);
            }
            else if (river.IsDirection(Direction.NorthWest, Direction.SouthWest))
            {
                // 43, 44, 45
                return index3.Next(43);
            }
            else if (river.IsDirection(Direction.NorthWest, Direction.West))
            {
                // 46, 47, 48
                return index3.Next(46);
            }
            else if (river.IsDirection(Direction.SouthEast, Direction.SouthWest))
            {
                // 49, 50
                return index2.Next(49);
            }
            else if (river.IsDirection(Direction.SouthEast, Direction.West))
            {
                // 51, 52
                return index2.Next(51);
            }
            else if (river.IsDirection(Direction.SouthWest, Direction.West))
            {
                // 53, 54
                return index2.Next(53);
            }

            return 0;
        }
    }
}

