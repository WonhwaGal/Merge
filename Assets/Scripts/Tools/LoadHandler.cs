using System.IO;
using UnityEngine;

namespace Code.SaveLoad
{
    public static class LoadHandler
    {
        private readonly static string SavePath = Application.persistentDataPath + "/DataSaver.json";

        public static void Save(ProgressData data)
        {
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(SavePath, json);
        }

        public static ProgressData Load()
        {
            ProgressData result = null;
            if (!File.Exists(SavePath))
                return result;

            string json = File.ReadAllText(SavePath);
            result = JsonUtility.FromJson<ProgressData>(json);

            return result;
        }
    }
}