using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] int stageNum;
    [SerializeField] BlocksDictionary blocksDictionary;
    [SerializeField] GameObject[] blocks;
    [SerializeField] BlockBreakingEffectManager effectManager;

    [SerializeField] int boxCount = 1;       //생성할 랜덤박스 개수
    [SerializeField] int jewelWithHardBlockCount = 2;//생성할 보석+단단한블럭 개수
    [SerializeField] int jewelCount = 8;    //생성할 보석 개수
    [SerializeField] int sandCount = 10; // 생성할 모래 개수
    [SerializeField] int relicBlockCount = 1;   //생성할 유물블럭 개수
    [SerializeField] int hardBlockCount = 7;    //생성할 단단한블럭 개수
    [SerializeField] int unbreakableCount = 3;  //생성할 안부서지는블럭 개수
    [SerializeField] int monsterBlockCount = 1;  //생성할 안부서지는블럭 개수


    public void AppendBlocksDictionary()
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            Block blockScript = blocks[i].GetComponent<Block>();
            blocksDictionary.blockPosition.Add(blocks[i].transform.position, blocks[i]);
            blockScript.stageNum = stageNum;
            blockScript.blocksDictionary = blocksDictionary;
            blockScript.effectManager = effectManager;
        }
    }

    void CallChangeBlock(GameObject block, int changeBlockType)
    {
        block.GetComponent<Block>().nowBlockType = changeBlockType;
        block.GetComponent<Block>().ChangeBlock(changeBlockType);
    }

    void generateBlock(List<int> randNums, int blockChangeCount, int blockCount, int generateBlockCode)
    {
        int blockIndex = blockChangeCount + blockCount;
        for (int i = blockChangeCount; i < blockIndex; i++)
        {
            blockChangeCount++;
            if (blocks[randNums[i]].GetComponent<Block>().nowBlockType != 0)
            {
                blockIndex++;
            }
            else
            {
                CallChangeBlock(blocks[randNums[i]], generateBlockCode);
            }
        }
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
        for (int i = blockChangeCount; i < boxIndex; i++)
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


        //보석(석탄)+단단한블럭 생성 시작
        int jewelWithHardBlockIndex = blockChangeCount + jewelWithHardBlockCount;
        for (int i = blockChangeCount; i < jewelWithHardBlockIndex; i++)
        {
            blockChangeCount++;
            if (blocks[numbers[i]].GetComponent<Block>().nowBlockType == 0)
            {

                int persentage = Random.Range(1, 11);    //1~10까지 랜덤

                if (stageNum == 1)
                {
                    if (persentage <= 5)
                    {
                        blocks[numbers[i]].GetComponent<Block>().nowBlockType = 2;
                        blocks[numbers[i]].GetComponent<Block>().ChangeBlock(2);
                    }
                    else
                    {
                        blocks[numbers[i]].GetComponent<Block>().nowBlockType = 7;
                        blocks[numbers[i]].GetComponent<Block>().ChangeBlock(7);
                    }
                }


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
                jewelWithHardBlockIndex++;
            }


        }
        //보석(석탄)+단단한블럭 생성 끝

        //보석 생성 시작
        for (int i = 0; i < jewelCount; i++)
        {
            if (stageNum == 1)
            {
                int persentage = Random.Range(1, 11);
                if (persentage <= 5)
                {
                    generateBlock(numbers, blockChangeCount, 1, 2);
                }
                else
                {
                    generateBlock(numbers, blockChangeCount, 1, 7);
                }
            }
        }
        //보석 생성 끝

        //모래 생성 시작
        generateBlock(numbers, blockChangeCount, sandCount, 6);
        //모래 생성 끝

        //유물 생성 시작
        generateBlock(numbers, blockChangeCount, relicBlockCount, 4);
        //유물 생성 끝

        //단단한블럭 생성 시작
        generateBlock(numbers, blockChangeCount, hardBlockCount, 3);
        //단단한블럭 생성 끝

        //안부서지는블럭 생성 시작
        generateBlock(numbers, blockChangeCount, unbreakableCount, -1);
        //안부서지는블럭 생성 끝

        //몬스터블럭 생성 시작
        generateBlock(numbers, blockChangeCount, monsterBlockCount, 5);
        //몬스터블럭 생성 끝
    }



}
