using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUnbreakable : MonoBehaviour
{
    [SerializeField] BlocksDictionary blocksDictionary;
    [SerializeField] GameObject[] blocks;
    [SerializeField] BlockBreakingEffectManager effectManager;
    public void AppendBlocksDictionary()
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            Block blockScript = blocks[i].GetComponent<Block>();
            blocksDictionary.blockPosition.Add(blocks[i].transform.position, blocks[i]);
            blockScript.blocksDictionary = blocksDictionary;
            blockScript.effectManager = effectManager;
        }
    }

    private void Start()
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].GetComponent<Block>().nowBlockType = -1;
            blocks[i].GetComponent<Block>().ChangeBlock(-1);
        }
    }
}
