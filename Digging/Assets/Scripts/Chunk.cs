using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] BlocksDictionary blocksDictionary;
    [SerializeField] GameObject[] blocks;

    [SerializeField] int boxCount = 3;       //생성할 랜덤박스 개수
    [SerializeField] int jewelCount = 10;    //생성할 보석 개수
    [SerializeField] int sandCount = 10; // 생성할 모래 개수
    [SerializeField] int relicBlockCount = 3;   //생성할 유물블럭 개수

    public void AppendBlocksDictionary()
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            blocksDictionary.blockPosition.Add(blocks[i].transform.position, blocks[i]);
            blocks[i].GetComponent<Block>().blocksDictionary = blocksDictionary;
        }
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

        //보물상자 생성 시작
        int boxIndex = blockChangeCount + boxCount;
        for (int i = blockChangeCount; i <  boxIndex; i++)
        {
            blockChangeCount++;
            if (blocks[numbers[i]].GetComponent<Block>().nowBlockType == 0)
            {
                blocks[numbers[i]].GetComponent<Block>().nowBlockType = 1;
                blocks[numbers[i]].GetComponent<Block>().ChangeBlock(1);
                Vector2 xPlus = (Vector2)blocks[numbers[i]].transform.position + new Vector2(1, 0);
                Vector2 xMinus = (Vector2)blocks[numbers[i]].transform.position + new Vector2(-1, 0);
                Vector2 yPlus = (Vector2)blocks[numbers[i]].transform.position + new Vector2(0, 1);
                Vector2 yMinus = (Vector2)blocks[numbers[i]].transform.position + new Vector2(0, -1);

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
        //보물상자 생성 끝

        //보석:석탄 생성 시작
        int coalIndex = blockChangeCount + jewelCount;
        for(int i = blockChangeCount; i < coalIndex; i++)
        {
            blockChangeCount++;
            if (blocks[numbers[i]].GetComponent<Block>().nowBlockType != 0)
            {
                coalIndex++;
            }
            else
            {
                CallChangeBlock(blocks[numbers[i]], 2);
            }
        }
        //보석:석탄 생성 끝

        //모래 생성 시작
        int sandIndex = blockChangeCount + sandCount;
        for (int i = blockChangeCount; i < sandIndex; i++)
        {
            blockChangeCount++;
            if (blocks[numbers[i]].GetComponent<Block>().nowBlockType != 0)
            {
                sandIndex++;
            }
            else
            {
                CallChangeBlock(blocks[numbers[i]], 6);
            }
        }
        //모래 생성 끝

        //유물 생성 시작
        int relicBoxIndex = blockChangeCount + relicBlockCount;
        for (int i = blockChangeCount; i < relicBoxIndex; i++)
        {
            blockChangeCount++;
            if (blocks[numbers[i]].GetComponent<Block>().nowBlockType != 0)
            {
                relicBoxIndex++;
            }
            else
            {
                CallChangeBlock(blocks[numbers[i]], 4);
            }
        }
        //유물 생성 끝
    }

}
