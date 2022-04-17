using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Actors;
using Blueprints;
using Blueprints.Requirements;
using Map;
using UI.Tooltip;
using Utils;

namespace UI.ActionBar
{
    public class CityActionBar : IActionBar
    {
        private readonly ActionBarSystem system;

        public CityActionBar(ActionBarSystem system)
        {
            this.system = system;
        }

        public bool ShouldRender(MapTile tile)
        {
            return tile.Location != null;
        }

        public void Render(MapTile tile)
        {
            var location = tile.Location;
            system.SetMButtons(LocationBlueprint.GetMaxSlots(location.Type.Value));

            foreach (var building in location.Buildings)
            {
                if (building.IsBuilding)
                {
                    AddIncompleteBuildingButton(tile, building);
                    continue;
                }

                AddCompletedBuildingButton(tile, building);
            }

            AddEmptyButton(tile);

            system.SetSButtons(0);
        }

        private void AddBuildingList(MapTile mapTile, MapTile tile)
        {
            var buildList = new List<(BuildingType type, bool enabled)>();
            var buildings = mapTile.Location.Buildings.ToEnumerable().ToArray();

            var toBeDisplayed = new[]
            {
                BuildingType.Residence,
                BuildingType.Smallholding,
                BuildingType.Mine,
                BuildingType.Mint,
                BuildingType.Refinery,
                BuildingType.Barracks,
                BuildingType.School
            };

            foreach (var display in toBeDisplayed)
            {
                if (buildings.All(x => !BuildingBlueprints.IsSameFamily(display, x.Type)))
                {
                    buildList.Add((display,
                        BuildingBlueprints.FulfillsRequirements(Game.GetSelf(), tile, display)));
                }
            }

            system.SetExtraButtons(buildList.Count);

            foreach (var (type, enabled) in buildList)
            {
                AddBuildExtraButton(type, mapTile, new ActionButtonOptions {Disabled = !enabled});
            }
        }

        private void AddBuildExtraButton(BuildingType type, MapTile mapTile, ActionButtonOptions options = null)
        {
            var buildIcon = BuildingTypeToIcon(type);
            var buildName = BuildingBlueprints.GetName(type);
            var buildReqs = RequirementsToText(type, Game.GetSelf(), mapTile);
            var buildDesc = BuildingBlueprints.GetDescription(type);

            system.AddExtraAction(buildIcon, new TooltipDescription($"Build {buildName}", buildDesc, buildReqs),
                () =>
                {
                    system.ToggleExtra();
                    
                    mapTile.Location.BuildServerRpc(type);
                }, options);
        }

        private void AddDemolishExtraButton(Location location, Building building)
        {
            var name = BuildingBlueprints.GetName(building.Type);
            system.AddExtraAction(16, new TooltipDescription($"Demolish {name}", $"Instantly demolishes {name}."),
                () => { location.DemolishServerRpc(building.Type); });
        }

        private void AddEmptyButton(MapTile tile)
        {
            system.AddMAction(0, new TooltipDescription("Build", "Empty build slot"), () =>
            {
                if (!system.ToggleExtra())
                {
                    return;
                }

                AddBuildingList(tile, tile);
            });
        }

        private void AddCompletedBuildingButton(MapTile mapTile, Building building)
        {
            var icon = BuildingTypeToIcon(building.Type);
            var name = BuildingBlueprints.GetName(building.Type);
            var reqs = RequirementsToText(building.Type, Game.GetSelf(), mapTile);
            var desc = BuildingBlueprints.GetDescription(building.Type);
            var options = new ActionButtonOptions
            {
                UpgradeLevel = building.Level
            };

            system.AddMAction(icon, new TooltipDescription(name, desc, reqs), () =>
            {
                if (!system.ToggleExtra())
                {
                    return;
                }

                var upgradeBlueprint = BuildingBlueprints.GetUpgrade(building.Type);

                system.SetExtraButtons(upgradeBlueprint == null ? 1 : 2);

                if (upgradeBlueprint != null)
                {
                    AddBuildExtraButton(upgradeBlueprint.Value, mapTile, new ActionButtonOptions
                    {
                        Type = ActionButtonOptions.ActionButtonType.Upgrade
                    });
                }

                AddDemolishExtraButton(mapTile.Location, building);
            }, options);
        }

