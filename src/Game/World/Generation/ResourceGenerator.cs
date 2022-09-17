using Scorpia.Game.Nodes;
using Scorpia.Game.Utils;

namespace Scorpia.Game.World.Generation;

public class ResourceGenerator : IGenerator
{
    private const int GoldSpawnChance = 6;
    private const int SofrumSpawnChance = 4;
    private const int NitraSpawnChance = 1;
    private const int ZellosSpawnChance = 4;
    
    public void Generate(MapNode map, NoiseMap noiseMap)
    {
        foreach (var tile in map.Tiles)
        {
            if (tile.Biome != Biome.Grass)
            {
                continue;
            }

            if(CheckGold(tile, map))
            {
                continue;
            }

            if (CheckZellos(tile, map))
            {
                continue;
            }

            if (CheckSofrum(tile, map))
            {
                continue;
            }

            if(CheckNitra(tile, map))
            {
                continue;
            }
        }
    }
    
    private bool HasChance(MapNode map, int chance)
    {
        return map.Rnd.Next(100) <= chance;
    }

    private bool CheckGold(MapTile tile, MapNode map)
    {
        if (HasChance(map, GoldSpawnChance) && map.HasNeighbour(tile, x => x.Biome == Biome.Mountain, 2))
        {
            tile.Resource = Resource.Gold;

            return true;
        }

        return false;
    }

    private bool CheckSofrum(MapTile tile, MapNode map)
    {
        if (HasChance(map, SofrumSpawnChance) && tile.HasFeature(MapTileFeature.Forest))
        {
            tile.Resource = Resource.Sofrum;

            return true;
        }

        return false;
    }

    private bool CheckNitra(MapTile tile, MapNode map)
    {
        if (HasChance(map, NitraSpawnChance))
        {
            tile.Resource = Resource.Nitra;

            return true;
        }

        return false;
    }

    private bool CheckZellos(MapTile tile, MapNode map)
    {
        if (HasChance(map, ZellosSpawnChance) && map.HasNeighbour(tile, x => x.Biome == Biome.Water || x.River != null, 4))
        {
            tile.Resource = Resource.Zellos;

            return true;
        }

        return false;
    }
}