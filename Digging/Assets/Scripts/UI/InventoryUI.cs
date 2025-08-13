using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using static UnityEditor.Progress;
#endif

public class InventoryUI : MonoBehaviour
{
    #region Field

    // ���Կ� ���� �ʵ�
    private WeaponInstance instanceW;
    private ItemInstance instanceI;

    [SerializeField] private List<SlotInfo> slotInfos;
    public IReadOnlyList<SlotInfo> InventorySlots => slotInfos;

    #endregion Field

    // �κ��丮 ������ ä���
    public void InitFillData()
    {
        slotInfos = GetComponentsInChildren<SlotInfo>(true).ToList();

        for(int i = 0; i < slotInfos.Count; i++)
        {
            slotInfos[i].Initialize(SlotType.Inventory, i); // ���� Ÿ�԰� ���� ���� �ֱ�

            var item = slotInfos[i].GetComponentInChildren<SlotInteraction>();
            // ��Ŭ��: ����
            item.onLeftClick = SlotManager.Instance.SetSelectedSlot;

            // ��Ŭ��: ����
            item.onRightClick = SlotManager.Instance.RemoveInstance;
        }
    }

    // �κ��丮 �ʱ�ȭ
    public void ResetInventorySlot()
    {
        foreach(var slot in slotInfos)
        {
            slot.GetComponentInChildren<SlotInteraction>().Clear();
        }
    }

    // �� ���� ã�� -> ���߿� ���� �����̳� ���Ⱑ �κ��丮���� ���� �׶� ���
    /*public SlotInfo FindEmptySlot(bool isWeapon, string name)
    {
        foreach(var slot in slotInfos)
        {
            // ������ ����ִ� ���Կ� ���� �� ��
            if(slot._instanceW == null && slot._instanceI == null)
                return slot;
            // �������� �ִ� ��Ȳ
            else if(!isWeapon && slot._instanceW == null && slot._instanceI != null)
            {
                if(slot._instanceI._item.itemName == name)
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
    }*/

}
