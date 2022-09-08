namespace Server.Actions;

public interface IAction
{
    string Name { get; }

    void Execute(IServiceProvider serviceProvider);
}