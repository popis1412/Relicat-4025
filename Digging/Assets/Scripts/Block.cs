using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] GameObject relicEffect;
    GameObject actvieRelicEffect;
    

    [SerializeField] GameObject dropItem;

    [SerializeField] GameObject monster1;

    [SerializeField] Sprite block_Normal_0;
    [SerializeField] Sprite block_Normal_1;
    [SerializeField] Sprite block_Normal_2;
    [SerializeField] Sprite block_Normal_3;
    [SerializeField] Sprite block_Normal_4;

    [SerializeField] Sprite block_Jewel_Coal_0;
    [SerializeField] Sprite block_Jewel_Coal_1;
    [SerializeField] Sprite block_Jewel_Coal_2;
    [SerializeField] Sprite block_Jewel_Coal_3;
    [SerializeField] Sprite block_Jewel_Coal_4;

    [SerializeField] Sprite block_Jewel_Copper_0;
    [SerializeField] Sprite block_Jewel_Copper_1;
    [SerializeField] Sprite block_Jewel_Copper_2;
    [SerializeField] Sprite block_Jewel_Copper_3;
    [SerializeField] Sprite block_Jewel_Copper_4;

    [SerializeField] Sprite block_Jewel_Iron_0;
    [SerializeField] Sprite block_Jewel_Iron_1;
    [SerializeField] Sprite block_Jewel_Iron_2;
    [SerializeField] Sprite block_Jewel_Iron_3;
    [SerializeField] Sprite block_Jewel_Iron_4;

    [SerializeField] Sprite block_Jewel_Gold_0;
    [SerializeField] Sprite block_Jewel_Gold_1;
    [SerializeField] Sprite block_Jewel_Gold_2;
    [SerializeField] Sprite block_Jewel_Gold_3;
    [SerializeField] Sprite block_Jewel_Gold_4;

    [SerializeField] Sprite block_Jewel_Ruby_0;
    [SerializeField] Sprite block_Jewel_Ruby_1;
    [SerializeField] Sprite block_Jewel_Ruby_2;
    [SerializeField] Sprite block_Jewel_Ruby_3;
    [SerializeField] Sprite block_Jewel_Ruby_4;

    [SerializeField] Sprite block_Jewel_Diamond_0;
    [SerializeField] Sprite block_Jewel_Diamond_1;
    [SerializeField] Sprite block_Jewel_Diamond_2;
    [SerializeField] Sprite block_Jewel_Diamond_3;
    [SerializeField] Sprite block_Jewel_Diamond_4;

    [SerializeField] Sprite block_Rock_0;
    [SerializeField] Sprite block_Rock_1;
    [SerializeField] Sprite block_Rock_2;
    [SerializeField] Sprite block_Rock_3;
    [SerializeField] Sprite block_Rock_4;

    [SerializeField] Sprite block_Sand_0;
    [SerializeField] Sprite block_Sand_1;
    [SerializeField] Sprite block_Sand_2;
    [SerializeField] Sprite block_Sand_3;
    [SerializeField] Sprite block_Sand_4;

    [SerializeField] Sprite block_unbreakable_0;

    [SerializeField] Sprite boxCloseSprite;
    [SerializeField] Sprite boxOpenSprite;



    public int nowBlockType = 0; //�ٸ� �ڵ忡�� blockChange�� ���������� ���� blockType �������� ������ ������ �ٸ� �ڵ忡�� blockChange�� ȣ���԰� ���ÿ� �̸� ���� blockType ���� �ٲ��� Ȯ���� ����
    public int blockType = 0; 
    public float blockHealth = 3;
    float blockMaxHealth = 3;

    public bool isGroundSurface = false;

    public int stageNum = 0;

    [SerializeField] SpriteRenderer spriteRenderer;

    public BlocksDictionary blocksDictionary;   //BlocksDictionary���� Chunk�� ȣ���ϴ� AppendBlocksDictionary���� �Ҵ�� ����
    public BlockBreakingEffectManager effectManager;

    Vector2 targetDropPosition;
    bool isDropping;
    float currentDropSpeed = 0f;
    float dropSpeed = 0.1f;
    float maxDropSpeed = 0.5f;

    bool canOpenBox = false;
    GameObject getPlayer;
    bool boxOpen = false;
    float boxDestroyCount = 2f;

    Player player;
    PlayerController playerController;
    

    private void Awake()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        player = Player.FindAnyObjectByType<Player>();
        playerController = PlayerController.FindAnyObjectByType<PlayerController>();
    }

    void ItemDrop(int itemType, int itemCode, Player playerScript, int addEA) //itemType 0�� ����, 1�� ����
    {
        GameObject newDropItem = Instantiate(dropItem, this.transform.position, Quaternion.identity);

        DropItem dropItemScript = newDropItem.GetComponent<DropItem>();

        Sprite dropItemSprite;

        if (itemType == 0)
        {
            dropItemSprite = playerScript.items[itemCode].itemImage;
        }
        else
        {
            dropItemSprite = playerScript.minerals[itemCode].itemImage;
        }

        dropItemScript.setDropItem(itemType, itemCode, dropItemSprite, addEA);
    }
    public void ChangeBlock(int newBlockType)   //�� ��ü ���
    {
        //-2�� ���̺굥���Ϳ� �ȵ� ������,-1�� ������, 0�� normal, 1�� ��������, 2�� ��ź, 3�� �ܴ��ѹ���, 4�� ����, 5�� ����, 6�� ��, 7�� ����, 8�� ö(��), 9�� ��, 10�� ���, 11�� ���̾�
        if (blockType != 0)
            Debug.Log(transform.position + " : �߸��� ȣ��");
        if (blockType != -2)
        {
            blockType = newBlockType;
            if (newBlockType == -1 || newBlockType == -2)
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
                spriteRenderer.sprite = boxCloseSprite;     //�������� ���;� ��
                BoxCollider2D boxCollider = this.gameObject.GetComponent<BoxCollider2D>();
                if (boxCollider != null)
                {
                    boxCollider.isTrigger = true;
                }
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
                spriteRenderer.sprite = block_Normal_0;
                actvieRelicEffect = Instantiate(relicEffect, this.transform);
                blockHealth = 3;
                blockMaxHealth = 3;
            }
            else if (newBlockType == 5)
            {
                spriteRenderer.sprite = block_Normal_1; //�Ϲݺ�ó�� ���̰� ��
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
        else
        {
            print("���� ���� ȣ������ Ȯ���� �ʿ�");
        }
    }

    public void BlockDestroy(float blockDamage, Player playerScript) //*�÷��̾� ��Ʈ�ѷ����� Player�� �߰����ָ�
    {
        if (blockType != 1)
        {
            effectManager.CallBreakingEffect(this.gameObject, blockType);
            if (blockHealth - blockDamage > 0)  //���� ����� �ֱ�
            {
                if(blockType >= 0)
                    blockHealth -= blockDamage;

                if (blockHealth < blockMaxHealth && blockHealth >= blockMaxHealth / 4 * 3) //�� ü���� 3���� 1 ������ ��
                {
                    if (blockType == 0 || blockType == 4 ||blockType == 5) //�Ϲݺ� or ������ or���� ��
                    {
                        spriteRenderer.sprite = block_Normal_1;
                    }
                    else if (blockType == 2) //��ź
                    {
                        spriteRenderer.sprite = block_Jewel_Coal_1;
                    }
                    else if (blockType == 3) //�ܴ��� ����
                    {
                        spriteRenderer.sprite = block_Rock_1;
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
                else if (blockHealth < blockMaxHealth / 4 * 3 && blockHealth >= blockMaxHealth / 2) //�� ü���� 3���� 1 ������ ��
                {
                    if (blockType == 0 || blockType == 4 || blockType == 5) //�Ϲݺ� or ������ or���� ��
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
                    else if (blockType == 9) //��
                    {
                        spriteRenderer.sprite = block_Jewel_Gold_2;
                    }
                    else if (blockType == 10) //���
                    {
                        spriteRenderer.sprite = block_Jewel_Ruby_2;
                    }
                    else if (blockType == 11) //���̾�
                    {
                        spriteRenderer.sprite = block_Jewel_Diamond_2;
                    }
                }
                else if (blockHealth < blockMaxHealth / 2 && blockHealth >= blockMaxHealth / 4) //�� ü���� 3���� 1 ������ ��
                {
                    if (blockType == 0 || blockType == 4 || blockType == 5) //�Ϲݺ� or ������ or���� ��
                    {
                        spriteRenderer.sprite = block_Normal_3;
                    }
                    else if (blockType == 2) //��ź
                    {
                        spriteRenderer.sprite = block_Jewel_Coal_3;
                    }
                    else if (blockType == 3) //�ܴ��� ����
                    {
                        spriteRenderer.sprite = block_Rock_3;
                    }
                    else if (blockType == 6) //��
                    {
                        spriteRenderer.sprite = block_Sand_3;
                    }
                    else if (blockType == 7) //����
                    {
                        spriteRenderer.sprite = block_Jewel_Copper_3;
                    }
                    else if (blockType == 8) //ö
                    {
                        spriteRenderer.sprite = block_Jewel_Iron_3;
                    }
                    else if (blockType == 9) //��
                    {
                        spriteRenderer.sprite = block_Jewel_Gold_3;
                    }
                    else if (blockType == 10) //���
                    {
                        spriteRenderer.sprite = block_Jewel_Ruby_3;
                    }
                    else if (blockType == 11) //���̾�
                    {
                        spriteRenderer.sprite = block_Jewel_Diamond_3;
                    }
                }
                else if (blockHealth < blockMaxHealth / 4 && blockHealth > 0) //�� ü���� 3���� 1 ������ ��
                {
                    if (blockType == 0 || blockType == 4 || blockType == 5) //�Ϲݺ� or ������ or���� ��
                    {
                        spriteRenderer.sprite = block_Normal_4;
                    }
                    else if (blockType == 2) //��ź
                    {
                        spriteRenderer.sprite = block_Jewel_Coal_4;
                    }
                    else if (blockType == 3) //�ܴ��� ����
                    {
                        spriteRenderer.sprite = block_Rock_4;
                    }
                    else if (blockType == 6) //��
                    {
                        spriteRenderer.sprite = block_Sand_4;
                    }
                    else if (blockType == 7) //����
                    {
                        spriteRenderer.sprite = block_Jewel_Copper_4;
                    }
                    else if (blockType == 8) //ö
                    {
                        spriteRenderer.sprite = block_Jewel_Iron_4;
                    }
                    else if (blockType == 9) //��
                    {
                        spriteRenderer.sprite = block_Jewel_Gold_4;
                    }
                    else if (blockType == 10) //���
                    {
                        spriteRenderer.sprite = block_Jewel_Ruby_4;
                    }
                    else if (blockType == 11) //���̾�
                    {
                        spriteRenderer.sprite = block_Jewel_Diamond_4;
                    }
                }
            }
            else//�� �ı�
            {
                effectManager.CallBreakEffect(this.gameObject, blockType);
                if (blockType == 0)
                {
                    //�Ϲ� �� �ν��� ��
                    SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[0]);
                }
                else if (blockType == 2)
                {
                    //��ź �ν��� ��
                    SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[1]);
                    ItemDrop(1, 0, playerScript, 1);
                }
                else if (blockType == 3)
                {
                    //�ܴ��� ���� �ν��� ��
                    SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[4]);
                }
                else if (blockType == 4)
                {
                    //���� �ν��� ��
                    SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[2]);
                    if (actvieRelicEffect != null)
                        Destroy(actvieRelicEffect);
                    if(LoadScene.instance.stage_Level == 0)
                    {
                        ItemDrop(0, Random.Range(0, 10), playerScript, 1);
                    }
                    else if(LoadScene.instance.stage_Level == 1)
                    {
                        ItemDrop(0, Random.Range(0, 20), playerScript, 1);

                    }


                }
                else if (blockType == 5)
                {
                    print("���� �� �ı�");
                    //���� �� �ν��� ��
                    SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[24]);
                    Instantiate(monster1, transform.position, Quaternion.identity);
                }
                else if (blockType == 6)
                {
                    // �� �ν��� ��
                    SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[3]);

                }
                else if (blockType == 7)
                {
                    //���� �ν��� ��
                    SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[1]);
                    ItemDrop(1, 1, playerScript, 1);
                }
                else if (blockType == 8)
                {
                    //ö �ν��� ��
                    SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[1]);
                    ItemDrop(1, 2, playerScript, 1);
                }
                else if (blockType == 9)
                {
                    //�� �ν��� ��
                    SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[1]);
                    ItemDrop(1, 3, playerScript, 1);
                }
                else if (blockType == 10)
                {
                    //��� �ν��� ��
                    SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[1]);
                    ItemDrop(1, 4, playerScript, 1);
                }
                else if (blockType == 11)
                {
                    //���̾� �ν��� ��
                    SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[1]);
                    ItemDrop(1, 5, playerScript, 1);
                }

                blocksDictionary.DestroyBlock(this.gameObject);
                if (isGroundSurface)
                {
                    blocksDictionary.GroundSurfaceChange();
                }

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
    }


    public void BlockReload()
    {
        blocksDictionary.blockPosition.Remove(this.transform.position);

        Destroy(this.gameObject);
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
        blocksDictionary.DestroyBlock(this.gameObject); //�� ���� ���� �������� �ϴ� ��ųʸ����� ���Ƿ� ����

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
                blocksDictionary.DropSandBlock(this.gameObject, this.transform.position); 
            }
            else
            {
                this.transform.position = movePosition;
            }

            
        }

        //�������ڿ��� �÷��̾ �־����µ��� canOpenBox�� true�� ��� ����
        if (blockType == 1 && canOpenBox == true && Vector2.Distance(this.gameObject.transform.position, getPlayer.transform.position) > 2.5f) 
        {
            canOpenBox = false;
        }

        //�������� ��ó�� �÷��̾ �ִ� ���¿��� FŰ(��ȣ�ۿ�Ű)�� ������ �������� ����
        if (blockType == 1 && boxOpen == false && canOpenBox == true && Input.GetKeyDown(KeyCode.F))
        {
            SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[26]);
            
                Player playerScript = getPlayer.GetComponent<Player>();
            if (LoadScene.instance.stage_Level == 0)
            {
                int randCoal = Random.Range(1, 11);
                int randCopper = Random.Range(1, 11);

                for (int i = 0; i < randCoal; i++)
                {
                    ItemDrop(1, 0, getPlayer.GetComponent<Player>(), 1);
                }

                for (int i = 0; i < randCopper; i++)
                {
                    ItemDrop(1, 1, playerScript.GetComponent<Player>(), 1);
                }
            }
            else if (LoadScene.instance.stage_Level == 1)
            {
                int randCoal = Random.Range(1, 6);
                int randCopper = Random.Range(1, 6);
                int randIron = Random.Range(1, 6);

                for (int i = 0; i < randCoal; i++)
                {
                    ItemDrop(1, 0, getPlayer.GetComponent<Player>(), 1);
                }

                for (int i = 0; i < randCopper; i++)
                {
                    ItemDrop(1, 1, playerScript.GetComponent<Player>(), 1);
                }
                for (int i = 0; i < randIron; i++)
                {
                    ItemDrop(1, 2, playerScript.GetComponent<Player>(), 1);
                }

            }

            spriteRenderer.sprite = boxOpenSprite;
            boxOpen = true;
        }

        if(boxOpen == true)
        {
            if (boxDestroyCount - Time.deltaTime > 0)
                boxDestroyCount -= Time.deltaTime;
            else
            {
                blocksDictionary.blockPosition.Remove(this.transform.position);
                Destroy(this.gameObject);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (blockType == 1 && collision.tag == "Player")
        {
            getPlayer = collision.gameObject;
            canOpenBox = true;
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (blockType == 1 && collision.tag == "Player")
        {
            getPlayer = collision.gameObject;
            canOpenBox = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (blockType == 1 && collision.tag == "Player")
        {
            getPlayer = collision.gameObject;
            canOpenBox = false;
        }
    }
}
