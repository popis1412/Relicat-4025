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

    // �׼�
    public Action<SlotInfo> onLeftClick;
    public Action<SlotInfo> onRightClick;

    [SerializeField] private Image image;
    [HideInInspector] public Transform parentAfterDrag;

    // �̹��� �ֱ�
    public void Apply<T>(T instance)
    {
        ///<TOOD>
        /// �ش� Ÿ���� �ۼ��� �̸��� �ʵ� �˻�
        /// Instance: �ν��Ͻ� �ʵ� ���(static ����)
        /// Public: public �ʵ�
        /// NonPublic: private, protected �ʵ� 
        /// </TOOD>
        Sprite icon = typeof(T).GetField("itemImage", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?
        .GetValue(instance) as Sprite;

        image.sprite = icon;
        image.color = new Color(1, 1, 1, 1);

        // UI ������ ����
        float width = image.sprite.rect.width;
        float height = image.sprite.rect.height;

        Vector2 size = rect.sizeDelta;

        if(width > height + 20)
        {
            size.x = 80f;
            size.y = height;  // ���ΰ� ��� ���� ũ�⸦ �ش� �̹����� ũ�⿡ ����.
        }
        else if(height > width + 20)
        {
            size.x = width;  // ���ΰ� ��� ���� ũ�⸦ �ش� �̹����� ũ�⿡ ����.
            size.y = 80f;
        }
        else
        {
            size = new Vector2(80, 80);
        }

        rect.sizeDelta = size;
    }

    // ���� ���� ������ ����
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

    // �̹��� ���ֱ�
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

        // �������� ���
        if(slotInfo._type == SlotType.QuickSlot)
        {
            // ���ų� ������ ��ĭ�� ���
            if(!SlotManager.Instance._isOpen || slotInfo._instanceW == null && slotInfo._instanceI == null)
                return;
        }
        else if(slotInfo._type == SlotType.Inventory)
        {
            if(eventData.pointerClick.GetComponent<Slot>().item == null)
                return;
        }
        
        switch(eventData.button)
        {
            case PointerEventData.InputButton.Left:
                onLeftClick?.Invoke(slotInfo);
                break;
            // �̰� ���߿� ���ֵ� ��. ���� ���ſ���.
            case PointerEventData.InputButton.Right:
                onRightClick?.Invoke(slotInfo);
                break;
        }
    }

    #endregion Mouse Event
}
