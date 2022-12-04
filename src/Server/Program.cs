using Scorpia.Game;
using Scorpian;
using Scorpian.Network;
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
    new SendEventAction()
};

settings.HeadlessLoopAction = () =>
{
    var input = Prompt.Select("Select action", new[] {"Event"});

    var action = actions.FirstOrDefault(x => string.Equals(x.Name, input, StringComparison.InvariantCultureIgnoreCase));
    action?.Execute(game.ServiceProvider);
};

game.Run(settings);