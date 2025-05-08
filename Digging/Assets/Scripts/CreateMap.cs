using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class CreateMap : MonoBehaviour
{
    public GameObject grass;
    public GameObject drit;

    public int numMultiple;

    // Start is called before the first frame update
    void Start()
    {
        InputTile(this.gameObject.transform.position);
    }

    void InputTile(Vector2 position)
    {
        for (int i = 0; i < 5; i++) // 세로 방향 (y축)
        {
            GameObject columnParent = new GameObject($"Columns_{i+1}");

            for (int j = -10; j < 10; j++) // x축: -10 ~ +9 총 20개
            {
                float xPos = position.x + (0.5f + j * numMultiple); // x는 -9.5 ~ +9.5까지
                float yPos = position.y + (0.5f + i * numMultiple); // y는 줄마다 1씩 증가

                Vector3 tilePos = new Vector3(xPos, -yPos, 0);
                GameObject tile;

                if (i == 0)
                {
                    tile = Instantiate(grass, new Vector3(xPos, -0.5f, 0), Quaternion.identity);
                    tile.name = $"Grass {i} {j}";
                    
                }
                else
                {
                    tile = Instantiate(drit, tilePos, Quaternion.identity);
                    tile.name = $"Drit {i} {j}";
                }

                tile.transform.SetParent(columnParent.transform);
            }
            
        }
    }
}
