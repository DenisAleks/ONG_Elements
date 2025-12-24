using System;
using System.Collections;
using System.Collections.Generic;
using _Game._Scripts.Model.Levels;

namespace _Game._Scripts.Lvl
{
    public sealed class LevelRepository
    {
        private readonly List<LevelTable> _levels = new();

        public IReadOnlyList<LevelTable> Levels => _levels;
        public bool IsLoaded { get; private set; }

        public void LoadAll(Action onComplete) 
            => CoroutineRunner.Instance.StartCoroutine(LoadCoroutine(onComplete));

        private IEnumerator LoadCoroutine(Action onComplete)
        {
            var loader = new LevelCsvLoader();
            yield return loader.LoadAll(csvTexts =>
            {
                for (var i = 0; i < csvTexts.Count; i++)
                {
                    var csv = csvTexts[i];
                    _levels.Add(LevelTableParser.Parse(i + 1, csv));
                }
            });

            IsLoaded = true;
            onComplete?.Invoke();
        }
    }
}