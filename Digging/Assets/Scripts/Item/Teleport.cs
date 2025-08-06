using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private ItemInstance _instance;

    public void Setup(ItemInstance instance)
    {
        _instance = instance;
    }

    public void Spawn(PlayerController player)
    {
        player.transform.position = new Vector3(14.5f, 0.5f, 0);
    }
}
