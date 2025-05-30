using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class collectToolTipFollower : MonoBehaviour
{
    public Collection collection;

    public GameObject tooltipPanel; // ����ٴ� UI �г�
    public Vector2 offset = new Vector2(20f, -20f); // ���콺������ ������

    private bool isHovering = false;

    [SerializeField] private TextMeshProUGUI[] tooltipText_list;


    void Update()
    {
        if (isHovering && tooltipPanel.activeSelf)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                tooltipPanel.transform.parent as RectTransform,
                Input.mousePosition,
                null, // �Ǵ� UI�� World Space�� �ƴ϶�� Camera ��� null
                out pos
            );

            tooltipPanel.GetComponent<RectTransform>().anchoredPosition = pos + offset;

            
        }
    }

    public void OnPointerEnter(int idx)
    {
        
        if (collection.li_isCollect[idx] == true)
        {
            isHovering = true;
            tooltipPanel.SetActive(true);

            tooltipText_list[0].text = collection.player.items[idx].itemName;
            tooltipText_list[1].text = "���� ��� ���� : " + collection.player.items[idx].value.ToString() + "��";
            tooltipText_list[2].text = "���� ��� ���� : " + collection.player.items[idx].duplicate_value.ToString() + "��";
            tooltipText_list[3].text = "���� ��� ���� : " + collection.player.items[idx].accumulation_count.ToString() + "��";

        }
        //Debug.Log(tooltipPanel.activeSelf);
    }

    public void OnPointerExit()
    {
        isHovering = false;
        tooltipPanel.SetActive(false);
    }
}
