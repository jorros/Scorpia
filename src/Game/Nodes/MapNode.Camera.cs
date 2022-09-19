using System.Numerics;
using Scorpia.Engine.InputManagement;
using Scorpia.Engine.Maths;
using Scorpia.Engine.SceneManagement;
using Scorpia.Game.World;

namespace Scorpia.Game.Nodes;

public class MapNodeCamera : Component
{
    private MapTile _tile = null!;
    private MapNode _map = null!;
    private MapTile _currentTile = null!;
    
    private Vector2? _dragOrigin;
    private bool _isDragging;
    
    public static bool clickIsBlocked;

    public override void OnInit()
    {
        _map = (MapNode)Parent;
    }

    public override void OnUpdate()
    {
        ProcessHover();
        
        StartDragging();
        
        StopDragging();
    }
    
    private void ProcessHover()
    {
        var mousePos = Camera.ScreenToWorld(Input.MousePosition);
        _tile = _map.GetTile(mousePos);

        if (_tile == _currentTile)
        {
            return;
        }
            
        _currentTile = _tile;
        // Game.GetPlayerAction().Hover(_currentTile);
    }
    
    private void SelectTile()
    {
        if (Input.IsButtonUp(MouseButton.Left) && !_isDragging && !clickIsBlocked)
        {
            var mousePos = Camera.ScreenToWorld(Input.MousePosition);
            _tile = _map.GetTile(mousePos);
            // Game.GetPlayerAction().LeftClick(tile);
        }

        if(Input.IsButtonUp(MouseButton.Right) && _tile is not null && !clickIsBlocked)
        {
            _tile = null;
            // Game.GetPlayerAction().RightClick(tile);
        }

        if (clickIsBlocked)
        {
            clickIsBlocked = false;
        }
    }
    
    private void StartDragging()
    {
        if(Input.IsButtonDown(MouseButton.Left))
        {
            _dragOrigin = Camera.ScreenToWorld(Input.MousePosition.ToVector());
        }

        if (!Input.IsButton(MouseButton.Left) || _dragOrigin == null)
        {
            return;
        }
        
        var delta = _dragOrigin.Value - Camera.ScreenToWorld(Input.MousePosition.ToVector());

        if (delta != Vector2.Zero)
        {
            _isDragging = true;
        }

        Camera.Position = ClampCamera(Camera.Position + delta);
    }
    
    private void StopDragging()
    {
        if (Input.IsButtonUp(MouseButton.Left) && _isDragging)
        {
            _isDragging = false;
        }
    }

    private Vector2 ClampCamera(Vector2 position)
    {
        var screenBounds = Camera.RenderContext.GetDrawSize();
        var size = _map.Map.GetSize();

        var start = new Vector2(0, 0);
        var end = new Vector2(size.Width - screenBounds.Width, size.Height - screenBounds.Height);
        
        var newX = Math.Clamp(position.X, start.X, end.X);
        var newY = Math.Clamp(position.Y, start.Y, end.Y);
        
        return new Vector2(newX, newY);
    }
}