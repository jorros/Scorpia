using Microsoft.Extensions.DependencyInjection;
using Scorpia.Engine.SceneManagement;

namespace Scorpia.Engine;

public static class ServiceCollectionExtensions
{
    public static void AddScene<T>(this IServiceCollection services) where T : Scene
    {
        services.AddTransient<T>();
    }
}