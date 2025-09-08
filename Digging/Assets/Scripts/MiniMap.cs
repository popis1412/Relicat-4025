using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    [SerializeField] int stageNum;
    [SerializeField] Vector2 leftTopPos = new Vector2(0, 0);
    public MapImage mapImage;

    void Start()
    {
        mapImage.SetTextureSize(stageNum, leftTopPos);
    }

    // Update is called once per frame
    void Update()
    {

    }
}