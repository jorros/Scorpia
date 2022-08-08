using System;

namespace Scorpia.Engine;

public class EngineException : Exception
{
    public EngineException(string message) : base(message)
    {
    }
}