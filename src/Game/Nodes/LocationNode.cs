using System.Drawing;
using Scorpia.Game.Blueprints;
using Scorpia.Game.Nodes.Entities;
using Scorpia.Game.World;
using Scorpian.Asset;
using Scorpian.Graphics;
using Scorpian.HexMap;
using Scorpian.Maths;
using Scorpian.Network.Protocol;
using Scorpian.SceneManagement;
using Scorpian.UI;

namespace Scorpia.Game.Nodes;

public partial class LocationNode : NetworkedNode
{
    public NetworkedVar<uint> Player { get; } = new();
    public NetworkedVar<byte> Type { get; } = new();
    public NetworkedVar<Hex> Position { get; } = new();
    public NetworkedVar<string> Name { get; } = new();
    public NetworkedVar<int> Population { get; } = new();
    public NetworkedVar<int> MaxPopulation { get; } = new();
    public NetworkedVar<BalanceSheet> FoodProduction { get; } = new();
    public NetworkedVar<float> FoodStorage { get; } = new();
    public NetworkedVar<BalanceSheet> Income { get; } = new();
    public NetworkedVar<int> InvestedConstruction { get; } = new();
    public NetworkedVar<bool> IsCapital { get; } = new();
    public NetworkedList<string> Tags { get; } = new();

    public NetworkedVar<short> CityDistricts { get; } = new();
    public NetworkedVar<short> FarmingDistricts { get; } = new();
    public NetworkedVar<short> MiningDistricts { get; } = new();
    public NetworkedVar<short> UniversityDistricts { get; } = new();
    public NetworkedVar<short> FortificationDistricts { get; } = new();

    public NetworkedList<Building> Buildings { get; } = new();

    public MapTile MapTile { get; private set; }

    private HorizontalGridLayout _title = null!;
    private MapNode _map = null!;
    private Label _nameLabel = null!;

    public override Task OnInit()
    {
        _map = Scene.FindNode<MapNode>();
        
        _map.Map.GetData(Position.Value).Location = this;

        if (!NetworkManager.IsClient)
        {
            return Task.CompletedTask;
        }
        
        _map.RefreshTile(Position.Value);
        var pos = _map.Map.HexToWorld(Position.Value);

        _title = new HorizontalGridLayout
        {
            Background = AssetManager.Get<Sprite>("Game:HUD/title"),
            SpaceBetween = 10,
            Position = pos,
            Height = 60,
            Padding = new Box(20, 15, 20, 15)
        };

        _nameLabel = new Label
        {
            Type = "title",
            Text = Name.Value,
        };
        _title.Attach(_nameLabel);

        Name.OnChange += (_, args) =>
        {
            _nameLabel.Text = args.NewValue;
        };

        return Task.CompletedTask;
    }

    public override async Task OnTick()
    {
        if (NetworkManager.IsClient)
        {
            return;
        }

        await ServerTick();
    }

    public int GetMaxFoodStorage()
    {
        return 100;
    }

    public Building? GetCurrentConstruction()
    {
        foreach (var building in Buildings)
        {
            if (building.IsBuilding)
            {
                return building;
            }
        }

        return null;
    }

    public Building? GetBuildingByFamily(BuildingType type)
    {
        foreach (var building in MapTile.Location.Buildings)
        {
            if (!BuildingBlueprints.IsSameFamily(building.Type, type))
            {
                continue;
            }

            return building;
        }

        return null;
    }

    public override void OnRender(RenderContext context, float dT)
    {
        var scaledWidth = ScorpiaStyle.Stylesheet!.Scale(_title.Width);
        var pos = _map.Map.HexToWorld(Position.Value);
        _title.Position = new PointF(pos.X - scaledWidth / 2f, pos.Y - 140);
        
        _title.Render(context, ScorpiaStyle.Stylesheet, true);
    }
}