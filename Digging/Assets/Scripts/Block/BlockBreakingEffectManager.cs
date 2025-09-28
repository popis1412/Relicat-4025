using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBreakingEffectManager : MonoBehaviour
{
    [SerializeField] GameObject blockBreakingEffect;
    private List<GameObject> breakingEffectPool = new List<GameObject>();
    private int breakingPoolSize = 5;
    int loopCount = 0;

    [SerializeField] GameObject blockBreakEffect;
    private List<GameObject> breakEffectPool = new List<GameObject>();
    private int breakPoolSize = 5;

    private void Awake()
    {
        breakingEffectPool = new List<GameObject>();

        for (int i = 0; i < breakingPoolSize; i++)
        {
            GameObject obj = Instantiate(blockBreakingEffect);
            obj.SetActive(false);
            breakingEffectPool.Add(obj);
        }

        for (int i = 0; i < breakingPoolSize; i++)
        {
            GameObject obj = Instantiate(blockBreakEffect);
            obj.SetActive(false);
            breakEffectPool.Add(obj);
        }
    }

    public void CallBreakingEffect(GameObject callingBlock, int blockType)
    {
        int callParticleIndex = -1;
        bool isAlreadyCalled = false;
        for(int i = 0; i < breakingEffectPool.Count; i++)
        {
            if (breakingEffectPool[i].GetComponent<BlockBreakingEffect>().callingBlock == callingBlock && breakingEffectPool[i].activeSelf)
            {
                callParticleIndex = i;
                isAlreadyCalled = true;
                break;
            }
        }

        if (isAlreadyCalled == false)
        {
            for(int i = 0; i < breakingEffectPool.Count; i++)
            {
                if(!breakingEffectPool[i].activeSelf)
                {
                    callParticleIndex = i;
                    break;
                }
            }
        }

        if (callParticleIndex != -1)
        {
            BlockBreakingEffect effectPoolScript = breakingEffectPool[callParticleIndex].GetComponent<BlockBreakingEffect>();

            breakingEffectPool[callParticleIndex].SetActive(true);

            if (isAlreadyCalled == true)
            {
                effectPoolScript.ParticlePlay();
            }
            else
            {
                breakingEffectPool[callParticleIndex].transform.position = callingBlock.transform.position;
                effectPoolScript.callingBlock = callingBlock;
                effectPoolScript.changeColor(blockType);
                effectPoolScript.ParticlePlay();
            }

        }
    }

    public void CallBreakEffect(GameObject callingBlock, int blockType)
    {

        int callParticleIndex = -1;
        for (int i = 0; i < breakEffectPool.Count; i++)
        {
            if (!breakEffectPool[i].activeSelf)
            {
                callParticleIndex = i;
                break;
            }
        }

        if (callParticleIndex != -1)
        {
            BlockBreakEffect effectPoolScript = breakEffectPool[callParticleIndex].GetComponent<BlockBreakEffect>();

            breakEffectPool[callParticleIndex].SetActive(true);
            
            breakEffectPool[callParticleIndex].transform.position = callingBlock.transform.position;
            effectPoolScript.callingBlock = callingBlock;
            effectPoolScript.changeColor(blockType);
            effectPoolScript.ParticlePlay();

        }
    }

}
