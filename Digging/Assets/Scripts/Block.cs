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



    public int nowBlockType = 0; //다른 코드에서 blockChange를 실행했지만 실제 blockType 변동까지 느리기 때문에 다른 코드에서 blockChange를 호출함과 동시에 미리 무슨 blockType 으로 바뀔지 확인할 변수
    public int blockType = 0; 
    public float blockHealth = 3;
    float blockMaxHealth = 3;

    public bool isGroundSurface = false;

    public int stageNum = 0;

    [SerializeField] SpriteRenderer spriteRenderer;

    public BlocksDictionary blocksDictionary;   //BlocksDictionary에서 Chunk에 호출하는 AppendBlocksDictionary에서 할당될 예정
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

    void ItemDrop(int itemType, int itemCode, Player playerScript, int addEA) //itemType 0은 유물, 1은 광물
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
    public void ChangeBlock(int newBlockType)   //블럭 교체 명령
    {
        //-2는 세이브데이터에 안들어갈 무적블럭,-1은 무적블럭, 0은 normal, 1은 보물상자, 2는 석탄, 3은 단단한바위, 4는 유물, 5는 몬스터, 6은 모래, 7은 구리, 8은 철(은), 9는 금, 10은 루비, 11은 다이아
        if (blockType != 0)
            Debug.Log(transform.position + " : 잘못된 호출");
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
                spriteRenderer.sprite = boxCloseSprite;     //보물상자 들어와야 함
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
                spriteRenderer.sprite = block_Normal_1; //일반블럭처럼 보이게 함
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
            print("벽에 대한 호출인지 확인이 필요");
        }
    }

    public void BlockDestroy(float blockDamage, Player playerScript) //*플레이어 컨트롤러에서 Player를 추가해주면
    {
        if (blockType != 1)
        {
            effectManager.CallBreakingEffect(this.gameObject);
            if (blockHealth - blockDamage > 0)  //블럭에 대미지 주기
            {
                blockHealth -= blockDamage;

                if (blockHealth < blockMaxHealth && blockHealth >= blockMaxHealth / 4 * 3) //블럭 체력이 3분의 1 이하일 때
                {
                    if (blockType == 0 || blockType == 4 ||blockType == 5) //일반블럭 or 유물블럭 or몬스터 블럭
                    {
                        spriteRenderer.sprite = block_Normal_1;
                    }
                    else if (blockType == 2) //석탄
                    {
                        spriteRenderer.sprite = block_Jewel_Coal_1;
                    }
                    else if (blockType == 3) //단단한 바위
                    {
                        spriteRenderer.sprite = block_Rock_1;
                    }
                    else if (blockType == 6) //모래
                    {
                        spriteRenderer.sprite = block_Sand_1;
                    }
                    else if (blockType == 7) //구리
                    {
                        spriteRenderer.sprite = block_Jewel_Copper_1;
                    }
                    else if (blockType == 8) //철
                    {
                        spriteRenderer.sprite = block_Jewel_Iron_1;
                    }
                    else if (blockType == 9) //금
                    {
                        spriteRenderer.sprite = block_Jewel_Gold_1;
                    }
                    else if (blockType == 10) //루비
                    {
                        spriteRenderer.sprite = block_Jewel_Ruby_1;
                    }
                    else if (blockType == 11) //다이아
                    {
                        spriteRenderer.sprite = block_Jewel_Diamond_1;
                    }
                }
                else if (blockHealth < blockMaxHealth / 4 * 3 && blockHealth >= blockMaxHealth / 2) //블럭 체력이 3분의 1 이하일 때
                {
                    if (blockType == 0 || blockType == 4 || blockType == 5) //일반블럭 or 유물블럭 or몬스터 블럭
                    {
                        spriteRenderer.sprite = block_Normal_2;
                    }
                    else if (blockType == 2) //석탄
                    {
                        spriteRenderer.sprite = block_Jewel_Coal_2;
                    }
                    else if (blockType == 3) //단단한 바위
                    {
                        spriteRenderer.sprite = block_Rock_2;
                    }
                    else if (blockType == 6) //모래
                    {
                        spriteRenderer.sprite = block_Sand_2;
                    }
                    else if (blockType == 7) //구리
                    {
                        spriteRenderer.sprite = block_Jewel_Copper_2;
                    }
                    else if (blockType == 8) //철
                    {
                        spriteRenderer.sprite = block_Jewel_Iron_2;
                    }
                    else if (blockType == 9) //금
                    {
                        spriteRenderer.sprite = block_Jewel_Gold_2;
                    }
                    else if (blockType == 10) //루비
                    {
                        spriteRenderer.sprite = block_Jewel_Ruby_2;
                    }
                    else if (blockType == 11) //다이아
                    {
                        spriteRenderer.sprite = block_Jewel_Diamond_2;
                    }
                }
                else if (blockHealth < blockMaxHealth / 2 && blockHealth >= blockMaxHealth / 4) //블럭 체력이 3분의 1 이하일 때
                {
                    if (blockType == 0 || blockType == 4 || blockType == 5) //일반블럭 or 유물블럭 or몬스터 블럭
                    {
                        spriteRenderer.sprite = block_Normal_3;
                    }
                    else if (blockType == 2) //석탄
                    {
                        spriteRenderer.sprite = block_Jewel_Coal_3;
                    }
                    else if (blockType == 3) //단단한 바위
                    {
                        spriteRenderer.sprite = block_Rock_3;
                    }
                    else if (blockType == 6) //모래
                    {
                        spriteRenderer.sprite = block_Sand_3;
                    }
                    else if (blockType == 7) //구리
                    {
                        spriteRenderer.sprite = block_Jewel_Copper_3;
                    }
                    else if (blockType == 8) //철
                    {
                        spriteRenderer.sprite = block_Jewel_Iron_3;
                    }
                    else if (blockType == 9) //금
                    {
                        spriteRenderer.sprite = block_Jewel_Gold_3;
                    }
                    else if (blockType == 10) //루비
                    {
                        spriteRenderer.sprite = block_Jewel_Ruby_3;
                    }
                    else if (blockType == 11) //다이아
                    {
                        spriteRenderer.sprite = block_Jewel_Diamond_3;
                    }
                }
                else if (blockHealth < blockMaxHealth / 4 && blockHealth > 0) //블럭 체력이 3분의 1 이하일 때
                {
                    if (blockType == 0 || blockType == 4 || blockType == 5) //일반블럭 or 유물블럭 or몬스터 블럭
                    {
                        spriteRenderer.sprite = block_Normal_4;
                    }
                    else if (blockType == 2) //석탄
                    {
                        spriteRenderer.sprite = block_Jewel_Coal_4;
                    }
                    else if (blockType == 3) //단단한 바위
                    {
                        spriteRenderer.sprite = block_Rock_4;
                    }
                    else if (blockType == 6) //모래
                    {
                        spriteRenderer.sprite = block_Sand_4;
                    }
                    else if (blockType == 7) //구리
                    {
                        spriteRenderer.sprite = block_Jewel_Copper_4;
                    }
                    else if (blockType == 8) //철
                    {
                        spriteRenderer.sprite = block_Jewel_Iron_4;
                    }
                    else if (blockType == 9) //금
                    {
                        spriteRenderer.sprite = block_Jewel_Gold_4;
                    }
                    else if (blockType == 10) //루비
                    {
                        spriteRenderer.sprite = block_Jewel_Ruby_4;
                    }
                    else if (blockType == 11) //다이아
                    {
                        spriteRenderer.sprite = block_Jewel_Diamond_4;
                    }
                }
            }
            else//블럭 파괴
            {
                effectManager.CallBreakEffect(this.gameObject);
                if (blockType == 0)
                {
                    //일반 블럭 부쉈을 때
                    SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[0]);
                }
                else if (blockType == 2)
                {
                    //석탄 부쉈을 때
                    SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[1]);
                    ItemDrop(1, 0, playerScript, 1);
                }
                else if (blockType == 3)
                {
                    //단단한 바위 부쉈을 때
                    SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[4]);
                }
                else if (blockType == 4)
                {
                    //유물 부쉈을 때
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
                    print("몬스터 블럭 파괴");
                    //몬스터 블럭 부쉈을 때
                    SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[24]);
                    Instantiate(monster1, transform.position, Quaternion.identity);
                }
                else if (blockType == 6)
                {
                    // 모래 부쉈을 때
                    SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[3]);

                }
                else if (blockType == 7)
                {
                    //구리 부쉈을 때
                    SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[1]);
                    ItemDrop(1, 1, playerScript, 1);
                }
                else if (blockType == 8)
                {
                    //철 부쉈을 때
                    SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[1]);
                    ItemDrop(1, 2, playerScript, 1);
                }
                else if (blockType == 9)
                {
                    //금 부쉈을 때
                    SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[1]);
                    ItemDrop(1, 3, playerScript, 1);
                }
                else if (blockType == 10)
                {
                    //루비 부쉈을 때
                    SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[1]);
                    ItemDrop(1, 4, playerScript, 1);
                }
                else if (blockType == 11)
                {
                    //다이아 부쉈을 때
                    SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[1]);
                    ItemDrop(1, 5, playerScript, 1);
                }

                blocksDictionary.blockPosition.Remove(this.transform.position);
                if (isGroundSurface)
                {
                    blocksDictionary.GroundSurfaceChange();
                }

                if (blocksDictionary.blockPosition.ContainsKey((Vector2)this.transform.position + new Vector2(0, 1))) //내 위에 블럭 있는지 체크
                {
                    GameObject aboveBlock = blocksDictionary.blockPosition[(Vector2)transform.position + new Vector2(0, 1)]; ;
                    if (aboveBlock.GetComponent<Block>().blockType == 6) //내 위에 있는 블럭이 모래면 떨어트리기
                    {
                        aboveBlock.GetComponent<Block>().DropBlock(aboveBlock.GetComponent<Block>().DropCheck(this.transform.position, 0));
                    }
                }

                Destroy(this.gameObject);//무조건 이 if문에서 맨 마지막으로
                
            }
        }
    }


    public void BlockReload()
    {
        blocksDictionary.blockPosition.Remove(this.transform.position);

        Destroy(this.gameObject);
    }

    public int DropCheck(Vector2 dropCheckPosition, int checkDropHeight) //모래 얼마나 떨어질지 계산(재귀함수)
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

    public void DropBlock(int dropHeight) //모래 떨어트리기
    {
        if (blocksDictionary.blockPosition.ContainsKey((Vector2)this.transform.position + new Vector2(0, 1))) //내 위에 블럭있는지 체크
        {
            GameObject aboveBlock = blocksDictionary.blockPosition[(Vector2)transform.position + new Vector2(0, 1)]; ;
            if (aboveBlock.GetComponent<Block>().blockType == 6) //내 위에 있는 블럭도 모래면 같이 떨어지기
            {
                aboveBlock.GetComponent<Block>().DropBlock(dropHeight);
            }
        }

        targetDropPosition = (Vector2)this.transform.position - new Vector2(0, dropHeight); //어디까지 떨어져야하는지 확인
        blocksDictionary.blockPosition.Remove(this.transform.position); //이 블럭은 이제 움직여야 하니 딕셔너리에서 임의로 제거

        isDropping = true; //떨어트리는게 눈에 보여야 하기 때문에 떨어트리라는 변수를 체크하고 Update()가 알아서 떨어트리게 함
    }


    private void Update()
    {
        if (isDropping) //떨어져야함
        {
            if (currentDropSpeed + dropSpeed * Time.deltaTime > maxDropSpeed)   //속도 계산
                currentDropSpeed = maxDropSpeed;
            else currentDropSpeed += dropSpeed * Time.deltaTime;

            Vector2 movePosition = new Vector2(this.transform.position.x, this.transform.position.y - currentDropSpeed); //떨어질 위치

            if (movePosition.y < targetDropPosition.y) //떨어질 위치가 목표 위치보다 아래면 더이상 떨어지게 하지 않고, 속도 초기화하고, 딕셔너리에 추가
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

        //보물상자에서 플레이어가 멀어졌는데도 canOpenBox가 true인 경우 해제
        if (blockType == 1 && canOpenBox == true && Vector2.Distance(this.gameObject.transform.position, getPlayer.transform.position) > 2.5f) 
        {
            canOpenBox = false;
        }

        //보물상자 근처에 플레이어가 있는 상태에서 F키(상호작용키)를 누를시 보물상자 해제
        if(blockType == 1 && canOpenBox == true && Input.GetKeyDown(KeyCode.F))
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
