using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class QuitSlotUI : MonoBehaviour
{
    #region Field
    [SerializeField] private List<SlotInfo> slotInfos;
    public List<SlotInfo> quickSlots => slotInfos;

    // ���Կ� ���� �ʵ�
    private WeaponInstance instanceW = null;
    private ItemInstance instanceI = null;

    #endregion Field

    private void Start()
    {
        slotInfos = GetComponentsInChildren<SlotInfo>(true).ToList();

        for(int i = 0; i < slotInfos.Count; i++)
        {
            slotInfos[i].Initialize(SlotType.QuickSlot, i); // ���� Ÿ�԰� ���� ���� �ֱ�

            var item = slotInfos[i].GetComponentInChildren<SlotInteraction>();
            // ��Ŭ��: ����
            item.onLeftClick = SlotManager.Instance.SetSelectedSlot;

            // ��Ŭ��: ����
            item.onRightClick = SlotManager.Instance.RemoveInstance;
        }
    }
    #region Func

    // �� ���� ã��(�������� ���)[���� �ν��Ͻ��� �� ���]
    public SlotInfo FindEmptySlot(bool isWeapon, string name)
    {
        foreach(var slot in slotInfos)
        {
            // ������ ����ִ� ���Կ� ���� �� ��
            if(slot._instanceW == null && slot._instanceI == null)
                return slot;
            // �������� �ִ� ��Ȳ - �������� �ִ� ������ ���� �ʾ��� ��
            else if(!isWeapon && slot._instanceW == null && slot._instanceI != null)
            {
                // ���߿� �ν��Ͻ��� ���� ī��Ʈ�� �ε带 �ؼ� �ϴ� ���� ����
                int count = slot._instanceI._count;
                int limit = slot._instanceI._item.stackLimit;

                if(count < limit && slot._instanceI._item.itemName == name)    // �����ۿ��� ���⸦ ä�� ��
                    return slot;

                continue;
            }
            // ���Ⱑ �ִ� ��Ȳ
            else if(slot._instanceW != null && slot._instanceI == null)
                continue;
            else
                continue;
        }

        return null;
    }

    // �� ���� ã��
    /*public SlotInfo FindEmptySlot(bool isWeapon, string name)
    {
        foreach(var slot in slotInfos)
        {
            // ������ ����ִ� �����̸� �ٷ� ��ȯ
            if(slot._instanceW == null && slot._instanceI == null)
                return slot;

            // �������� �ִ� ��Ȳ
            else if(slot._instanceW == null && slot._instanceI != null)
                if(slot._instanceI._item.name == name)
                    return slot;

            // ���Ⱑ �ִ� ��Ȳ
            else if(slot._instanceI == null && slot._instanceW != null)
                continue;

            else
                continue;
        }

        return null;
    }*/

    public SlotInfo FindDrill(Drill drill)
    {
        foreach(SlotInfo slot in quickSlots)
        {
            if(slot._instanceI == null && slot._instanceW._template.type == WeaponType.Drill)
            {
                if(slot._instanceW._energy == 0)    // �������� ������ ���� �帱�� ã��
                    continue;

                return slot;
            }
        }

        return null;
    }

    #endregion Func
}

