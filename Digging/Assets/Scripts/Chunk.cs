using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Chunk : MonoBehaviour
{
    [SerializeField] int stageNum;
    [SerializeField] BlocksDictionary blocksDictionary;
    [SerializeField] GameObject[] blocks;
    [SerializeField] BlockBreakingEffectManager effectManager;

    [SerializeField] int boxCount = 1;       //������ �����ڽ� ����
    [SerializeField] int jewelWithHardBlockCount = 2;//������ ����+�ܴ��Ѻ� ����
    [SerializeField] int jewelCount = 8;    //������ ���� ����
    [SerializeField] int sandCount = 10; // ������ �� ����
    [SerializeField] int relicBlockCount = 2;   //������ ������ ����
    [SerializeField] int hardBlockCount = 7;    //������ �ܴ��Ѻ� ����
    [SerializeField] int unbreakableCount = 3;  //������ �Ⱥμ����º� ����
    [SerializeField] int monsterBlockCount = 3;  //������ ���ͺ� ����


    public void AppendBlocksDictionary()
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            Block blockScript = blocks[i].GetComponent<Block>();
            blocksDictionary.blockPosition.Add(blocks[i].transform.position, blocks[i]);
            blockScript.stageNum = LoadScene.instance.stage_Level;
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

        stageNum = LoadScene.instance.stage_Level;

        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            boxCount = 0;       //������ �����ڽ� ����
            jewelWithHardBlockCount = 0;//������ ����+�ܴ��Ѻ� ����
            jewelCount = 0;    //������ ���� ����
            sandCount = 3; // ������ �� ����
            relicBlockCount = 0;   //������ ������ ����
            hardBlockCount = 3;    //������ �ܴ��Ѻ� ����
            unbreakableCount = 0;  //������ �Ⱥμ����º� ����
            monsterBlockCount = 0;  //������ ���ͺ� ����
        }
        else if (LoadScene.instance.difficulty_level == 0)
        {
            boxCount = 2;       //������ �����ڽ� ����
            jewelWithHardBlockCount = 1;//������ ����+�ܴ��Ѻ� ����
            jewelCount = 10;    //������ ���� ����
            sandCount = 5; // ������ �� ����
            relicBlockCount = 3;   //������ ������ ����
            hardBlockCount = 7;    //������ �ܴ��Ѻ� ����
            unbreakableCount = 3;  //������ �Ⱥμ����º� ����
            monsterBlockCount = 0;  //������ ���ͺ� ����
        }
        else if (LoadScene.instance.difficulty_level == 1)
        {
            boxCount = 1;       //������ �����ڽ� ����
            jewelWithHardBlockCount = 2;//������ ����+�ܴ��Ѻ� ����
            jewelCount = 8;    //������ ���� ����
            sandCount = 10; // ������ �� ����
            relicBlockCount = 2;   //������ ������ ����
            hardBlockCount = 7;    //������ �ܴ��Ѻ� ����
            unbreakableCount = 3;  //������ �Ⱥμ����º� ����
            monsterBlockCount = 1;  //������ ���ͺ� ����
        }
        else if (LoadScene.instance.difficulty_level == 2)
        {
            boxCount = 1;       //������ �����ڽ� ����
            jewelWithHardBlockCount = 2;//������ ����+�ܴ��Ѻ� ����
            jewelCount = 5;    //������ ���� ����
            sandCount = 10; // ������ �� ����
            relicBlockCount = 1;   //������ ������ ����
            hardBlockCount = 7;    //������ �ܴ��Ѻ� ����
            unbreakableCount = 3;  //������ �Ⱥμ����º� ����
            monsterBlockCount = 2;  //������ ���ͺ� ����
        }
        else
        {
            Debug.Log("���̵��� �������� �ʾҽ��ϴ�.");
        }


        List<int> numbers = new List<int>();    //���� ���� ����

        for (int i = 0; i < blocks.Length; i++)
        {
            numbers.Add(i);

        }

        for (int i = 0; i < numbers.Count; i++) //���� ���� �ڼ���
        {
            int randomIndex = Random.Range(i, numbers.Count);
            int temp = numbers[i];
            numbers[i] = numbers[randomIndex];
            numbers[randomIndex] = temp;
        }

        int blockChangeCount = 0;   //�ٲ� �� ����

        //�������� ���� ����
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
        //�������� ���� ��


        //����(��ź)+�ܴ��Ѻ� ���� ����
        int jewelWithHardBlockIndex = blockChangeCount + jewelWithHardBlockCount;
        for (int i = blockChangeCount; i < jewelWithHardBlockIndex; i++)
        {
            blockChangeCount++;
            if (blocks[numbers[i]].GetComponent<Block>().nowBlockType == 0)
            {

                int persentage = Random.Range(1, 11);    //1~10���� ����

                if (stageNum == 0)
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
                else if(stageNum == 1)
                {
                    if (persentage <= 3)
                    {
                        blocks[numbers[i]].GetComponent<Block>().nowBlockType = 2;
                        blocks[numbers[i]].GetComponent<Block>().ChangeBlock(2);
                    }
                    else if(persentage <= 6)
                    {
                        blocks[numbers[i]].GetComponent<Block>().nowBlockType = 7;
                        blocks[numbers[i]].GetComponent<Block>().ChangeBlock(7);
                    }
                    else
                    {
                        blocks[numbers[i]].GetComponent<Block>().nowBlockType = 8;
                        blocks[numbers[i]].GetComponent<Block>().ChangeBlock(8);
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
        //����(��ź)+�ܴ��Ѻ� ���� ��

        //���� ���� ����
        for (int i = 0; i < jewelCount; i++)
        {
            if (stageNum == 0)
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
            else if(stageNum == 1)
            {
                int persentage = Random.Range(1, 11);
                if (persentage <= 3)
                {
                    generateBlock(numbers, blockChangeCount, 1, 2);
                }
                else if(persentage <= 6)
                {
                    generateBlock(numbers, blockChangeCount, 1, 7);
                }
                else
                {
                    generateBlock(numbers, blockChangeCount, 1, 8);
                }
            }
        }
        //���� ���� ��

        //�� ���� ����
        generateBlock(numbers, blockChangeCount, sandCount, 6);
        //�� ���� ��

        //���� ���� ����
        generateBlock(numbers, blockChangeCount, relicBlockCount, 4);
        //���� ���� ��

        //�ܴ��Ѻ� ���� ����
        generateBlock(numbers, blockChangeCount, hardBlockCount, 3);
        //�ܴ��Ѻ� ���� ��

        //�Ⱥμ����º� ���� ����
        generateBlock(numbers, blockChangeCount, unbreakableCount, -1);
        //�Ⱥμ����º� ���� ��

        //���ͺ� ���� ����
        generateBlock(numbers, blockChangeCount, monsterBlockCount, 5);
        //���ͺ� ���� ��
    }



}
