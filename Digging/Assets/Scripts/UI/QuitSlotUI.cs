using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class QuitSlotUI : MonoBehaviour
{
    #region Field
    [SerializeField] private List<SlotInfo> slotInfos;
    public List<SlotInfo> quickSlots => slotInfos;

    // 슬롯에 관한 필드
    private WeaponInstance instanceW = null;
    private ItemInstance instanceI = null;

    #endregion Field

    #region Func

    // 퀵슬롯 데이터 채우기
    public void InitFillData()
    {
        slotInfos = GetComponentsInChildren<SlotInfo>(true).ToList();

        for(int i = 0; i < slotInfos.Count; i++)
        {
            slotInfos[i].Initialize(SlotType.QuickSlot, i); // 슬롯 타입과 슬롯 순서 넣기

            var item = slotInfos[i].GetComponentInChildren<SlotInteraction>();
            // 좌클릭: 선택
            item.onLeftClick = SlotManager.Instance.SetSelectedSlot;

            // 우클릭: 제거
            item.onRightClick = SlotManager.Instance.RemoveInstance;
        }
    }

    // 퀵슬롯 초기화
    public void ResetQuickSlot()
    {
        foreach(var slot in quickSlots)
        {
            slot._instanceW = null;
            slot._instanceI = null;

            slot.GetComponentInChildren<SlotInteraction>().Clear();
            SlotManager.Instance.UpdateText(slot);
        }
    }

    // 빈 슬롯 찾기(아이템인 경우)[내부 인스턴스로 할 경우]
    public SlotInfo FindEmptySlot(bool isWeapon, string name)
    {
        foreach(var slot in slotInfos)
        {
            // 완전히 비어있는 슬롯에 무기 일 때
            if(slot._instanceW == null && slot._instanceI == null)
                return slot;
            // 아이템이 있는 상황 - 아이템이 최대 개수를 넘지 않았을 때
            else if(!isWeapon && slot._instanceW == null && slot._instanceI != null)
            {
                // 나중에 인스턴스의 내부 카운트를 로드를 해서 하는 것을 권장
                int count = slot._instanceI._count;
                int limit = slot._instanceI._item.stackLimit;

                if(count < limit && slot._instanceI._item.itemName == name)    // 아이템에서 무기를 채울 때
                    return slot;

                continue;
            }
            // 무기가 있는 상황
            else if(slot._instanceW != null && slot._instanceI == null)
                continue;
            else
                continue;
        }

        return null;
    }

    // 빈 슬롯 찾기
    /*public SlotInfo FindEmptySlot(bool isWeapon, string name)
    {
        foreach(var slot in slotInfos)
        {
            // 완전히 비어있는 슬롯이면 바로 반환
            if(slot._instanceW == null && slot._instanceI == null)
                return slot;

            // 아이템이 있는 상황
            else if(slot._instanceW == null && slot._instanceI != null)
                if(slot._instanceI._item.name == name)
                    return slot;

            // 무기가 있는 상황
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
                if(slot._instanceW._energy == 0)    // 에너지가 없으면 다음 드릴을 찾기
                    continue;

                return slot;
            }
        }

        return null;
    }

    #endregion Func
}

