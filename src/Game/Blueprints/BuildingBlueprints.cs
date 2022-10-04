using Scorpia.Game.Blueprints.Blueprints;
using Scorpia.Game.Blueprints.Requirements;
using Scorpia.Game.Nodes;
using Scorpia.Game.Nodes.Entities;
using Scorpia.Game.World;

namespace Scorpia.Game.Blueprints;

public static class BuildingBlueprints
{
    private static readonly IReadOnlyList<IBuildingBlueprint> Blueprints = new IBuildingBlueprint[]
    {
        new AcademyBuildingBlueprint(),
        new BarracksBuildingBlueprint(),
        new BunkerBuildingBlueprint(),
        new DeepMineBuildingBlueprint(),
        new EstateBuildingBlueprint(),
        new FarmBuildingBlueprint(),
        new FortsBuildingBlueprint(),
        new MineBuildingBlueprint(),
        new MintBuildingBlueprint(),
        new RefineryBuildingBlueprint(),
        new RefineryComplexBuildingBlueprint(),
        new ResidenceBuildingBlueprint(),
        new SchoolBuildingBlueprint(),
        new SmallholdingBuildingBlueprint(),
        new UniversityBuildingBlueprint()
    };

    private static IBuildingBlueprint GetBlueprint(BuildingType type) => Blueprints.First(x => x.Type == type);

    public static int GetConstructionCost(BuildingType type)
    {
        return GetBlueprint(type).ConstructionCost;
    }

    public static string GetName(BuildingType type)
    {
        return GetBlueprint(type).Name;
    }

    public static string GetDescription(BuildingType type)
    {
        return GetBlueprint(type).Description;
    }

    public static BuildingType? GetUpgrade(BuildingType type)
    {
        return GetBlueprint(type).Upgrade;
    }

    public static BuildingType? GetDowngrade(BuildingType type)
    {
        var blueprint = GetBlueprint(type);

        foreach (var other in Blueprints)
        {
            if (other.Upgrade == blueprint.Type)
            {
                return other.Type;
            }
        }

        return null;
    }

    public static bool IsSameFamily(BuildingType type, BuildingType other)
    {
        var blueprint = GetBlueprint(type);
        var otherBlueprint = GetBlueprint(other);

        return blueprint.Family == otherBlueprint.Family;
    }

    public static bool FulfillsRequirements(PlayerNode player, MapTile mapTile, BuildingType type)
    {
        var blueprint = GetBlueprint(type);

        var basicRequirement = blueprint.Requirements.All(requirement => requirement.IsFulfilled(player, mapTile));

        var currentLevel = 0;
        var currentBuilding = mapTile.Location.GetBuildingByFamily(type);

        if (currentBuilding != null)
        {
            currentLevel = currentBuilding.Value.Level;
        }

        if (blueprint.AdditionalLevelRequirements == null ||
            !blueprint.AdditionalLevelRequirements.ContainsKey(currentLevel + 1))
        {
            return basicRequirement;
        }

        var additionalRequirements = blueprint.AdditionalLevelRequirements[currentLevel + 1];
        var additionalRequirement =
            additionalRequirements.All(requirement => requirement.IsFulfilled(player, mapTile));

        return basicRequirement && additionalRequirement;
    }

    public static IEnumerable<Requirement> GetRequirements(BuildingType type)
    {
        return GetBlueprint(type).Requirements;
    }

    public static IEnumerable<Production.Production> GetProduction(BuildingType type)
    {
        return GetBlueprint(type).Production;
    }
    
    public static IEnumerable<Requirement> GetAdditionalRequirements(BuildingType type, int level)
    {
        return GetBlueprint(type).AdditionalLevelRequirements?.ContainsKey(level) == true
            ? GetBlueprint(type).AdditionalLevelRequirements[level]
            : null;
    }
}