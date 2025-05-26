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
}
