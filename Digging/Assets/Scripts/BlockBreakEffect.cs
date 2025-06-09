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
}
