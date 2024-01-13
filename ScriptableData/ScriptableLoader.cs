using System.IO;
using UnityEditor;
using UnityEngine;

namespace Sonic853.ScriptableData
{
    public class ScriptableLoader<T> : ScriptableObject where T : ScriptableLoader<T>, new()
    {
        /// <summary>
        /// Default: Assets/Sonic853/Data
        /// </summary>
        protected static string savePath = Path.Combine("Assets", "Sonic853", "Data");
        /// <summary>
        /// Default: {typeof(T).Name}.asset
        /// </summary>
        protected static string fileName = null;
        protected static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = CreateInstance<T>();
                    instance.Load();
                }
                return instance;
            }
        }
        public virtual void Load()
        {
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);
            if (string.IsNullOrEmpty(fileName))
                fileName = $"{typeof(T).Name}.asset";
            string filePath = Path.Combine(savePath, fileName);
            var _instance = AssetDatabase.LoadAssetAtPath<T>(filePath);
            if (_instance == null)
            {
                _instance = CreateInstance<T>();
                AssetDatabase.CreateAsset(_instance, filePath);
                AssetDatabase.SaveAssets();
            }
            instance = _instance;
        }
        public virtual void Save()
        {
            if (string.IsNullOrEmpty(fileName))
                fileName = $"{typeof(T).Name}.asset";
            string filePath = Path.Combine(savePath, fileName);
            if (File.Exists(filePath))
                EditorUtility.SetDirty(instance);
            else
            {
                if (!Directory.Exists(savePath))
                    Directory.CreateDirectory(savePath);
                AssetDatabase.CreateAsset(instance, filePath);
            }
            AssetDatabase.SaveAssets();
        }
    }
}
