using System.Drawing;
using Scorpia.Game.HUD.Tooltip;
using Scorpia.Game.Nodes;
using Scorpia.Game.Nodes.Entities;
using Scorpia.Game.Player;
using Scorpia.Game.World;
using Scorpian.Asset;
using Scorpian.InputManagement;
using Scorpian.UI;

namespace Scorpia.Game.HUD.TileInfo;

public class CityTileInfo : ITileInfo
{
    private readonly TileInfoNode _infoNode;
    private readonly AssetManager _assetManager;

    public CityTileInfo(TileInfoNode infoNode, AssetManager assetManager)
    {
        _infoNode = infoNode;
        _assetManager = assetManager;
    }

    public int WindowHeight => 700;

    public bool ShouldRender(MapTile tile)
    {
        return tile.Location is not null;
    }

    public void Init(MapTile tile)
    {
        _nextY = 130;
        _districtX = 155;

        var location = tile.Location;
        _infoNode.SetName(location.Name.Value);

        SetAvatar(location);
        AddPlayerIcon(location);
        AddResourceIcon(tile);
        AddFertilityIcon(tile);
        AddStats(location);
        AddActionButtons(location);
        AddDistrictButtons(location);
        AddBuildingButtons(location);
        _foodStorage = AddFoodStorage(location);
    }

    public void Update(MapTile tile)
    {
        _farmingDistrict.Text = tile.Location!.FarmingDistricts.Value.ToString();
        _miningDistrict.Text = tile.Location.MiningDistricts.Value.ToString();
        _universityDistrict.Text = tile.Location.UniversityDistricts.Value.ToString();
        _fortificationDistrict.Text = tile.Location.FortificationDistricts.Value.ToString();

        _foodStorage.Progress =
            (byte) Math.Floor(tile.Location.FoodStorage.Value / tile.Location.GetMaxFoodStorage() * 100);
    }

    private void AddActionButtons(LocationNode location)
    {
        var pos = 0;
        AddActionButton(pos, "road",
            new TooltipDescription("Build road", "Connect roads to other cities.", TooltipPosition.Info),
            () => { });
        pos++;

        AddActionButton(pos, "build",
            new TooltipDescription("Build city district", "Extend your city by building additional city districts.",
                TooltipPosition.Info), () => { });
        pos++;

        AddActionButton(pos, "attack", new TooltipDescription("Attack foreign army",
            "Attacks enemy troops close to the city.",
            TooltipPosition.Info), () => { }, true);
        pos++;

        if (location.Type.Value == (byte) LocationType.Village)
        {
            AddActionButton(pos, "upgrade",
                new TooltipDescription("Upgrade to city", "Upgrade your village to a city, so you can build districts.",
                    TooltipPosition.Info),
                () => { });
            pos++;
        }
    }

    private void AddActionButton(int i, string icon, TooltipDescription description, Action action,
        bool disabled = false)
    {
        var button = new Button
        {
            Type = "action",
            Content = new Image
            {
                Sprite = _assetManager.Get<Sprite>($"Game:HUD/info_action_{icon}")
            },
            Position = new PointF(12, i * 123),
            Enabled = !disabled
        };
        var element = new TooltippedElement<Button>(button, _assetManager)
        {
            Description = description
        };
        button.OnClick += (_, args) =>
        {
            if (args.Button == MouseButton.Left)
            {
                action();
            }
        };
        _infoNode.Window.Attach(element);
    }

    private void AddDistrictButtons(LocationNode location)
    {
        _farmingDistrict = AddDistrictButton("farming", location.FarmingDistricts.Value.ToString(),
            new TooltipDescription("Build farming district", "Farming blabla", TooltipPosition.Info), () => { });

        _miningDistrict = AddDistrictButton("mining", location.MiningDistricts.Value.ToString(),
            new TooltipDescription("Build mining district", "Mining blabla", TooltipPosition.Info), () => { });

        _universityDistrict = AddDistrictButton("research", location.UniversityDistricts.Value.ToString(),
            new TooltipDescription("Build university district", "Universit blabla", TooltipPosition.Info), () => { });

        _fortificationDistrict = AddDistrictButton("fortification", location.FortificationDistricts.Value.ToString(),
            new TooltipDescription("Build fortification district", "Fortification blabla", TooltipPosition.Info),
            () => { });
    }

    private Label AddDistrictButton(string icon, string level, TooltipDescription description, Action action)
    {
        var content = new HorizontalGridLayout();
        content.Attach(new Image
        {
            Sprite = _assetManager.Get<Sprite>($"Game:HUD/info_district_{icon}")
        });
        var label = new Label
        {
            Type = "district",
            Text = level,
            Margin = new Point(30, 0)
        };
        content.Attach(label);

        var button = new Button
        {
            Type = "district",
            Content = content,
            Position = new PointF(_districtX, 0)
        };

        button.OnClick += (_, args) =>
        {
            if (args.Button == MouseButton.Left)
            {
                action();
            }
        };

        var element = new TooltippedElement<Button>(button, _assetManager)
        {
            Description = description
        };
        _infoNode.Window.Attach(element);

        _districtX += 190;

        return label;
    }

