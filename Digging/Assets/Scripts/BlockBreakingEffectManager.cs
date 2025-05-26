using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBreakingEffectManager : MonoBehaviour
{
    [SerializeField] GameObject blockBreakingEffect;
    private List<GameObject> effectsPool = new List<GameObject>();
    private int poolSize = 5;
    int loopCount = 0;

    private void Awake()
    {
        effectsPool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(blockBreakingEffect);
            obj.SetActive(false);
            effectsPool.Add(obj);
        }
    }

    public void CallEffect(GameObject callingBlock)
    {
        int callParticleIndex = -1;
        bool isAlreadyCalled = false;
        for(int i = 0; i < effectsPool.Count; i++)
        {
            if (effectsPool[i].GetComponent<BlockBreakingEffect>().callingBlock == callingBlock && effectsPool[i].activeSelf)
            {
                callParticleIndex = i;
                isAlreadyCalled = true;
                break;
            }
            print("reCall" + callParticleIndex + ", " + loopCount++);
        }

        if (isAlreadyCalled == false)
        {
            for(int i = 0; i < effectsPool.Count; i++)
            {
                if(!effectsPool[i].activeSelf)
                {
                    callParticleIndex = i;
                    break;
                }
            }
            print("SetIndex" + callParticleIndex + ", " + loopCount++);
        }

        if (callParticleIndex != -1)
        {
            BlockBreakingEffect effectPoolScript = effectsPool[callParticleIndex].GetComponent<BlockBreakingEffect>();

            effectsPool[callParticleIndex].SetActive(true);

            if (isAlreadyCalled == true)
            {
                effectPoolScript.ParticlePlay();
            }
            else
            {
                effectsPool[callParticleIndex].transform.position = callingBlock.transform.position;
                effectPoolScript.callingBlock = callingBlock;
                effectPoolScript.ParticlePlay();
                print("callEffect" + callParticleIndex + ", " + loopCount);
            }

        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            print(loopCount);
    }
}
