using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collection : MonoBehaviour
{
    public Inventory Inventory;
    public Player player;

    [SerializeField] private GameObject[] Collection_Table;

    public bool[] li_isCollect;

    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    public Slot[] slots;

#if UNITY_EDITOR
    private void OnValidate()
    {
        slots = slotParent.GetComponentsInChildren<Slot>();
    }
#endif

    // Start is called before the first frame update
    void Start()
    {
        li_isCollect = new bool[player.items.Count];
        for (int i = 0; i < player.items.Count; i++)
        {
            li_isCollect[i] = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Button_ResistRelic(int itemNum)
    {
        Inventory.SellItem(player.items[itemNum]);
        Collection_Table[itemNum].GetComponentInChildren<SpriteRenderer>().sprite = player.items[itemNum].itemImage;
    }
}
