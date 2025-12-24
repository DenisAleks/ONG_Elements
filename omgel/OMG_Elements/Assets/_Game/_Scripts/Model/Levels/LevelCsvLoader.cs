using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace _Game._Scripts.Model.Levels
{
    public sealed class LevelCsvLoader
    {
        private const string LevelsFolder = "Levels";
        private const string IndexFile = "levels.txt";

        public IEnumerator LoadAll(Action<List<string>> onComplete)
        {
            var result = new List<string>();

#if UNITY_ANDROID && !UNITY_EDITOR
            yield return LoadAndroid(result);
#else
            LoadDesktop(result);
            yield return null;
#endif

            if (result.Count == 0)
            {
                Debug.LogError("LevelCsvLoader: no levels loaded!");
            }

            onComplete?.Invoke(result);
        }

        // =========================
        // ANDROID
        // =========================
        private IEnumerator LoadAndroid(List<string> result)
        {
            var indexPath = Path.Combine(
                Application.streamingAssetsPath,
                LevelsFolder,
                IndexFile
            );

            using var indexRequest = UnityWebRequest.Get(indexPath);
            yield return indexRequest.SendWebRequest();

            if (indexRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to load level index: {indexRequest.error}");
                yield break;
            }

            var lines = indexRequest.downloadHandler.text
                .Split('\n', StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var fileName = line.Trim();
                if (string.IsNullOrEmpty(fileName))
                    continue;

                var levelPath = Path.Combine(
                    Application.streamingAssetsPath,
                    LevelsFolder,
                    fileName
                );

                using var levelRequest = UnityWebRequest.Get(levelPath);
                yield return levelRequest.SendWebRequest();

                if (levelRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Failed to load level {fileName}: {levelRequest.error}");
                    continue;
                }

                result.Add(levelRequest.downloadHandler.text);
            }
        }

        // =========================
        // EDITOR / DESKTOP
        // =========================
        private void LoadDesktop(List<string> result)
        {
            var path = Path.Combine(Application.streamingAssetsPath, LevelsFolder);

            if (!Directory.Exists(path))
            {
                Debug.LogError($"Levels folder not found: {path}");
                return;
            }

            var files = Directory.GetFiles(path, "*.csv");

            foreach (var file in files)
            {
                result.Add(File.ReadAllText(file));
            }
        }
    }
}
