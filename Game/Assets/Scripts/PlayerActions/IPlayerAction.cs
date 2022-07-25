using JetBrains.Annotations;
using Map;

namespace PlayerActions
{
    public interface IPlayerAction
    {
        [CanBeNull] string Description { get; }
        
        void LeftClick(MapTile mapTile);

        void RightClick(MapTile mapTile);

        void Hover(MapTile mapTile);
    }
}