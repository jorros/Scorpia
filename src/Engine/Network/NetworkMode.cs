using System;

namespace Scorpia.Engine.Network;

[Flags]
public enum NetworkMode
{
    Client = 1,
    Server = 2
}