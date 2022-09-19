namespace Scorpia.Game.Utils;

public class IndexHelper
{
    private readonly int _max;
    private int _current;

    public IndexHelper(int max)
    {
        _max = max;
    }

    public int Next()
    {
        if (_current < _max)
        {
            return _current++;
        }

        _current = 0;
        return _current;
    }
}