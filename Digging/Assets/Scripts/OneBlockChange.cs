using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneBlockChange : MonoBehaviour
{
    Block blockScript;
    [SerializeField] BlocksDictionary blocksDictionary;
    [SerializeField] BlockBreakingEffectManager effectManager;
    [SerializeField] int changeBlockCode = 0;
    void Start()
    {
        blockScript = GetComponent<Block>();
        if (blockScript != null)
        {
            blockScript.nowBlockType = changeBlockCode;
            blockScript.ChangeBlock(changeBlockCode);
            if (blocksDictionary != null)
            {
                blocksDictionary.blockPosition.Add(this.transform.position, this.gameObject);
                blockScript.blocksDictionary = blocksDictionary;
                blockScript.effectManager = effectManager;
            }
        }
        else
            print($"{this.gameObject} is not Block");
    }
}
