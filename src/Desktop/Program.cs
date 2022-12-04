using Scorpia.Game;
using Scorpian;
using Scorpian.Network;

var settings = new EngineSettings
{
    Headless = false,
    Name = "Scorpia",
    DisplayName = "Scorpia",
    NetworkEnabled = true,
    NetworkMode = NetworkMode.Client
};

var game = new Game();

#if DEBUG
if (args.Contains("autoconnect"))
{
    Game.AutoConnect = true;
}
#endif

game.Run(settings);