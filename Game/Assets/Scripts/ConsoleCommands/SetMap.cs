using System.Collections.Generic;
using DevConsole;
using Scorpia.Assets.Scripts.Map;
using UnityEngine;

namespace Scorpia.Assets.Scripts.ConsoleCommands
{
    [ConsoleCommand(new string[] { "setmap" })]
    public class SetMap
    {
        public static string Help(string command, bool verbose)
        {
            return "Changes variables of the noise generator";
        }

        public static string Execute(string[] tokens)
        {
            var map = GameObject.FindWithTag("Map");

            if (map == null)
            {
                return "[Error] Could not find map object";
            }

            var behaviour = map.GetComponent<MapRenderer>();

            if (tokens.Length != 4)
            {
                return "[Error] Invalid parameters";
            }

            var scale = float.Parse(tokens[0]);
            var octaves = int.Parse(tokens[1]);
            var persistence = float.Parse(tokens[2]);
            var lacunarity = float.Parse(tokens[3]);

            //behaviour.map.SCALE = scale;
            //behaviour.map.OCTAVES = octaves;
            //behaviour.map.PERSISTENCE = persistence;
            //behaviour.map.LACUNARITY = lacunarity;

            behaviour.Refresh();

            return "Map generated";
        }

        public static List<string> FetchAutocompleteOptions(string command, string[] tokens)
        {
            var list = new List<string>();

            if (tokens.Length < 1)
            {
                list.Add("<SCALE>");
            }
            if (tokens.Length < 2)
            {
                list.Add("<OCTAVES>");
            }
            if (tokens.Length < 3)
            {
                list.Add("<PERSISTENCE>");
            }
            if (tokens.Length < 4)
            {
                list.Add("<LACUNARITY>");
            }

            return list;
        }
    }
}