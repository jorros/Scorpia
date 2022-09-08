using Scorpia.Engine;
using Scorpia.Engine.Network;
using Scorpia.Game;
using Server.Actions;
using Sharprompt;

var settings = new EngineSettings
{
    Headless = true,
    Name = "ScorpiaServer",
    DisplayName = "Scorpia",
    NetworkEnabled = true,
    NetworkMode = NetworkMode.Server
};

var game = new Game();

var actions = new IAction[]
{
    new TestAction()
};

settings.HeadlessLoopAction = () =>
{
    var input = Prompt.Select("Select action", new[] {"Test"});

    var action = actions.FirstOrDefault(x => string.Equals(x.Name, input, StringComparison.InvariantCultureIgnoreCase));
    action?.Execute(game.ServiceProvider);
};

game.Run(settings);