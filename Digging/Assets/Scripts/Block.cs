using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] Sprite level1_block_Normal_0;
    [SerializeField] Sprite level1_block_Normal_1;
    [SerializeField] Sprite level1_block_Normal_2;

    [SerializeField] Sprite level1_block_Jewel_0;
    [SerializeField] Sprite level1_block_Jewel_1;
    [SerializeField] Sprite level1_block_Jewel_2;

    [SerializeField] Sprite[] Normal_block_Breaking;   // [추가] 부서지는 일반 흙블록 이미지 스프라이트)

    public int nowBlockType = 0; //0은 normal, 1은 보물상자, 2는 보석, 3은 단단한바위, 4는 유물, 5는 몬스터, 6은 자갈 
    [SerializeField]int blockType = 0;

    public float blockHealth = 0;

    [SerializeField] SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        blockHealth = 3;
        Normal_block_Breaking = Resources.LoadAll<Sprite>("Sprites/block/Block_breaking");
    }

    public void ChangeBlock(int newBlockType)
    {
        if(blockType != newBlockType)
        {
            if (blockType != 0)
                Debug.Log(transform.position + " : 잘못된 호출");
            nowBlockType = newBlockType;
            blockType = newBlockType;
            //print($"Obj: {blockType}, health: {blockHealth}");

            if(newBlockType == 0)   // 노멀
            {
                spriteRenderer.sprite = level1_block_Normal_0;
            }
            else if(newBlockType == 1)  // 보물 상자
            {
                spriteRenderer.sprite = level1_block_Normal_1;
            }
            else if (newBlockType == 2) // 보석
            {
                spriteRenderer.sprite = level1_block_Jewel_0;
            }
            else if (newBlockType == 3) // 단단한 바위
            {
                spriteRenderer.sprite = level1_block_Normal_2;
            }
            else if (newBlockType == 4) // 유물
            {
                spriteRenderer.sprite = level1_block_Jewel_0;
            }
        }
    }

    public void BlockDestroy(float blockDamage, GameObject player)
    {
        blockHealth -= blockDamage;

        print($"blockHealth: {blockHealth}, blockDamage: {blockDamage}");

        if(blockHealth > 2.5f)
        {
            spriteRenderer.sprite = Normal_block_Breaking[0];
        }
        else if(blockHealth > 1.5f)
        {
            spriteRenderer.sprite = Normal_block_Breaking[1];
        }
        else if(blockHealth > 0.5f)
        {
            spriteRenderer.sprite = Normal_block_Breaking[2];
        }
        else if (blockHealth < 0)
        {
            Destroy(this.gameObject);
        }

    }

    public void DropCheck()
    {

    }

    public void DropBlock(int dropHeight)
    {

    }

}
