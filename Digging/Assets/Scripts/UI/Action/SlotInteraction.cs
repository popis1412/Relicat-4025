using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotInteraction : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private RectTransform rect;

    [SerializeField] SlotInfo slotInfo;

    // 액션
    public Action<SlotInfo> onLeftClick;
    public Action<SlotInfo> onRightClick;

    [SerializeField] private Image image;
    [HideInInspector] public Transform parentAfterDrag;

    // 이미지 넣기
    public void Apply<T>(T instance)
    {
        ///<TOOD>
        /// 해당 타입의 작성한 이름의 필드 검색
        /// Instance: 인스턴스 필드 대상(static 제외)
        /// Public: public 필드
        /// NonPublic: private, protected 필드 
        /// </TOOD>
        Sprite icon = null;

        try
        {
            var field = typeof(T).GetField("itemImage", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if(field != null)
            {
                object value = field.GetValue(instance);
                icon = value as Sprite;
            }
        }
        catch
        {
            // 아무 것도 하지 않음 — icon은 null 유지됨
        }

        // 스프라이트가 없으면 그냥 슬롯 Clear()
        if(icon == null)
        {
            Clear();
            return;
        }


        if(instance is WeaponInstance weapon)   // 무기 업그레이드 시
        {
            icon = weapon.GetSprite();  // 레벨에 따른 이미지 가져오기
            print(icon);
        }

        image.sprite = icon;
        image.color = new Color(1, 1, 1, 1);

        // UI 사이즈 조절
        float width = image.sprite.rect.width;
        float height = image.sprite.rect.height;

        Vector2 size = rect.sizeDelta;

        if(width > height + 20)
        {
            size.x = 80f;
            size.y = height;  // 가로가 길면 세로 크기를 해당 이미지의 크기에 맞춤.
        }
        else if(height > width + 20)
        {
            size.x = width;  // 세로가 길면 가로 크기를 해당 이미지의 크기에 맞춤.
            size.y = 80f;
        }
        else
        {
            size = new Vector2(80, 80);
        }

        rect.sizeDelta = size;
    }

    // 서로 슬롯 데이터 변경
    public void SetSlotInfo(SlotInfo info)
    {
        slotInfo = info;
        var image = GetComponent<Image>();

        if(info._instanceW != null)
        {
            Apply(info._instanceW);
        }
        else if(info._instanceI != null)
        {
            Apply(info._instanceI);
        }
        else
        {
            Clear();
        }
    }

    // 이미지 없애기
    public void Clear()
    {
        Vector2 size = new Vector2(80, 80);
        rect.sizeDelta = size;

        slotInfo._instanceI = null;
        slotInfo._instanceW = null;

        image.sprite = null;
        image.color = new Color(0, 0, 0, 0);
    }

    #region Mouse Event
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!SlotManager.Instance._isOpen)
            return;

        // 퀵슬롯일 경우
        if(slotInfo._type == SlotType.QuickSlot)
        {
            // 없거나 완전한 빈칸인 경우, 갯수가 0개인 경우
            if(!SlotManager.Instance._isOpen || slotInfo._instanceW == null && slotInfo._instanceI == null || slotInfo._instanceI._count <= 0)
                return;
        }
        else if(slotInfo._type == SlotType.Inventory)
        {
            if(GetComponent<Slot>().item == null || GetComponent<Slot>().item.type == ItemType.Null || GetComponent<Slot>().item.count <= 0)
                return;
        }
        
        switch(eventData.button)
        {
            case PointerEventData.InputButton.Left:
                onLeftClick?.Invoke(slotInfo);
                break;
            // 이건 나중에 없애도 됨. 무기 제거용임.
            case PointerEventData.InputButton.Right:
                onRightClick?.Invoke(slotInfo);
                break;
        }
    }

    #endregion Mouse Event
}
