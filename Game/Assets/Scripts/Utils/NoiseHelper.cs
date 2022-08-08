using UnityEngine;

namespace Utils
{
    public static class NoiseHelper
    {
        public static float SumNoise(float x, float y, int octaves, float persistance, float startFrequency)
        {
            var amplitude = 1f;
            var frequency = startFrequency;
            var noiseSum = 0f;
            var amplitudeSum = 0f;
            for (var i = 0; i < octaves; i++)
            {
                noiseSum += amplitude * Mathf.PerlinNoise(x * frequency, y * frequency);
                amplitudeSum += amplitude;
                amplitude *= persistance;
                frequency *= 2;

            }
            
            return noiseSum / amplitudeSum; // set range back to 0-1
        }
        
        public static float RangeMap(float inputValue, float inMin, float inMax, float outMin, float outMax)
        {
            return outMin + (inputValue - inMin) * (outMax - outMin) / (inMax - inMin);
        }
    }
}