using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using VRC.Udon.Editor.ProgramSources;


namespace Sonic853.EditorTools
{
    public class MoveSerializedUdonProgramFile : MonoBehaviour
    {
        /// <summary>
        /// 从 SerializedUdonPrograms 移动对应的 SerializedProgramAsset 文件到同一目录
        /// </summary>
        /// <param name="command"></param>
        [MenuItem("Assets/Move/Serialized Udon Program File")]
        static void MoveFile()
        {
            var selectedAssets = Selection.GetFiltered<UdonAssemblyProgramAsset>(SelectionMode.Assets);
            var defaultSerializedProgramAssetPath = Path.Combine("Assets", "SerializedUdonPrograms");
            foreach (var asset in selectedAssets)
            {
                var path = AssetDatabase.GetAssetPath(asset);
                var dir = Path.GetDirectoryName(path);
                var serializedProgramAsset = asset.SerializedProgramAsset;
                if (serializedProgramAsset == null)
                    continue;
                var serializedProgramAssetPath = AssetDatabase.GetAssetPath(serializedProgramAsset);
                if (
                    !serializedProgramAssetPath.StartsWith(defaultSerializedProgramAssetPath)
                    && !serializedProgramAssetPath.StartsWith("Assets/SerializedUdonPrograms/")
                    && !serializedProgramAssetPath.StartsWith("Assets\\SerializedUdonPrograms\\")
                ) { continue; }
                var name = Path.GetFileName(serializedProgramAssetPath);
                var serializedProgramAssetDir = Path.Combine(dir, "SerializedUdonPrograms");
                if (!Directory.Exists(serializedProgramAssetDir)) AssetDatabase.CreateFolder(dir, "SerializedUdonPrograms");
                AssetDatabase.MoveAsset(serializedProgramAssetPath, Path.Combine(serializedProgramAssetDir, name));
            }
        }
    }
}
