using System.Collections.Generic;
using UnityEngine;

public class UVDrawer
{
    /// <summary>
    /// Draws selected UV on texture.
    /// </summary>
    public static void DrawUVOnTexture(Texture2D texture, Vector2[] meshUVs, Mesh mesh, int offsetFromEdges, int lineThickness, Color uvColor, Color backgroundColor)
    {
        Vector2 textureResolution = new Vector2(texture.width, texture.height);

        texture = FillBackground(texture, backgroundColor);

        var triangles = GetTrianglesWithVertices(mesh);

        var previousPoint = new Vector2Int();
        var firstPoint = new Vector2Int();
        for (int i = 0; i < triangles.Count; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                var currentVertex = meshUVs[triangles[i].usedVertices[j]];
                var vertexPos = GetVertexPositionOnUV(currentVertex, offsetFromEdges, textureResolution);

                // If its not first point, than draw Line
                if (j > 0) DrawLineBetweenTwoPixels(vertexPos.x, vertexPos.y, previousPoint.x, previousPoint.y, uvColor, texture, lineThickness);
                // else if its first point, get its position
                else if (j == 0) { firstPoint = vertexPos; }

                // If it is last point draw also line between current point and the first point
                if (j == 2) { DrawLineBetweenTwoPixels(vertexPos.x, vertexPos.y, firstPoint.x, firstPoint.y, uvColor, texture, lineThickness); }

                previousPoint = vertexPos;
            }
        }

        texture.Apply();
    }


    /// <summary>
    /// Fills textures background with one color.
    /// </summary>
    public static Texture2D FillBackground(Texture2D texture, Color color)
    {
        var pixels = texture.GetPixels();

        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                texture.SetPixel(x, y, color);
            }
        }

        return texture;
    }


    /// <summary>
    /// Uses Bresenham's line algorithm to draw line between two pixels.
    /// </summary>
    private static void DrawLineBetweenTwoPixels(int x, int y, int x2, int y2, Color color, Texture2D texture, int lineThickness)
    {
        int w = x2 - x;
        int h = y2 - y;
        int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;

        if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
        if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
        if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;

        int longest = Mathf.Abs(w);
        int shortest = Mathf.Abs(h);

        if (!(longest > shortest))
        {
            longest = Mathf.Abs(h);
            shortest = Mathf.Abs(w);
            if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
            dx2 = 0;
        }

        int numerator = longest >> 1;

        for (int i = 0; i <= longest; i++)
        {
            texture.SetPixel(x, y, color);

            if (lineThickness > 0)
            {
                for (int t = 0; t < lineThickness; t++)
                {
                    texture.SetPixel(x + t + 1, y, color);
                    texture.SetPixel(x, y + t + 1, color);
                    texture.SetPixel(x - t - 1, y, color);
                    texture.SetPixel(x, y - t - 1, color);
                }
            }


            numerator += shortest;

            if (!(numerator < longest))
            {
                numerator -= longest;

                x += dx1;
                y += dy1;
            }
            else
            {
                x += dx2;
                y += dy2;
            }
        }
    }


    /// <summary>
    /// Triangle with vertex it uses
    /// </summary>
    private struct Triangle { public int[] usedVertices; }


    /// <summary>
    /// Get triangles with its vertex
    /// </summary>
    /// <returns></returns>
    private static List<Triangle> GetTrianglesWithVertices(Mesh mesh)
    {
        var triangles = new List<Triangle>();

        int triangleAmount = mesh.triangles.Length / 3;
        int lastIndex = 0;
        for (int i = 0; i < triangleAmount; i++)
        {
            var newTriangle = new Triangle();
            newTriangle.usedVertices = new int[3];

            for (int j = 0; j < 3; j++)
            {
                newTriangle.usedVertices[j] = mesh.triangles[lastIndex];
                lastIndex++;
            }

            triangles.Add(newTriangle);
        }

        return triangles;
    }


    /// <summary>
    /// Get vertex position on the texture.
    /// </summary>
    /// <param name="vertex">Vertex you want to get positions on texture</param>
    /// <param name="uvOffsetFromEdges">Offset of the UV from the edges of the texture</param>
    /// <returns>Returns vertex position in Vector2int variable on given texture.</returns>
    private static Vector2Int GetVertexPositionOnUV(Vector2 vertex, int uvOffsetFromEdges, Vector2 textureResolution)
    {
        float x = Mathf.Lerp(0 + uvOffsetFromEdges, textureResolution.x - uvOffsetFromEdges, vertex.x);
        float y = Mathf.Lerp(0 + uvOffsetFromEdges, textureResolution.y - uvOffsetFromEdges, vertex.y);
        var verticesInt = Vector2Int.RoundToInt(new Vector2(x, y));
        return verticesInt;
    }
}
