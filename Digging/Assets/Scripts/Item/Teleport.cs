using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    PlayerController player;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    private void Start()
    {

    }

    public void Spawn()
    {
        player.transform.position = new Vector3(14.5f, 0.5f, 0);
    }
}
