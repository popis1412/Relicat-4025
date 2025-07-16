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

    public GameObject player;
    [SerializeField] RawImage rawImage;
    private Texture2D texture;
    private RectTransform rectTransform;


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

            float targetX = (mapsizeX / 2 - px) * textureSize;
            float targetY = (-mapsizeY/2 -py) * textureSize;
            rectTransform.anchoredPosition = new Vector2 (targetX, targetY);
        }
    }

    public void SetTextureSize(int stageNum)
    {
        int width, height;

        if(stageNum == 1)   //�������� ��ȣ���� �ؿ� �߰��ϸ� ��
        {
            width = 30 * textureSize;       //�������� ���α��� * textureSize
            mapsizeX = 30;                  //�������� ���α���
            height = 33 * textureSize;      //�������� ���α��� * textureSize
            mapsizeY = 33;                  //�������� ���α���

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

        for (int i = 0; i < clearTex.Length; i++)
            clearTex[i] = Color.clear;
        tex.SetPixels(clearTex);
        tex.Apply();
    }

    public void DrawSquare(int x, int y, Color color)
    {
        int targetX = x * textureSize;
        int targetY = (y + mapsizeY) * textureSize;
        int size = textureSize;
        for (int dx = -size / 2; dx <= size / 2; dx++)
        {
            for (int dy = -size / 2; dy <= size / 2; dy++)
            {
                int px = targetX + dx;
                int py = targetY + dy;
                if (px >= 0 && px < texture.width && py >= 0 && py < texture.height)
                {
                    texture.SetPixel(px, py, color);
                }
                else
                    print($"�ȼ��� �׸����� ��ġ�� �ؽ����� �ٱ���");
            }
        }
        print($"�׸��� �� ��ġ�� ({x}, {y})��");
        texture.Apply();
    }

    public void EraseSquare(int x, int y)
    {
        int targetX = x * textureSize;
        int targetY = (y + mapsizeY) * textureSize;
        int size = textureSize;
        for (int dx = -size / 2; dx <= size / 2; dx++)
        {
            for (int dy = -size / 2; dy <= size / 2; dy++)
            {
                int px = targetX + dx;
                int py = targetY + dy;

                if (px >= 0 && px < texture.width && py >= 0 && py < texture.height)
                {
                    texture.SetPixel(px, py, Color.clear);
                }
                else
                    print("�ȼ��� �׸����� ��ġ�� �ؽ����� �ٱ���");
            }
        }
        print($"������ �� ��ǥ�� ({x},{y})");
        texture.Apply();
    }
}
