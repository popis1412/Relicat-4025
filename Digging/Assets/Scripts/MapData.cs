using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
    public class RectangularBounds
    {
        public float MinX { get; }
        public float MinY { get; }
        public float MaxX { get; }
        public float MaxY { get; }

        public RectangularBounds(float minX, float minY, float maxX, float maxY)
        {
            MinX = minX;
            MinY = minY;
            MaxX = maxX;
            MaxY = maxY;
        }
    }

    public class MapSize
    {
        public Vector3 Center { get; }

        public float X { get; }
        public float Y { get; }

        public float MinX { get; }
        public float MinY { get; }
        public float MaxX { get; }
        public float MaxY { get; }

        public MapSize(Vector3 center, float sizeX, float sizeY)
        {
            // center
            Center = center;

            // size
            X = sizeX;
            Y = sizeY;

            // half
            var xHalf = X / 2;
            var yHalf = Y / 2;

            // x 
            MinX = center.x - xHalf;
            MaxX = center.x + xHalf;

            // y 
            MinY = center.y - yHalf;
            MaxY = center.y + yHalf;
        }
    }

    public class MapSizeIsometric : MapSize
    {
        public Vector3 Bottom { get; }
        public Vector3 Top { get; }
        public Vector3 Left { get; }
        public Vector3 Right { get; }

        public MapSizeIsometric(Vector3 center, float sizeX, float sizeY,
            float cellSizeX, float cellSizeY) : base(center, sizeX, sizeY)
        {
            // isometric point
            Bottom = new Vector3(center.x, MinY * cellSizeY, center.z);
            Top = new Vector3(center.x, MaxY * cellSizeY, center.z);
            Left = new Vector3(MinX * cellSizeX, center.y, center.z);
            Right = new Vector3(MaxX * cellSizeX, center.y, center.z);
        }
    }

}
