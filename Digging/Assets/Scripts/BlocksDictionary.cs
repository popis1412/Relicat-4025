using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksDictionary : MonoBehaviour
{
    public Dictionary <Vector2, GameObject> blockPosition;
    [SerializeField] GameObject[] chunks;
    [SerializeField] GameObject[] grounds;
    [SerializeField] GameObject[] unbreakables;

    private void Awake()
    {
        blockPosition = new Dictionary<Vector2, GameObject>();
        for (int i = 0; i < chunks.Length; i++)
        {
            chunks[i].GetComponent<Chunk>().AppendBlocksDictionary();
        }
        for (int i = 0; i < grounds.Length; i++)
        {
            grounds[i].GetComponent<Ground>().AppendBlocksDictionary();
        }
        for(int i = 0; i < unbreakables.Length; i++)
        {
            unbreakables[i].GetComponent<SetUnbreakable>().AppendBlocksDictionary();
        }

    }

}
