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

    public int nowBlockType = 0; //0은 normal, 1은 보물상자, 2는 보석, 3은 단단한바위, 4는 유물, 5는 몬스터, 6은 자갈 
    int blockType = 0;

    public float blockHealth = 0;

    [SerializeField] SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    public void ChangeBlock(int newBlockType)
    {
        if(blockType != newBlockType)
        {
            if (blockType != 0)
                Debug.Log(transform.position + " : 잘못된 호출");
            blockHealth = 0;
            nowBlockType = newBlockType;
            blockType = newBlockType;
            if(newBlockType == 0)
            {
                spriteRenderer.sprite = level1_block_Normal_0;
                blockHealth = 3;
            }
            else if(newBlockType == 1)
            {
                spriteRenderer.sprite = level1_block_Normal_1;
            }
            else if (newBlockType == 2)
            {
                spriteRenderer.sprite = level1_block_Jewel_0;
            }
            else if (newBlockType == 3)
            {
                spriteRenderer.sprite = level1_block_Normal_2;
            }
            else if (newBlockType == 4)
            {
                spriteRenderer.sprite = level1_block_Jewel_0;
            }
        }
    }

    public void BlockDestroy(float blockDamage)
    {
        blockHealth -= blockDamage;

        print($"blockHealth: {blockHealth}, blockDamage: {blockDamage}");

        if(blockHealth < 2.25f)
        {
            spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/block/block_breaking_01");
        }
        else if(blockHealth > 1.0f)
        {
            spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/block/block_breaking_02");
        }
        else if(blockHealth > 0.5f)
        {
            spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/block/block_breaking_03");
        }
        else if (blockHealth < 0)
            Destroy(this);
    }

    public void DropCheck()
    {

    }

    public void DropBlock(int dropHeight)
    {

    }

}
