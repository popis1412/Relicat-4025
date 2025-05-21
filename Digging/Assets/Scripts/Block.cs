using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] GameObject monster1;

    [SerializeField] Sprite block_Normal_0;
    [SerializeField] Sprite block_Normal_1;
    [SerializeField] Sprite block_Normal_2;

    [SerializeField] Sprite block_Jewel_Coal_0;
    [SerializeField] Sprite block_Jewel_Coal_1;
    [SerializeField] Sprite block_Jewel_Coal_2;

    [SerializeField] Sprite block_Jewel_Copper_0;
    [SerializeField] Sprite block_Jewel_Copper_1;
    [SerializeField] Sprite block_Jewel_Copper_2;

    [SerializeField] Sprite block_Jewel_Iron_0;
    [SerializeField] Sprite block_Jewel_Iron_1;
    [SerializeField] Sprite block_Jewel_Iron_2;

    [SerializeField] Sprite block_Jewel_Gold_0;
    [SerializeField] Sprite block_Jewel_Gold_1;
    [SerializeField] Sprite block_Jewel_Gold_2;

    [SerializeField] Sprite block_Jewel_Ruby_0;
    [SerializeField] Sprite block_Jewel_Ruby_1;
    [SerializeField] Sprite block_Jewel_Ruby_2;

    [SerializeField] Sprite block_Jewel_Diamond_0;
    [SerializeField] Sprite block_Jewel_Diamond_1;
    [SerializeField] Sprite block_Jewel_Diamond_2;

    [SerializeField] Sprite block_Rock_0;
    [SerializeField] Sprite block_Rock_1;
    [SerializeField] Sprite block_Rock_2;

    [SerializeField] Sprite block_Sand_0;
    [SerializeField] Sprite block_Sand_1;
    [SerializeField] Sprite block_Sand_2;

    [SerializeField] Sprite block_unbreakable_0;


    public int nowBlockType = 0; //�ٸ� �ڵ忡�� blockChange�� ���������� ���� blockType �������� ������ ������ �ٸ� �ڵ忡�� blockChange�� ȣ���԰� ���ÿ� �̸� ���� blockType ���� �ٲ��� Ȯ���� ����
    int blockType = 0; 
    float blockHealth = 3;
    float blockMaxHealth = 3;

    [SerializeField] SpriteRenderer spriteRenderer;

    public BlocksDictionary blocksDictionary;   //BlocksDictionary���� Chunk�� ȣ���ϴ� AppendBlocksDictionary���� �Ҵ�� ����

    Vector2 targetDropPosition;
    bool isDropping;
    float currentDropSpeed = 0f;
    float dropSpeed = 0.1f;
    float maxDropSpeed = 0.5f;

    private void Awake()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    public void ChangeBlock(int newBlockType)   //�� ��ü ���
    {
        //-1�� ������, 0�� normal, 1�� ��������, 2�� ��ź, 3�� �ܴ��ѹ���, 4�� ����, 5�� ����, 6�� ��, 7�� ����, 8�� ö(��), 9�� ��, 10�� ���, 11�� ���̾�
        if (blockType != 0)
            Debug.Log(transform.position + " : �߸��� ȣ��");

        blockType = newBlockType;
        if (newBlockType == -1)
        {
            spriteRenderer.sprite = block_unbreakable_0;
            blockHealth = 100;
            blockMaxHealth = 100;
        }
        else if (newBlockType == 0)
        {
            spriteRenderer.sprite = block_Normal_0;
            blockHealth = 3;
            blockMaxHealth = 3;
        }
        else if(newBlockType == 1)
        {
            spriteRenderer.sprite = block_Normal_1;     //�������� ���;� ��
        }
        else if (newBlockType == 2)
        {
            spriteRenderer.sprite = block_Jewel_Coal_0;
            blockHealth = 3;
            blockMaxHealth = 3;
        }
        else if (newBlockType == 3)
        {
            spriteRenderer.sprite = block_Rock_0;
            blockHealth = 9;
            blockMaxHealth = 9;
        }
        else if (newBlockType == 4)
        {
            spriteRenderer.sprite = block_Jewel_Diamond_0;  //���� ���;� ��
            blockHealth = 3;
            blockMaxHealth = 3;
        }
        else if (newBlockType == 5)
        {
            spriteRenderer.sprite = block_Normal_0; //�Ϲݺ�ó�� ���̰� ��
            blockHealth = 3;
            blockMaxHealth = 3;
        }
        else if (newBlockType == 6)
        {
            spriteRenderer.sprite = block_Sand_0;
            blockHealth = 3;
            blockMaxHealth = 3;
        }
        else if (newBlockType == 7)
        {
            spriteRenderer.sprite = block_Jewel_Copper_0;
            blockHealth = 3;
            blockMaxHealth = 3;
        }
        else if (newBlockType == 8)
        {
            spriteRenderer.sprite = block_Jewel_Iron_0;
            blockHealth = 3;
            blockMaxHealth = 3;
        }
        else if (newBlockType == 9)
        {
            spriteRenderer.sprite = block_Jewel_Gold_0;
            blockHealth = 3;
            blockMaxHealth = 3;
        }
        else if (newBlockType == 10)
        {
            spriteRenderer.sprite = block_Jewel_Ruby_0;
            blockHealth = 3;
            blockMaxHealth = 3;
        }
        else if (newBlockType == 11)
        {
            spriteRenderer.sprite = block_Jewel_Diamond_0;
            blockHealth = 3;
            blockMaxHealth = 3;
        }

    }

    public void BlockDestroy(float blockDamage, Player playerScript) //*�÷��̾� ��Ʈ�ѷ����� Player�� �߰����ָ�
    {
        print(blockDamage);
        if (blockHealth - blockDamage > 0 && blockType != 1)  //���� ����� �ֱ�
        {
            blockHealth -= blockDamage;

            if (blockHealth < blockMaxHealth/3 * 2)
            {
                if (blockHealth < blockMaxHealth/3) //�� ü���� 3���� 1 ������ ��
                {
                    if (blockType == 0 || blockType == 5) //�Ϲݺ� or ���� ��
                    {
                        spriteRenderer.sprite = block_Normal_2;
                    }
                    else if (blockType == 2) //��ź
                    {
                        spriteRenderer.sprite = block_Jewel_Coal_2;
                    }
                    else if (blockType == 3) //�ܴ��� ����
                    {
                        spriteRenderer.sprite = block_Rock_2;
                    }
                    else if (blockType == 4) //����
                    {
                        spriteRenderer.sprite = block_Jewel_Diamond_2;
                    }
                    else if (blockType == 6) //��
                    {
                        spriteRenderer.sprite = block_Sand_2;
                    }
                    else if (blockType == 7) //����
                    {
                        spriteRenderer.sprite = block_Jewel_Copper_2;
                    }
                    else if (blockType == 8) //ö
                    {
                        spriteRenderer.sprite = block_Jewel_Iron_2;
                    }
                    else if(blockType == 9) //��
                    {
                        spriteRenderer.sprite = block_Jewel_Gold_2;
                    }
                    else if(blockType == 10) //���
                    {
                        spriteRenderer.sprite = block_Jewel_Ruby_2;
                    }
                    else if(blockType == 11) //���̾�
                    {
                        spriteRenderer.sprite = block_Jewel_Diamond_2;
                    }
                }
                else //�� ü���� 3���� 2 ������ ��
                {
                    if (blockType == 0 || blockType == 5) //�Ϲݺ� or ���� ��
                    {
                        spriteRenderer.sprite = block_Normal_1;
                    }
                    else if (blockType == 2) // ��ź
                    {
                        spriteRenderer.sprite = block_Jewel_Coal_1;
                    }
                    else if (blockType == 3) //�ܴ��� ����
                    {
                        spriteRenderer.sprite = block_Rock_1;
                    }
                    else if (blockType == 4) //����
                    {
                        spriteRenderer.sprite = block_Jewel_Diamond_1;
                    }
                    else if (blockType == 6) //��
                    {
                        spriteRenderer.sprite = block_Sand_1;
                    }
                    else if (blockType == 7) //����
                    {
                        spriteRenderer.sprite = block_Jewel_Copper_1;
                    }
                    else if (blockType == 8) //ö
                    {
                        spriteRenderer.sprite = block_Jewel_Iron_1;
                    }
                    else if (blockType == 9) //��
                    {
                        spriteRenderer.sprite = block_Jewel_Gold_1;
                    }
                    else if (blockType == 10) //���
                    {
                        spriteRenderer.sprite = block_Jewel_Ruby_1;
                    }
                    else if (blockType == 11) //���̾�
                    {
                        spriteRenderer.sprite = block_Jewel_Diamond_1;
                    }
                }
            }
        }
        else //�� �ı�
        {
            
            if(blockType == 0)
            {
                //�Ϲ� �� �ν��� ��
            }
            else if (blockType == 1)
            {
                //�������� �ν��� �� 
            }
            else if(blockType == 2)
            {
                //��ź �ν��� ��
                playerScript.Inventory.AddItem(playerScript.minerals[0], 1);
            }
            else if (blockType == 3)
            {
                //�ܴ��� ���� �ν��� ��
            }
            else if (blockType == 4)
            {
                //���� �ν��� ��
            }
            else if (blockType == 5)
            {
                //���� �� �ν��� ��
                Instantiate(monster1);
            }
            else if (blockType == 6)
            {
                //�� �ν��� ��
            }
            else if (blockType == 7)
            {
                //���� �ν��� ��
                playerScript.Inventory.AddItem(playerScript.minerals[1], 1);
            }
            else if (blockType == 8)
            {
                //ö �ν��� ��
                playerScript.Inventory.AddItem(playerScript.minerals[2], 1);
            }
            else if (blockType == 9)
            {
                //�� �ν��� ��
                playerScript.Inventory.AddItem(playerScript.minerals[3], 1);
            }
            else if (blockType == 10)
            {
                //��� �ν��� ��
                playerScript.Inventory.AddItem(playerScript.minerals[4], 1);
            }
            else if (blockType == 11)
            {
                //���̾� �ν��� ��
                playerScript.Inventory.AddItem(playerScript.minerals[5], 1);
            }


            blocksDictionary.blockPosition.Remove(this.transform.position);
            

            if (blocksDictionary.blockPosition.ContainsKey((Vector2)this.transform.position + new Vector2(0, 1))) //�� ���� �� �ִ��� üũ
            {
                GameObject aboveBlock = blocksDictionary.blockPosition[(Vector2)transform.position + new Vector2(0, 1)]; ;
                if (aboveBlock.GetComponent<Block>().blockType == 6) //�� ���� �ִ� ���� �𷡸� ����Ʈ����
                {
                    aboveBlock.GetComponent<Block>().DropBlock(aboveBlock.GetComponent<Block>().DropCheck(this.transform.position, 0));
                }
            }
            Destroy(this.gameObject);//������ �� if������ �� ����������
        }

    }

    public int DropCheck(Vector2 dropCheckPosition, int checkDropHeight) //�� �󸶳� �������� ���(����Լ�)
    {
        if (blocksDictionary.blockPosition.ContainsKey(dropCheckPosition))
        {
            return checkDropHeight;
        }
        else
        {
            return DropCheck(dropCheckPosition + new Vector2(0, -1), checkDropHeight + 1);
        }
    }

    public void DropBlock(int dropHeight) //�� ����Ʈ����
    {
        if (blocksDictionary.blockPosition.ContainsKey((Vector2)this.transform.position + new Vector2(0, 1))) //�� ���� ���ִ��� üũ
        {
            GameObject aboveBlock = blocksDictionary.blockPosition[(Vector2)transform.position + new Vector2(0, 1)]; ;
            if (aboveBlock.GetComponent<Block>().blockType == 6) //�� ���� �ִ� ���� �𷡸� ���� ��������
            {
                aboveBlock.GetComponent<Block>().DropBlock(dropHeight);
            }
        }

        targetDropPosition = (Vector2)this.transform.position - new Vector2(0, dropHeight); //������ ���������ϴ��� Ȯ��
        blocksDictionary.blockPosition.Remove(this.transform.position); //�� ���� ���� �������� �ϴ� ��ųʸ����� ���Ƿ� ����

        isDropping = true; //����Ʈ���°� ���� ������ �ϱ� ������ ����Ʈ����� ������ üũ�ϰ� Update()�� �˾Ƽ� ����Ʈ���� ��
    }

    private void Update()
    {
        if (isDropping) //����������
        {
            if (currentDropSpeed + dropSpeed * Time.deltaTime > maxDropSpeed)   //�ӵ� ���
                currentDropSpeed = maxDropSpeed;
            else currentDropSpeed += dropSpeed * Time.deltaTime;

            Vector2 movePosition = new Vector2(this.transform.position.x, this.transform.position.y - currentDropSpeed); //������ ��ġ

            if (movePosition.y < targetDropPosition.y) //������ ��ġ�� ��ǥ ��ġ���� �Ʒ��� ���̻� �������� ���� �ʰ�, �ӵ� �ʱ�ȭ�ϰ�, ��ųʸ��� �߰�
            {
                this.transform.position = targetDropPosition;
                isDropping = false;
                currentDropSpeed = 0f;
                blocksDictionary.blockPosition.Add(this.transform.position, this.gameObject);
            }
            else
            {
                this.transform.position = movePosition;
            }

            
        }
    }

}
