using Scorpia.Game.HUD.Tooltip;
using Scorpia.Game.World;

namespace Scorpia.Game.HUD.TileInfo;

public class EmptyTileInfo : ITileInfo
{
    private readonly TileInfoNode _infoNode;

    public EmptyTileInfo(TileInfoNode infoNode)
    {
        _infoNode = infoNode;
    }

    public int WindowHeight => 575;

    public bool ShouldRender(MapTile tile)
    {
        return tile.Location is null;
    }

    public void Init(MapTile tile)
    {
        _infoNode.SetName("Empty");

        SetAvatar(tile);
        AddResourceIcon(tile);
        AddFertilityIcon(tile);
    }

    public void Update(MapTile tile)
    {
    }

    private void SetAvatar(MapTile tile)
    {
        var i = tile switch
        {
            {Biome: Biome.Water} => "water",
            {Biome: Biome.Grass} when tile.HasFeature(MapTileFeature.Forest) => "forest",
            {Biome: Biome.Grass} => "grass",
            {Biome: Biome.Mountain} => "mountain",
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

        if (!string.IsNullOrEmpty(i.Item1))
        {
            _infoNode.AddInfoIcon(i.Item1, new TooltipDescription(i.Item2, string.Empty, i.Item3, TooltipPosition.Info));
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
            _infoNode.AddInfoIcon(i.Item1, new TooltipDescription(i.Item2, string.Empty, i.Item3, TooltipPosition.Info));
        }
    }
}