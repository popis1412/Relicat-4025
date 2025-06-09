using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField] int stageNum;
    [SerializeField] BlocksDictionary blocksDictionary;
    [SerializeField] GameObject[] blocks;
    [SerializeField] GameObject[] groundBlocks;
    [SerializeField] GameObject[] respawnBlcok;
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

        for (int i = 0; i < respawnBlcok.Length; i++)
        {
            Block blockScript = respawnBlcok[i].GetComponent<Block>();
            blocksDictionary.blockPosition.Add(respawnBlcok[i].transform.position, respawnBlcok[i]);
            blockScript.stageNum = stageNum;
            blockScript.blocksDictionary = blocksDictionary;
            blockScript.effectManager = effectManager;
            blockScript.ChangeBlock(-1);
        }
    }
}
