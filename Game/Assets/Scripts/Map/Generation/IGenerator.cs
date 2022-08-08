using Utils;

namespace Map.Generation
{
	public interface IGenerator
	{
		void Generate(Map map, NoiseMap noiseMap);
	}
}

