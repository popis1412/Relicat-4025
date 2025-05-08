using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] BlocksDictionary blocksDictionary;
    [SerializeField] GameObject[] blocks;

    [SerializeField] int boxCount = 3;       //생성할 랜덤박스 개수
    [SerializeField] int jewelCount = 10;    //생성할 보석 개수

    [SerializeField] GameObject player;

    public void AppendBlocksDictionary()
    {
        for (int i = 0; i < blocks.Length; i++)
            blocksDictionary.blockPosition.Add(blocks[i].transform.position, blocks[i]);
    }

    void CallChangeBlock(GameObject block, int changeBlockType)
    {
        block.GetComponent<Block>().nowBlockType = changeBlockType;
        block.GetComponent<Block>().ChangeBlock(changeBlockType);
    }
    void Start()
    {
        List<int> numbers = new List<int>();    //랜덤 수열 선언

        for (int i = 0; i < blocks.Length; i++)
        {
            numbers.Add(i);

        }

        for (int i = 0; i < numbers.Count; i++) //랜덤 수열 뒤섞기
        {
            int randomIndex = Random.Range(i, numbers.Count);
            int temp = numbers[i];
            numbers[i] = numbers[randomIndex];
            numbers[randomIndex] = temp;
        }

        int blockChangeCount = 0;   //바꾼 블럭 개수

        int boxIndex = blockChangeCount + boxCount;
        for (int i = blockChangeCount; i <  boxIndex; i++)
        {
            blockChangeCount++;
            if (blocks[numbers[i]].GetComponent<Block>().nowBlockType == 0)
            {
                blocks[numbers[i]].GetComponent<Block>().nowBlockType = 1;
                blocks[numbers[i]].GetComponent<Block>().ChangeBlock(1);
                Vector2 xPlus = new Vector2(blocks[numbers[i]].transform.position.x + 1.0f, blocks[numbers[i]].transform.position.y);
                Vector2 xMinus = new Vector2(blocks[numbers[i]].transform.position.x - 1.0f, blocks[numbers[i]].transform.position.y);
                Vector2 yPlus = new Vector2(blocks[numbers[i]].transform.position.x, blocks[numbers[i]].transform.position.y + 1.0f);
                Vector2 yMinus = new Vector2(blocks[numbers[i]].transform.position.x, blocks[numbers[i]].transform.position.y - 1.0f);

                if (blocksDictionary.blockPosition.ContainsKey(xPlus) &&
                    blocksDictionary.blockPosition[xPlus].GetComponent<Block>().nowBlockType == 0)
                {
                    CallChangeBlock(blocksDictionary.blockPosition[xPlus], 3);
                }
                if (blocksDictionary.blockPosition.ContainsKey(xMinus) &&
                    blocksDictionary.blockPosition[xMinus].GetComponent<Block>().nowBlockType == 0)
                {
                    CallChangeBlock(blocksDictionary.blockPosition[xMinus], 3);
                }
                if (blocksDictionary.blockPosition.ContainsKey(yPlus) &&
                    blocksDictionary.blockPosition[yPlus].GetComponent<Block>().nowBlockType == 0)
                {
                    CallChangeBlock(blocksDictionary.blockPosition[yPlus], 3);
                }
                if (blocksDictionary.blockPosition.ContainsKey(yMinus) &&
                    blocksDictionary.blockPosition[yMinus].GetComponent<Block>().nowBlockType == 0)
                {
                    CallChangeBlock(blocksDictionary.blockPosition[yMinus], 3);
                }
            }
            else
            {
                boxIndex++;
            }


        }

        int jewelIndex = blockChangeCount + jewelCount;

        for(int i = blockChangeCount; i < jewelIndex; i++)
        {
            blockChangeCount++;
            if (blocks[numbers[i]].GetComponent<Block>().nowBlockType != 0)
            {
                jewelIndex++;
            }
            else
            {
                CallChangeBlock(blocks[numbers[i]], 2);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(this.transform.position, player.transform.position);

        if(player != null)
        {
            if (this.gameObject.activeSelf && distanceToPlayer > 300)
            {
                this.gameObject.SetActive(false);
            }
            else if (!this.gameObject.activeSelf && distanceToPlayer <= 300)
            {
                this.gameObject.SetActive(true);
            }
        }
    }

}
