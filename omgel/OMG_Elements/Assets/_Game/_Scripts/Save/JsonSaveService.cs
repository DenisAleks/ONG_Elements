using System.IO;
using _Game._Scripts.Model.Data.Save;
using _Game._Scripts.Utils;
using UnityEngine;

namespace _Game._Scripts.Save
{
    public sealed class JsonSaveService : ISaveService
    {
        private string SavePath =>
            Path.Combine(Application.persistentDataPath, AppConstants.SaveFileName);

        public void Save(GameSaveData data)
        {
            var json = JsonUtility.ToJson(data, true);
            File.WriteAllText(SavePath, json);
        }

        public bool TryLoad(out GameSaveData data)
        {
            if (!File.Exists(SavePath))
            {
                data = null;
                return false;
            }

            var json = File.ReadAllText(SavePath);
            data = JsonUtility.FromJson<GameSaveData>(json);
            return true;
        }

        public void Clear()
        {
            if (File.Exists(SavePath))
            {
                File.Delete(SavePath);
            }
        }
    }
}