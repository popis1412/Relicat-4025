using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class collectToolTipFollower : MonoBehaviour
{
    public Collection collection;

    public GameObject tooltipPanel; // 따라다닐 UI 패널
    public Vector2 offset = new Vector2(20f, -20f); // 마우스에서의 오프셋

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
                null, // 또는 UI가 World Space가 아니라면 Camera 대신 null
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
            tooltipText_list[1].text = "최초 등록 보상 : " + collection.player.items[idx].value.ToString() + "원";
            tooltipText_list[2].text = "누적 등록 보상 : " + collection.player.items[idx].duplicate_value.ToString() + "원";
            tooltipText_list[3].text = "누적 등록 개수 : " + collection.player.items[idx].accumulation_count.ToString() + "개";

        }
        //Debug.Log(tooltipPanel.activeSelf);
    }

    public void OnPointerExit()
    {
        isHovering = false;
        tooltipPanel.SetActive(false);
    }
}
