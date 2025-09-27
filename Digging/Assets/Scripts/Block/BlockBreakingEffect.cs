using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBreakingEffect : MonoBehaviour
{

    [SerializeField] ParticleSystem ps1;
    [SerializeField] ParticleSystem ps2;
    [SerializeField] ParticleSystem ps3;

    public bool isPlay = false;

    public GameObject callingBlock;

    float count = 0;

    int nowColorType = 0;

    public void ParticlePlay()
    {
        if(!ps1.isPlaying)
            ps1.Play();
        if(!ps2.isPlaying)
            ps2.Play();
        if(!ps3.isPlaying)
            ps3.Play();

        var main = ps1.main;
        main.loop = true;

        main = ps2.main;
        main.loop = true;

        main = ps3.main;
        main.loop = true;

        count = 0.2f;
        isPlay = true;
    }

    public void ParitcleStop()
    {
        var main = ps1.main;
        main.loop = false;

        main = ps2.main;
        main.loop = false;

        main = ps3.main;
        main.loop = false;

        ps1.Stop();
        ps2.Stop();
        ps3.Stop();
    }

    void Update()
    {
        if(isPlay)
        {
            if (count - Time.deltaTime > 0)
                count -= Time.deltaTime;
            else
            {
                count = 0;
                ParitcleStop();
            }

            if ((!ps1.isPlaying && ps1.particleCount == 0) && (!ps2.isPlaying && ps2.particleCount == 0) && (!ps3.isPlaying && ps3.particleCount == 0))
            {
                isPlay = false;
                this.gameObject.SetActive(false);
            }
        }

    }


    public void changeColor(int blockType)
    {
        if (nowColorType != blockType)
        {
            nowColorType = blockType;
            if(blockType == 3) //단단한 바위
            {
                Color color = new Color(128f / 255f, 128f / 255f, 128f / 255f, 1f);
                var main = ps1.main;
                main.startColor = color;
                main = ps2.main;
                main.startColor = color;
                main = ps3.main;
                main.startColor = color;
            }
            else if (blockType == 6) //모래
            {
                Color color = new Color(1f, 204f / 255f, 153f / 255f, 1f);
                var main = ps1.main;
                main.startColor = color;
                main = ps2.main;
                main.startColor = color;
                main = ps3.main;
                main.startColor = color;
            }
            else if (blockType == -1 || blockType == -2) //안 부서지는거
            {
                Color color = new Color(0f, 0f, 0f, 1f);
                var main = ps1.main;
                main.startColor = color;
                main = ps2.main;
                main.startColor = color;
                main = ps3.main;
                main.startColor = color;
            }
            else
            {
                Color color = new Color(164f / 255f, 111f / 255f, 4f / 255f, 1f);
                var main = ps1.main;
                main.startColor = color;
                main = ps2.main;
                main.startColor = color;
                main = ps3.main;
                main.startColor = color;

            }
        }
    }
}
