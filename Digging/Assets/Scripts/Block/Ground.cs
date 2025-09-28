using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField] int stageNum;
    [SerializeField] BlocksDictionary blocksDictionary;
    [SerializeField] GameObject[] blocks;
    [SerializeField] GameObject[] groundBlocks;
    [SerializeField] GameObject[] entranceBlocks;
    [SerializeField] GameObject[] relicBlocks;
    [SerializeField] GameObject[] monsterBlocks;
    [SerializeField] BlockBreakingEffectManager effectManager;
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

        for (int i = 0; i < groundBlocks.Length; i++)
        {
            Block blockScript = groundBlocks[i].GetComponent<Block>();
            blocksDictionary.blockPosition.Add(groundBlocks[i].transform.position, groundBlocks[i]);
            blockScript.stageNum = stageNum;
            blockScript.blocksDictionary = blocksDictionary;
            blockScript.effectManager = effectManager;
            blockScript.isGroundSurface = true;
        }

        for (int i = 0; i < entranceBlocks.Length; i++)
        {
            Block blockScript = entranceBlocks[i].GetComponent<Block>();
            blocksDictionary.blockPosition.Add(entranceBlocks[i].transform.position, entranceBlocks[i]);
            blockScript.stageNum = stageNum;
            blockScript.blocksDictionary = blocksDictionary;
            blockScript.effectManager = effectManager;
        }

        for (int i = 0; i < relicBlocks.Length; i++)
        {
            Block blockScript = relicBlocks[i].GetComponent<Block>();
            blocksDictionary.blockPosition.Add(relicBlocks[i].transform.position, relicBlocks[i]);
            blockScript.stageNum = stageNum;
            blockScript.blocksDictionary = blocksDictionary;
            blockScript.effectManager = effectManager;
        }

        for (int i = 0; i < monsterBlocks.Length; i++)
        {
            Block blockScript = monsterBlocks[i].GetComponent<Block>();
            blocksDictionary.blockPosition.Add(monsterBlocks[i].transform.position, monsterBlocks[i]);
            blockScript.stageNum = stageNum;
            blockScript.blocksDictionary = blocksDictionary;
            blockScript.effectManager = effectManager;
        }
    }

    private void Start()
    {

        for (int i = 0; i < groundBlocks.Length; i++)
        {
            Block blockScript = groundBlocks[i].GetComponent<Block>();
            blockScript.ChangeBlock(-1);
        }
        for (int i = 0; i < relicBlocks.Length; i++)
        {
            Block blockScript = relicBlocks[i].GetComponent<Block>();
            blockScript.ChangeBlock(4);
        }
        for (int i = 0; i < monsterBlocks.Length; i++)
        {
            Block blockScript = monsterBlocks[i].GetComponent<Block>();
            blockScript.ChangeBlock(5);
        }
    }
}
