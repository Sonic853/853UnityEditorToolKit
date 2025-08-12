using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Sonic853.EditorTools
{
    public class CreateMaterialWithTexture : MonoBehaviour
    {
        /// <summary>
        /// 根据右键贴图创建材质至与贴图同一目录下
        /// </summary>
        /// <param name="command"></param>
        [MenuItem("Assets/Create/Material With Texture", false, 301)]
        static void DoubleMass()
        {
            // var selectedTextures = Selection.GetFiltered<Texture>(SelectionMode.DeepAssets);
            var selectedTextures = Selection.GetFiltered<Texture>(SelectionMode.Assets);
            foreach (var texture in selectedTextures)
            {
                var material = new Material(Shader.Find("Standard"));
                material.SetTexture("_MainTex", texture);
                var path = AssetDatabase.GetAssetPath(texture);
                var dir = Path.GetDirectoryName(path);
                var name = Path.GetFileNameWithoutExtension(path);
                AssetDatabase.CreateAsset(material, $"{dir}/{name}.mat");
            }
        }
    }
}
