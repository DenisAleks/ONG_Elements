#if UNITY_EDITOR
using System.IO;
using _Game._Scripts.Utils;
using UnityEditor;
using UnityEngine;

namespace _Game.Editor
{
    public static class SaveDebugMenu
    {
        [MenuItem("Game/Delete Save")]
        public static void DeleteSave()
        {
            var path = Path.Combine(Application.persistentDataPath, AppConstants.SaveFileName);

            if (!File.Exists(path))
            {
                Debug.Log("SaveDebug: No save file found");
                return;
            }

            File.Delete(path);
            Debug.Log($"SaveDebug: Save deleted\n{path}");
        }
    }
}
#endif