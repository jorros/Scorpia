using System.IO;

namespace Scorpia.Engine.Asset.SpriteSheetParsers;

internal interface ISpriteSheetParser
{
    SpritesheetDescriptor Read(Stream stream);
}