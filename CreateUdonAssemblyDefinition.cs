using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Sonic853.EditorTools
{
    public class CreateUdonAssemblyDefinition : MonoBehaviour
    {
        /// <summary>
        /// 创建 Assembly Definition，禁用 Use GUIDs，添加 VRC.Udon 与 UdonSharp.Runtime 引用。
        /// </summary>
        /// <param name="command"></param>
        [MenuItem("Assets/Create/Assembly Definition(Add Udon References)", false, 91)]
        static void NewUdonAssemblyDefinition()
        {
            // 选择路径
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(path))
                path = "Assets";

            // 如果选中的是文件，取它所在的文件夹
            if (Path.HasExtension(path))
                path = Path.GetDirectoryName(path);

            var filePath = EditorUtility.SaveFilePanel(
                "Create Udon Assembly Definition",
                path,
                "NewUdonAssembly",
                "asmdef"
            );
            if (string.IsNullOrEmpty(filePath))
                return;

            // 获取程序集名
            var assemblyName = Path.GetFileNameWithoutExtension(filePath);

            // 创建 asmdef 内容
            var asmdefJson = new AssemblyDefinitionData()
            {
                name = assemblyName,
                references = new string[]
                {
                    "VRC.Udon",
                    "UdonSharp.Runtime"
                }
            };

            var json = JsonConvert.SerializeObject(asmdefJson, Formatting.Indented);
            File.WriteAllText(filePath, json);
            // 刷新资源
            AssetDatabase.Refresh();
        }
        [System.Serializable]
        private class AssemblyDefinitionData
        {
            public string name = "";
            public string rootNamespace = "";
            public string[] references;
            public string[] includePlatforms = new string[0];
            public string[] excludePlatforms = new string[0];
            public bool allowUnsafeCode = false;
            public bool overrideReferences = false;
            public string[] precompiledReferences = new string[0];
            public bool autoReferenced = true;
            public string[] defineConstraints = new string[0];
            public object[] versionDefines = new object[0];
            public bool noEngineReferences = false;
        }
    }
}
