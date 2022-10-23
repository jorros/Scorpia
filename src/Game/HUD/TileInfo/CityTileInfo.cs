using Scorpia.Game.HUD.Tooltip;
using Scorpia.Game.Nodes;
using Scorpia.Game.Nodes.Entities;
using Scorpia.Game.Player;
using Scorpia.Game.World;

namespace Scorpia.Game.HUD.TileInfo;

public class CityTileInfo : ITileInfo
{
    private readonly TileInfoNode _infoNode;

    public CityTileInfo(TileInfoNode infoNode)
    {
        _infoNode = infoNode;
    }

    public int WindowHeight => 575;

    public bool ShouldRender(MapTile tile)
    {
        return tile.Location is not null;
    }

    public void Init(MapTile tile)
    {
        var location = tile.Location;
        _infoNode.SetName(location.Name.Value);

        SetAvatar(location);
        AddPlayerIcon(location);
        AddResourceIcon(tile);
        AddFertilityIcon(tile);
        AddStats(location);
    }

    public void Update(MapTile tile)
    {
    }

    private void AddPlayerIcon(LocationNode location)
    {
        var player = _infoNode.CurrentPlayer?.GetPlayer(location.Player.Value);

        if (player is not null)
        {
            _infoNode.AddInfoIcon(((PlayerColor)player.Color.Value).ToString().ToLower(), TooltipDescription.Empty);
        }
    }

    private void SetAvatar(LocationNode location)
    {
        var i = location.Type.Value switch
        {
            LocationType.Village => "village",
            LocationType.Town => "town",
            LocationType.City => "city",
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
        // _infoNode.AddStat("population",
        //     $"{location.Population.Value.Format()} / {location.MaxPopulation.Value.Format()}",
        //     new TooltipDescription("Population",
        //         $"The current population. Living space can be increased by building houses or upgrading the {location.Type.Value} to a higher tier",
        //         string.Empty, TooltipPosition.Info));
        //
        // _infoNode.AddStat("balance", $"{location.Income.Value.Total.FormatBalance()}",
        //     new TooltipDescription("Income", $"Current monthly income generated.", string.Empty, TooltipPosition.Info));
        //
        // _infoNode.AddStat("food",
        //     $"{location.FoodStorage.Value.Format()} / {LocationBlueprint.GetMaxFoodStorage(location.Type.Value)} ({location.FoodProduction.Value.Total.FormatBalance()})",
        //     new TooltipDescription("Food",
        //         $"You are currently storing {location.FoodStorage.Value.Format()} and the monthly food production is at {location.FoodProduction.Value.Total.FormatBalance()}. You can increase the storage room by upgrading this {location.Type.Value}. All produced food that cannot stored here will be transferred to your global storage.",
        //         string.Empty, TooltipPosition.Info));
        //
        // _infoNode.AddStat("garrison", location.Garrison.Value.Format(),
        //     new TooltipDescription("Garrison",
        //         $"There are currently {location.Garrison.Value.Format()} troops stationed in this {location.Type.Value}",
        //         string.Empty, TooltipPosition.Info));
    }
}