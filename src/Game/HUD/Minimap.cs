using System.Drawing;
using System.Numerics;
using Scorpia.Game.HUD.MinimapRenderers;
using Scorpia.Game.Nodes;
using Scorpian.Asset;
using Scorpian.Graphics;
using Scorpian.InputManagement;
using Scorpian.Maths;

namespace Scorpia.Game.HUD;

public class Minimap
{
    private readonly RenderTargetSprite _mapSprite;
    private readonly RenderContext _renderContext;
    private readonly MapNode _map;
    private readonly Size _size;

    private readonly Sprite _background;
    private readonly Vector2 _multiplier;
    private RectangleF _rect;

    private readonly MinimapRenderer[] _renderers;

    public Minimap(RenderContext renderContext, MapNode map, AssetManager assetManager, Size size)
    {
        _renderContext = renderContext;
        _map = map;
        _size = size;

        _mapSprite = _renderContext.CreateRenderTarget(size);
        _background = assetManager.Get<Sprite>("Game:HUD/minimap_background");

        _multiplier = new Vector2(_size.Width / (float) _map.Width, _size.Height / (float) _map.Height);

        _renderers = new MinimapRenderer[]
        {
            new TreeMinimapRenderer(),
            new MountainMinimapRenderer()
        };

        foreach (var renderer in _renderers)
        {
            renderer.Init(assetManager);
        }
    }

    public void Update()
    {
        if (!Input.IsButton(MouseButton.Left) || !_rect.Contains(Input.MousePosition))
        {
            return;
        }

        var pos = Input.MousePosition.ToVector() - _rect.Location.ToVector2();
        pos = pos / _rect.Size.ToVector2() * _map.WorldSize.ToVector2();

        _renderContext.Camera.LookAtAndClamp(pos, new RectangleF(new PointF(), _map.WorldSize));
    }

    private void RenderMinimap()
    {
        foreach (var renderer in _renderers)
        {
            foreach (var position in _map.Map)
            {
                var mapTile = _map.Map.GetData(position);

                var point = position.ToPoint().ToVector() * _multiplier;
                
                if (!renderer.ShouldRender(mapTile))
                {
                    continue;
                }

                var sprite = renderer.GetSprite(position);
                if (sprite is null)
                {
                    continue;
                }

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
        
        RenderMinimap();
        
        var relativePos = _renderContext.Camera.Position / _map.WorldSize.ToVector2() * _size.ToVector();
        var relativeSize = renderSize.ToVector2() / _map.WorldSize.ToVector2() * _size.ToVector();
        
        var viewRect = new RectangleF(relativePos.ToPointF(), relativeSize.ToSize());
        _renderContext.DrawRectangle(viewRect, Color.White, false);
        
        viewRect = new RectangleF(relativePos.ToPointF().Add(new PointF(1, 1)),
            relativeSize.ToSize() - new SizeF(2, 2));
        _renderContext.DrawRectangle(viewRect, Color.White, false);
        _mapSprite.EndDraw();
        
        _renderContext.Draw(_mapSprite, null, _rect, 0, Color.White, inWorld: false);
    }
}