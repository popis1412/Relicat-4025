using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using static UnityEditor.PlayerSettings;
#endif


public class BlockBreakEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem ps1;
    [SerializeField] ParticleSystem ps2;

    public bool isPlay = false;

    public GameObject callingBlock;

    int nowColorType = 0;

    public void ParticlePlay()
    {
        ps1.Play();
        ps2.Play();
    }
    // Update is called once per frame
    void Update()
    {

        if ((!ps1.isPlaying && ps1.particleCount == 0) && (!ps2.isPlaying && ps2.particleCount == 0))
        {
            isPlay = false;
            this.gameObject.SetActive(false);
        }
    }

    public void changeColor(int blockType)
    {
        if (nowColorType != blockType)
        {
            nowColorType = blockType;
            if (blockType == 3) //단단한 바위
            {
                Color color = new Color(128f / 255f, 128f / 255f, 128f / 255f, 1f);
                var main = ps1.main;
                main.startColor = color;
                main = ps2.main;
                main.startColor = color;
            }
            else if (blockType == 6) //모래
            {
                Color color = new Color(1f, 204f / 255f, 153f / 255f, 1f);
                var main = ps1.main;
                main.startColor = color;
                main = ps2.main;
                main.startColor = color;
            }
            else if (blockType == -1 || blockType == -2) //안부서지는거
            {
                Color color = new Color(0f, 0f, 0f, 1f);
                var main = ps1.main;
                main.startColor = color;
                main = ps2.main;
                main.startColor = color;
            }
            else
            {
                Color color = new Color(164f / 255f, 111f / 255f, 4f / 255f, 1f);
                var main = ps1.main;
                main.startColor = color;
                main = ps2.main;
                main.startColor = color;
            }
        }
    }

}
