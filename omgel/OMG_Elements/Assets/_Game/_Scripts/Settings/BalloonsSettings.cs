using _Game._Scripts.Lvl.Bg;
using _Game._Scripts.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game._Scripts.Settings
{
    [CreateAssetMenu(
        fileName = nameof(BalloonsSettings),
        menuName = AppConstants.Paths.ScriptableObjects + nameof(BalloonsSettings))]
    public class BalloonsSettings : ScriptableObject
    {
        [Tooltip("Possible balloon prefabs to spawn.")]
        public Balloon[] Prefabs;

        [Tooltip("Maximum number of balloons that can exist at the same time.")]
        public int MaxCount = 3;

        [Tooltip("Random delay range between spawn attempts (seconds).")]
        public Vector2 SpawnAttemptDelayRangeSeconds = new Vector2(1f, 3f);

        [Tooltip("Chance that a spawn attempt will actually create a balloon (0–1).")]
        [Range(0f, 1f)]
        public float SpawnProbability = 0.2f;

        [Tooltip("Random horizontal movement speed range.")]
        public Vector2 SpeedXRange = new Vector2(5f, 15f);

        [Tooltip("Random vertical sine wave amplitude (how high balloons float up and down).")]
        public Vector2 AmplitudeRange = new Vector2(5f, 10f);

        [Tooltip("Random sine wave frequency (how fast balloons oscillate vertically).")]
        public Vector2 FrequencyRange = new Vector2(1.2f, 2f);

        [Tooltip("Horizontal offset outside the camera where balloons spawn and despawn.")]
        public float SpawnOffsetX = 10f;

        [Tooltip("Vertical range where balloons can appear.")]
        public Vector2 BalloonsYRange = new Vector2(-30, 100);

        public Balloon GetRandomPrefab()
        {
            var index = Random.Range(0, Prefabs.Length);
            return Prefabs[index];
        }
    }
}