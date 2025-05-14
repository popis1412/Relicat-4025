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

    [SerializeField] Sprite[] Normal_block_Breaking;   // [�߰�] �μ����� �Ϲ� ���� �̹��� ��������Ʈ)

    public int nowBlockType = 0; //0�� normal, 1�� ��������, 2�� ����, 3�� �ܴ��ѹ���, 4�� ����, 5�� ����, 6�� �ڰ� 
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
                Debug.Log(transform.position + " : �߸��� ȣ��");
            nowBlockType = newBlockType;
            blockType = newBlockType;
            //print($"Obj: {blockType}, health: {blockHealth}");

            if(newBlockType == 0)   // ���
            {
                spriteRenderer.sprite = level1_block_Normal_0;
            }
            else if(newBlockType == 1)  // ���� ����
            {
                spriteRenderer.sprite = level1_block_Normal_1;
            }
            else if (newBlockType == 2) // ����
            {
                spriteRenderer.sprite = level1_block_Jewel_0;
            }
            else if (newBlockType == 3) // �ܴ��� ����
            {
                spriteRenderer.sprite = level1_block_Normal_2;
            }
            else if (newBlockType == 4) // ����
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