    private int _districtX = 155;

    private void AddBuildingButtons(LocationNode location)
    {
        for (var i = 0; i < 4; i++)
        {
            var button = new Button
            {
                Type = "building",
                Position = new PointF(6, 15 + 120 * i),
                Anchor = UIAnchor.TopRight,
                Content = new Image
                {
                    Sprite = _assetManager.Get<Sprite>("Game:HUD/info_building_add")
                }
            };
            _infoNode.Window.Attach(button);
        }
    }

    private ProgressBar AddFoodStorage(LocationNode location)
    {
        var header = new Label
        {
            Type = "info_label",
            Anchor = UIAnchor.BottomRight,
            TextAlign = Alignment.Center,
            Position = new PointF(220, 380),
            Text = "Food"
        };
        _infoNode.Window.Attach(header);

        var storage = new ProgressBar
        {
            Type = "storage",
            Anchor = UIAnchor.BottomRight,
            Progress = (byte) Math.Floor(location.FoodStorage.Value / location.GetMaxFoodStorage() * 100),
            Position = new PointF(160, 80)
        };
        _infoNode.Window.Attach(storage);

        var label = new Label
        {
            Type = "info",
            Anchor = UIAnchor.BottomRight,
            TextAlign = Alignment.Center,
            Position = new PointF(135, 130),
            Text = $"{Math.Floor(location.FoodStorage.Value)}/{location.GetMaxFoodStorage()}"
        };
        _infoNode.Window.Attach(label);

        return storage;
    }

    private void AddPlayerIcon(LocationNode location)
    {
        var player = _infoNode.CurrentPlayer?.GetPlayer(location.Player.Value);

        if (player is not null)
        {
            _infoNode.AddInfoIcon(((PlayerColor) player.Color.Value).ToString().ToLower(), TooltipDescription.Empty);
        }
    }

    private void SetAvatar(LocationNode location)
    {
        var i = location.Type.Value switch
        {
            (byte) LocationType.Village => "village",
            (byte) LocationType.City => "city",
            (byte) LocationType.Farmland => "farmland",
            (byte) LocationType.Fortification => "fortification",
            (byte) LocationType.Mine => "mine",
            (byte) LocationType.University => "university",
            (byte) LocationType.Outpost => "outpost",
            _ => null
        };

        if (i is not null)
        {
            _infoNode.SetAvatarIcon(i);
        }
    }

    private void AddResourceIcon(MapTile tile)
    {
        var i = tile.Resource switch
        {
            Resource.Sofrum => ("sofrum", "Sofrum", "Can produce sofrum here"),
            Resource.Gold => ("gold", "Gold", "Can mine gold here"),
            Resource.Zellos => ("zellos", "Zellos", "Can extract zellos here"),
            Resource.Nitra => ("nitra", "Nitra", "Can mine nitra here"),
            _ => (null, "Error", "")
        };

        if (i.Item1 is not null)
        {
            _infoNode.AddInfoIcon(i.Item1,
                new TooltipDescription(i.Item2, string.Empty, i.Item3, TooltipPosition.Info));
        }
    }

    private void AddFertilityIcon(MapTile tile)
    {
        var i = tile.Fertility switch
        {
            Fertility.Low => ("barren", "Barren", "Barely able to produce food here"),
            Fertility.High => ("fertile", "Fertile ground", "Can produce more food here"),
            _ => (null, "Error", "")
        };

        if (i.Item1 is not null)
        {
            _infoNode.AddInfoIcon(i.Item1,
                new TooltipDescription(i.Item2, string.Empty, i.Item3, TooltipPosition.Info));
        }
    }

    private void AddStats(LocationNode location)
    {
        AddStatLabel("Population", location.Population.Value.ToString());
        AddStatLabel("Garrison", $"0 regiments");
        
        AddStatLabel(string.Empty, string.Empty);
        
        AddStatLabel("Taxes", location.Income.Value.Total.FormatBalance());
        AddStatLabel("Research", "0");
        AddStatLabel("Food", location.FoodProduction.Value.Total.FormatBalance());
        AddStatLabel("Mining", "0");
    }

    private void AddStatLabel(string label, string value)
    {
        var l1 = new Label
        {
            Type = "info",
            Position = new PointF(160, _nextY),
            Text = label
        };
        _infoNode.Window.Attach(l1);

        var l2 = new Label
        {
            Type = "info",
            Position = new PointF(400, _nextY),
            Text = value
        };
        _infoNode.Window.Attach(l2);

        _nextY += (int) Math.Round(30 * 1.4);
    }

    private int _nextY = 130;
    private Label _farmingDistrict;
    private Label _miningDistrict;
    private Label _universityDistrict;
    private Label _fortificationDistrict;
    private ProgressBar _foodStorage;
}