        private void AddIncompleteBuildingButton(MapTile mapTile, Building building)
        {
            var icon = BuildingTypeToIcon(building.Type);
            var name = BuildingBlueprints.GetName(building.Type);
            var desc = BuildingBlueprints.GetDescription(building.Type);
            var reqs = RequirementsToText(building.Type, Game.GetSelf(), mapTile);
            var progress = (int) Math.Round((float) mapTile.Location.InvestedConstruction.Value /
                                            BuildingBlueprints.GetConstructionCost(building.Type) *
                                            100f);

            var options = new ActionButtonOptions
            {
                Progress = progress,
                Type = ActionButtonOptions.ActionButtonType.InProgress,
                UpgradeLevel = building.Level
            };

            system.AddMAction(icon, new TooltipDescription(name, desc, reqs), () =>
            {
                if (!system.ToggleExtra())
                {
                    return;
                }

                system.SetExtraButtons(1);
                AddDemolishExtraButton(mapTile.Location, building);
            }, options);
        }

        private static string RequirementsToText(BuildingType type, Player player, MapTile mapTile)
        {
            var requirements = BuildingBlueprints.GetRequirements(type).ToArray();

            Array.Sort(requirements);

            bool monthly = false, deposit = false, locationType = false;

            var sb = new StringBuilder();
            foreach (var requirement in requirements)
            {
                var valid = requirement.IsFulfilled(player, mapTile);

                switch (requirement.Index)
                {
                    case >= 10 and < 20 when !monthly:
                        monthly = true;
                        sb.Append("\nMonthly: ");
                        break;
                    case >= 20 and < 30 when !deposit && !valid:
                        deposit = true;
                        sb.Append("\nRequires: ");
                        break;
                    case >= 30 and < 40 when !locationType && !valid:
                        locationType = true;
                        sb.Append("\nSettlement needs to be: ");
                        break;
                }

                sb.Append(GetRequirementText(requirement, valid));

                sb.Append(" ");
            }

            return sb.ToString();
        }

        private static string GetRequirementText(Requirement requirement, bool valid) => (requirement, valid) switch
        {
            (CostRequirement, _) => $"<sprite name=tooltip_icon_coin>{requirement.Value.FormatValid(valid)}",
            (NitraRequirement, _) => $"<sprite name=tooltip_icon_nitra>{requirement.Value.FormatValid(valid)}",
            (SofrumRequirement, _) => $"<sprite name=tooltip_icon_sofrum>{requirement.Value.FormatValid(valid)}",
            (ZellosRequirement, _) => $"<sprite name=tooltip_icon_zellos>{requirement.Value.FormatValid(valid)}",
            (UpkeepRequirement, _) => $"<sprite name=tooltip_icon_coin>{requirement.Value.FormatValid(valid)}",
            (FertilityRequirement, false) => $"<sprite name=tooltip_icon_fertility>{(Fertility) requirement.Value}"
                .FormatValid(false),
            (GoldDepositRequirement, false) => $"<sprite name=tooltip_icon_gold> deposit".FormatValid(false),
            (NitraDepositRequirement, false) => $"<sprite name=tooltip_icon_nitra> deposit".FormatValid(false),
            (SofrumDepositRequirement, false) => $"<sprite name=tooltip_icon_sofrum> deposit".FormatValid(false),
            (ZellosDepositRequirement, false) => $"<sprite name=tooltip_icon_zellos> deposit".FormatValid(false),
            (LocationTypeRequirement, false) => $"{(LocationType) requirement.Value}".FormatValid(false),
            (OrRequirement orRequirement, _) =>
                $"{string.Join(" or ", orRequirement.Requirements.Select(x => GetRequirementText(x, valid)))}",
            _ => ""
        };

        private static int BuildingTypeToIcon(BuildingType type) => type switch
        {
            BuildingType.Academy => 1,
            BuildingType.Barracks => 2,
            BuildingType.Bunker => 3,
            BuildingType.DeepMine => 4,
            BuildingType.Estate => 5,
            BuildingType.Farm => 6,
            BuildingType.Forts => 7,
            BuildingType.Mine => 8,
            BuildingType.Mint => 9,
            BuildingType.Refinery => 10,
            BuildingType.RefineryComplex => 11,
            BuildingType.Residence => 12,
            BuildingType.School => 13,
            BuildingType.Smallholding => 14,
            BuildingType.University => 15,
            _ => 0
        };
    }
}