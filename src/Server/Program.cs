using Scorpia.Engine;
using Scorpia.Engine.Network;
using Scorpia.Game;

var settings = new EngineSettings
{
    Headless = true,
    Name = "ScorpiaServer",
    DisplayName = "Scorpia",
    NetworkEnabled = true,
    NetworkMode = NetworkMode.Server
};

var game = new Game();
game.Run(settings);

while (true)
{
    
}