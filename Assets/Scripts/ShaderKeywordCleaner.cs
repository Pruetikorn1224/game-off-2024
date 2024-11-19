using UnityEditor;
using UnityEngine;

public class ShaderKeywordCleaner
{
    [MenuItem("Tools/Clear All Shader Keywords")]
    public static void ClearShaderKeywords()
    {
        ShaderVariantCollection svc = new ShaderVariantCollection();
        svc.Clear();
        Debug.Log("Cleared all Shader Keywords.");
    }
}