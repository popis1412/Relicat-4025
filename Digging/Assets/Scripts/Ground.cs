using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField] BlocksDictionary blocksDictionary;
    [SerializeField] GameObject[] blocks;
    public void AppendBlocksDictionary()
    {
        for (int i = 0; i < blocks.Length; i++)
            blocksDictionary.blockPosition.Add(blocks[i].transform.position, blocks[i]);
    }
}
