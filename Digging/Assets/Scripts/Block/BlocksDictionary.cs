using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksDictionary : MonoBehaviour
{
    public Dictionary <Vector2, GameObject> blockPosition;
    [SerializeField] GameObject[] chunks;
    [SerializeField] GameObject[] grounds;
    [SerializeField] GameObject[] unbreakables;
    [SerializeField] MiniMap minimap;
    MapImage mapImage;

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

    private void Start()
    {
        mapImage = minimap.mapImage;
    }

    public void GroundSurfaceChange()
    {
        foreach(GameObject obj in blockPosition.Values)
        {
            Block blockScript = obj.GetComponent<Block>();
            if(blockScript.isGroundSurface)
            {
                blockScript.ChangeBlock(-1);
            }
        }
    }

    public void DestroyBlock(GameObject obj)
    {
        if(blockPosition.ContainsKey(obj.transform.position))
        {
            blockPosition.Remove(obj.transform.position);
        }

        mapImage.DrawSquare(Mathf.RoundToInt(obj.transform.position.x - 0.5f), Mathf.RoundToInt(obj.transform.position.y - 0.5f), Color.yellow);
    }

    public void DropSandBlock(GameObject obj, Vector2 objPos)
    {
        blockPosition.Add(objPos, obj);
        print($"지워주기로 한 좌표는 ({objPos}), 전달한 좌표는 ({Mathf.RoundToInt(objPos.x - 0.5f)},{Mathf.RoundToInt(objPos.y - 0.5f)})");
        mapImage.EraseSquare(Mathf.RoundToInt(objPos.x - 0.5f), Mathf.RoundToInt(objPos.y - 0.5f));
    }

}
