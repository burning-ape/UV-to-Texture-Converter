using System;
using UnityEditor;
using UnityEngine;

public class UVToTextureConverter : MonoBehaviour
{
    public Mesh mesh;
    public Vector2Int TextureResolution = new Vector2Int(1024, 1024);
    [HideInInspector] public Texture2D texture;

#if UNITY_EDITOR
    public void DrawUV(Mesh mesh, int offset, int lineThickness, Vector2Int textureResolution, Color uvColor, Color backgroundColor, int uvChannel)
    {
        texture = new Texture2D(textureResolution.x, textureResolution.y, TextureFormat.RGBA32, false, false);
        texture.filterMode = FilterMode.Bilinear;
        texture.alphaIsTransparency = true;
        texture.EncodeToPNG();

        var d = GetMeshUV(mesh, uvChannel);

        UVDrawer.DrawUVOnTexture(texture, d, mesh, offset, lineThickness, uvColor, backgroundColor);
    }


    private Vector2[] GetMeshUV(Mesh mesh, int uvIndex)
    {
        Vector2[] selectedUVs = new Vector2[mesh.uv.Length];

        if (uvIndex == 0) { CheckIfUVisNull(mesh.uv); selectedUVs = mesh.uv; }
        if (uvIndex == 1) { CheckIfUVisNull(mesh.uv2); selectedUVs = mesh.uv2; }
        if (uvIndex == 2) { CheckIfUVisNull(mesh.uv3); selectedUVs = mesh.uv3; }
        if (uvIndex == 3) { CheckIfUVisNull(mesh.uv4); selectedUVs = mesh.uv4; }
        if (uvIndex == 4) { CheckIfUVisNull(mesh.uv5); selectedUVs = mesh.uv5; }
        if (uvIndex == 5) { CheckIfUVisNull(mesh.uv6); selectedUVs = mesh.uv6; }
        if (uvIndex == 6) { CheckIfUVisNull(mesh.uv7); selectedUVs = mesh.uv7; }
        if (uvIndex == 7) { CheckIfUVisNull(mesh.uv8); selectedUVs = mesh.uv8; }

        return selectedUVs;
    }


    private void CheckIfUVisNull(Vector2[] uvs)
    {
        if (uvs.Length == 0)
            throw new ArgumentException("This UV channel is null");
    }



    public void CreateTextureAsset(string path)
    {
        if (texture == null) throw new ArgumentException("Texture is not created");

        var texName = AssetDatabase.GenerateUniqueAssetPath($"{path}/GeneratedTextures/UV_Texture.asset");      
        AssetDatabase.CreateAsset(texture, texName);
        AssetDatabase.Refresh();
    }
#endif
}

