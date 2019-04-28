using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OrganismType {Barren, Grass, Bushes, Trees, Hares, Deer, Squirrels, Foxes, Wolves, Owls};

public class Constants
{
    public const int TILES_LAYER_MASK = 1 << 8;
    public const int LIFE_FORCE_CAPACITY = 5;
    public static int[,] offset_directions_even = {
        {0, 1}, {0, -1}, {1, 0}, {1, -1}, {-1, 0}, {-1, -1}
    };
    public static int[,] offset_directions_odd = {
        {0, 1}, {0, -1}, {1, 1}, {1, 0}, {-1, 0}, {-1, 1}
    };
}

public class Helpers
{
    public static Vector3 PointToHexLocation(Point point)
    {
        return new Vector3(3.0f / 4.0f * point.x, 0.1f, (point.y + (point.x % 2) / 2.0f) * Mathf.Sqrt(3.0f) / 2.0f);
    }
}

public struct Point
{
    public int x, y;

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}