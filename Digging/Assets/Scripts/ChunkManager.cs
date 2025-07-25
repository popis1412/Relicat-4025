using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    [SerializeField] GameObject[] chunks;
    [SerializeField] GameObject player;

    float activeDistance = 16.0f;

    private void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        if(player != null)
        {
            for (int i = 0; i < chunks.Length; i++)
            {
                float distanceToPlayer = Vector2.Distance(new Vector2(chunks[i].transform.position.x + 5f, chunks[i].transform.position.y - 5f), player.transform.position);

                print($"��ȣ {i}, �Ÿ�{distanceToPlayer}");

                if (chunks[i].activeSelf && distanceToPlayer > activeDistance)
                {
                    chunks[i].SetActive(false);
                }
                else if (!chunks[i].activeSelf && distanceToPlayer <= activeDistance)
                {
                   chunks[i].SetActive(true);
                }

            }
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

    }
}
