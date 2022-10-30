using System.Numerics;
using Microsoft.Extensions.DependencyInjection;
using Scorpia.Engine.InputManagement;
using Scorpia.Engine.Maths;
using Scorpia.Engine.SceneManagement;
using Scorpia.Game.Player;
using Scorpia.Game.Scenes;
using Scorpia.Game.World;

namespace Scorpia.Game.Nodes;

public class MapNodeCamera : Component
{
    private MapTile? _tile;
    private MapNode _map = null!;
    private MapTile _currentTile = null!;
    
    private Vector2? _dragOrigin;
    private bool _isDragging;
    private CurrentPlayer _currentPlayer = null!;

    public static bool clickIsBlocked;

    public override void OnInit()
    {
        _map = (MapNode)Parent;
        _currentPlayer = ServiceProvider.GetRequiredService<CurrentPlayer>();
    }

    public override void OnUpdate()
    {
        if (IsOverUI())
        {
            return;
        }
        
        ProcessHover();
        
        StartDragging();
        SelectTile();
        StopDragging();
    }

    private bool IsOverUI()
    {
        if (SceneManager.GetCurrentScene() is not GameScene scene)
        {
            return false;
        }
        
        if (scene.topContainer.Boundaries.Contains(Input.MousePosition))
        {
            return true;
        }

        if (scene.mapWindow.Boundaries.Contains(Input.MousePosition))
        {
            return true;
        }

        if (scene.menuButtons.Boundaries.Contains(Input.MousePosition))
        {
            return true;
        }

        if (scene.infoWindow.Show && scene.infoWindow.Boundaries.Contains(Input.MousePosition))
        {
            return true;
        }

        return scene.notificationWindows.Any(ntfWindow => ntfWindow.Boundaries.Contains(Input.MousePosition));
    }
    
    private void ProcessHover()
    {
        var mousePos = Camera.ScreenToWorld(Input.MousePosition);
        var hexPos = _map.Map.WorldToHex(mousePos);

        if (!_map.Map.Contains(hexPos))
        {
            return;
        }
        
        _tile = _map.Map.GetData(hexPos);

        if (_tile == _currentTile)
        {
            return;
        }
            
        _currentTile = _tile;
        _currentPlayer.CurrentAction.Hover(_currentTile);
    }
    
    private void SelectTile()
    {
        if (Input.IsButtonUp(MouseButton.Left) && !_isDragging && !clickIsBlocked)
        {
            var mousePos = Camera.ScreenToWorld(Input.MousePosition);
            var hexPos = _map.Map.WorldToHex(mousePos);
            
            if (!_map.Map.Contains(hexPos))
            {
                return;
            }
            
            _tile = _map.Map.GetData(hexPos);
            _currentPlayer.CurrentAction.LeftClick(_tile);
        }

        if(Input.IsButtonUp(MouseButton.Right) && _tile is not null && !clickIsBlocked)
        {
            _tile = null;
            _currentPlayer.CurrentAction.RightClick(_tile);
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
            Console.WriteLine("Start dragging");
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
        var screenBounds = Camera.BoundingRectangle;
        var size = Camera.GetSize(_map.WorldSize).ToSize();

        var start = new Vector2(0, 0);
        var end = new Vector2(size.Width - screenBounds.Width, size.Height - screenBounds.Height);

        var newX = Math.Clamp(position.X, start.X, end.X);
        var newY = Math.Clamp(position.Y, start.Y, end.Y);
        
        return new Vector2(newX, newY);
    }
}