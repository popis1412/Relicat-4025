using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MapImage : MonoBehaviour
{
    float px, py;
    int mapsizeX, mapsizeY;
    int textureSize = 15;
    Vector2 leftTopPos = new Vector2(0, 0);

    public GameObject player;
    [SerializeField] RawImage rawImage;
    private Texture2D texture;
    private RectTransform rectTransform;

    public struct MiniMapPixel
    {
        public int x;
        public int y;
        public Color color;

        public MiniMapPixel(int x, int y, Color color)
        {
            this.x = x;
            this.y = y;
            this.color = color;
        }
    }

    public List<MiniMapPixel> pixels = new List<MiniMapPixel>();

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        if(player != null)
        {
            px = player.transform.position.x;
            py = player.transform.position.y;

            float targetX = (mapsizeX / 2 - px + leftTopPos.x) * textureSize;
            float targetY = (-mapsizeY / 2 - py + leftTopPos.y) * textureSize;
            rectTransform.anchoredPosition = new Vector2(targetX, targetY);
        }
    }

    public void SetTextureSize(int stageNum, Vector2 newLeftTopPos)
    {
        int width, height;
        leftTopPos = newLeftTopPos;

        if(stageNum == 0)   //스테이지 번호별로 밑에 추가하면 됨
        {
            width = 30 * textureSize;       //스테이지 가로길이 * textureSize
            mapsizeX = 30;                  //스테이지 가로길이
            height = 33 * textureSize;      //스테이지 세로길이 * textureSize
            mapsizeY = 33;                  //스테이지 세로길이

            texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;

            ClearTexture(texture);

            rawImage.texture = texture;
        }
        else if(stageNum == 1)   //스테이지 번호별로 밑에 추가하면 됨
        {
            width = 50 * textureSize;       //스테이지 가로길이 * textureSize
            mapsizeX = 50;                  //스테이지 가로길이
            height = 53 * textureSize;      //스테이지 세로길이 * textureSize
            mapsizeY = 53;                  //스테이지 세로길이

            texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;

            ClearTexture(texture);

            rawImage.texture = texture;
        }
        else if(stageNum == 2)   //스테이지 번호별로 밑에 추가하면 됨
        {
            width = 50 * textureSize;       //스테이지 가로길이 * textureSize
            mapsizeX = 50;                  //스테이지 가로길이
            height = 53 * textureSize;      //스테이지 세로길이 * textureSize
            mapsizeY = 53;                  //스테이지 세로길이

            texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;

            ClearTexture(texture);

            rawImage.texture = texture;
        }
        else
        {
            width = 30 * textureSize;
            mapsizeX = 30;
            height = 33 * textureSize;
            mapsizeY = 33;

            texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;

            ClearTexture(texture);

            rawImage.texture = texture;
        }
        rectTransform.sizeDelta = new Vector2(width, height);
    }

    public void ClearTexture(Texture2D tex)
    {
        Color[] clearTex = new Color[tex.width * tex.height];

        for(int i = 0; i < clearTex.Length; i++)
            clearTex[i] = Color.clear;
        tex.SetPixels(clearTex);
        tex.Apply();
        pixels.Clear();
    }

    public void DrawSquare(int x, int y, Color color)
    {
        int targetX = (x - Mathf.RoundToInt(leftTopPos.x)) * textureSize;
        int targetY = (y - Mathf.RoundToInt(leftTopPos.y) + mapsizeY) * textureSize;
        int size = textureSize;
        for(int dx = -size / 2; dx <= size / 2; dx++)
        {
            for(int dy = -size / 2; dy <= size / 2; dy++)
            {
                int px = targetX + dx;
                int py = targetY + dy;
                if(px >= 0 && px < texture.width && py >= 0 && py < texture.height)
                {
                    texture.SetPixel(px, py, color);
                }
                else
                    print($"픽셀을 그리려는 위치가 텍스쳐의 바깥임");
            }
        }
        print($"그리려 한 위치는 ({x}, {y})임");
        texture.Apply();
        pixels.Add(new MiniMapPixel(x, y, color));
    }

    public void EraseSquare(int x, int y)
    {
        int targetX = x * textureSize;
        int targetY = (y + mapsizeY) * textureSize;
        int size = textureSize;
        for(int dx = -size / 2; dx <= size / 2; dx++)
        {
            for(int dy = -size / 2; dy <= size / 2; dy++)
            {
                int px = targetX + dx;
                int py = targetY + dy;

                if(px >= 0 && px < texture.width && py >= 0 && py < texture.height)
                {
                    texture.SetPixel(px, py, Color.clear);
                }
                else
                {
                    //print("픽셀을 그리려는 위치가 텍스쳐의 바깥임");
                }

            }
        }
        print($"지우기로 한 좌표는 ({x},{y})");
        texture.Apply();
        int index = pixels.FindIndex(p => p.x == x && p.y == y);
        if(index >= 0)
            pixels.RemoveAt(index);
    }

}
