using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UVToTextureConverter))]
public class UVToTextureConverterEditor : Editor
{
    UVToTextureConverter converter;

    Color UvColor = Color.red, BackgroundColor = Color.white;
    int LineThickness = 0, Scale = 0;

    int SelectedUVChannel = 0;
    string[] UVoptions = new string[]
    {
        "UV0", "UV1", "UV2", "UV3", "UV4", "UV5", "UV6", "UV7"
    };


    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        converter = (UVToTextureConverter)target;

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();

        var newUvColor = EditorGUILayout.ColorField("UV Color", UvColor);
        if (newUvColor != UvColor) UvColor.a = 1;
        UvColor = newUvColor;


        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();

        var newBackgroundColor = EditorGUILayout.ColorField("Background Color", BackgroundColor);
        if(newBackgroundColor != BackgroundColor) BackgroundColor.a = 1;
        BackgroundColor = newBackgroundColor;

        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();

        GUILayout.Label("Line Thickness", GUILayout.Width(100));
        LineThickness = (int)GUILayout.HorizontalSlider(LineThickness, 0, 20);

        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();

        GUILayout.Label("Scale", GUILayout.Width(100));
        Scale = (int)GUILayout.HorizontalSlider(Scale, 0, 500);

        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();

        SelectedUVChannel = EditorGUILayout.Popup("Selected UV Channel", SelectedUVChannel, UVoptions);

        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();

        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("Draw UV on Texture"))
        {
            if (converter.TextureResolution.x == 0 || converter.TextureResolution.y == 0) throw new ArgumentException("Texture Resolution is zero");
            UpdateAndDrawUV();
        }

        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();

        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Create Asset Texture"))
        {
            MonoScript thisAsset = MonoScript.FromScriptableObject(this);
            var assetPath = AssetDatabase.GetAssetPath(thisAsset);
            var pluginPath = assetPath.Replace($"/Editor/{thisAsset.name}.cs", "");

            converter.CreateTextureAsset(pluginPath);
        }

        GUILayout.EndHorizontal();
        GUILayout.Space(15);
        GUILayout.BeginHorizontal();

        GUILayout.Label("", GUILayout.Height(EditorGUIUtility.currentViewWidth - 20), GUILayout.Width(EditorGUIUtility.currentViewWidth - 20));
        if(converter.texture != null) EditorGUI.DrawPreviewTexture(GUILayoutUtility.GetLastRect(), converter.texture);

        GUILayout.EndHorizontal();
    }


    private void UpdateAndDrawUV() => converter.DrawUV(converter.mesh, Scale, LineThickness, converter.TextureResolution, UvColor, BackgroundColor, SelectedUVChannel);


    [MenuItem("GameObject/Tools/UVToTextureConverter", priority = 0)]
    public static void CreateEmpty()
    {
        GameObject prefabsBakerGO = new GameObject("UVToTextureConverter");
        Undo.RegisterCreatedObjectUndo(prefabsBakerGO, "Create UVConverter");

        prefabsBakerGO.AddComponent<UVToTextureConverter>();
    }
}
