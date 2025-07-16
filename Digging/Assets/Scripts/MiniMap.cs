using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    [SerializeField] int stageNum;
    public MapImage mapImage;

    void Start()
    {
        mapImage.SetTextureSize(stageNum);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
