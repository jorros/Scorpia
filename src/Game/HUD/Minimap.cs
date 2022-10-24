using System.Drawing;
using System.Numerics;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.HexMap;
using Scorpia.Engine.InputManagement;
using Scorpia.Engine.Maths;
using Scorpia.Game.Nodes;
using Scorpia.Game.World;

namespace Scorpia.Game.HUD;

public class Minimap
{
    private readonly DrawableSprite _mapSprite;
    private readonly RenderContext _renderContext;
    private readonly MapNode _map;
    private readonly Size _size;

    private readonly Sprite _background;
    private readonly Vector2 _multiplier;

    private readonly Sprite[] _treeSprites;
    private readonly Sprite[] _mountainSprites;
    private RectangleF _rect;

    public Minimap(RenderContext renderContext, MapNode map, AssetManager assetManager, Size size)
    {
        _renderContext = renderContext;
        _map = map;
        _size = size;

        _mapSprite = _renderContext.CreateDrawable(size);
        _background = assetManager.Get<Sprite>("Game:HUD/minimap_background");

        _multiplier = new Vector2(_size.Width / (float) _map.Width, _size.Height / (float) _map.Height);

        _treeSprites = LoadSprites(assetManager, "tree", 5);
        _mountainSprites = LoadSprites(assetManager, "mountain", 6);
    }

    public void Update()
    {
        if (!Input.IsButton(MouseButton.Left) || !_rect.Contains(Input.MousePosition))
        {
            return;
        }
        var pos = Input.MousePosition.ToVector() - _rect.Location.ToVector2();
        pos = pos / _rect.Size.ToVector2() * _map.WorldSize.ToVector2();
            
        _renderContext.Camera.LookAt(pos);
        _renderContext.Camera.Position = ClampCamera(_renderContext.Camera.Position);
    }
    
    private Vector2 ClampCamera(Vector2 position)
    {
        var screenBounds = _renderContext.Camera.BoundingRectangle;
        var size = _renderContext.Camera.GetSize(_map.WorldSize).ToSize();

        var start = new Vector2(0, 0);
        var end = new Vector2(size.Width - screenBounds.Width, size.Height - screenBounds.Height);

        var newX = Math.Clamp(position.X, start.X, end.X);
        var newY = Math.Clamp(position.Y, start.Y, end.Y);
        
        return new Vector2(newX, newY);
    }

    private static Sprite[] LoadSprites(AssetManager assetManager, string name, int count)
    {
        var sprites = new Sprite[count];
        foreach (var i in Enumerable.Range(1, count))
        {
            sprites[i - 1] = assetManager.Get<Sprite>($"Game:HUD/minimap_{name}_{i}");
        }

        return sprites;
    }

    private Sprite GetSprite(Hex position, IReadOnlyList<Sprite> sprites)
    {
        return sprites[Math.Abs(position.GetHashCode() % sprites.Count)];
    }

    private void RenderNaturalFeatures()
    {
        foreach (var position in _map.Map)
        {
            Sprite sprite;
            var mapTile = _map.Map.GetData(position);

            var point = position.ToPoint().ToVector() * _multiplier;

            if (mapTile.Features.Contains(MapTileFeature.Forest))
            {
                sprite = GetSprite(position, _treeSprites);
                
                _renderContext.Draw(sprite, null,
                    new RectangleF((point - new Vector2(sprite.Size.Width / 2f, sprite.Size.Height / 2f)).ToPoint(),
                        sprite.Size), 0,
                    Color.White, inWorld: false);
            }

            if (mapTile.Biome == Biome.Mountain)
            {
                sprite = GetSprite(position, _mountainSprites);
                
                _renderContext.Draw(sprite, null,
                    new RectangleF((point - new Vector2(sprite.Size.Width / 2f, sprite.Size.Height / 2f)).ToPoint(),
                        sprite.Size), 0,
                    Color.White, inWorld: false);
            }
        }
    }

    public void Render()
    {
        var renderSize = _renderContext.Camera.BoundingRectangle.Size;
        _rect = new RectangleF(_renderContext.DrawSize.Width - _size.Width, 60, _size.Width, _size.Height);
        
        _mapSprite.BeginDraw();
        _renderContext.Draw(_background, null, _rect with {X = 0, Y = 0}, 0, Color.White, inWorld: false);

        RenderNaturalFeatures();
        
        var relativePos = _renderContext.Camera.Position / _map.WorldSize.ToVector2() * _size.ToVector();
        var relativeSize = renderSize.ToVector2() / _map.WorldSize.ToVector2() * _size.ToVector();

        var viewRect = new RectangleF(relativePos.ToPointF(), relativeSize.ToSize());
        _renderContext.DrawRectangle(viewRect, Color.White, false);
        
        viewRect = new RectangleF(relativePos.ToPointF().Add(new PointF(1, 1)), relativeSize.ToSize() - new SizeF(2, 2));
        _renderContext.DrawRectangle(viewRect, Color.White, false);
        _mapSprite.EndDraw();

        _renderContext.Draw(_mapSprite, null, _rect, 0, Color.White, inWorld: false);
    }
}