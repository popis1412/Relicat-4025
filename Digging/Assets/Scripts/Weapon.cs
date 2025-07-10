using UnityEngine;

public class Weapon : MonoBehaviour
{
    float _pickdamage;   // ä�� �ӵ� [default: 6]
    int _durability;     // ������
    float _vertiRange;   // ä�� ����(����)
    float _horRange;     // ä�� ����(����)

    public void Init(float pickdamge, int durability, float vertiRange, float horRange) // ���� ���� �ʱ�ȭ
    {
        _pickdamage = pickdamge;
        _durability = durability;
        _vertiRange = vertiRange;
        _horRange = horRange;
    }

    public void Digging()
    {
        
    }

    // ���콺 ��ġ �������� �ı� ������ ��� ��ȯ
    private GameObject GetVaildBlockUnderMouse()
    {
        // ���콺 ������
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

        // �Ÿ�
        Vector2 characterCenter = transform.position;
        Vector2 dirToHit = hit.point - characterCenter;
        Vector2 hitNormal = hit.normal;


        // �ð������� ������ Ȯ��
        Debug.DrawLine(characterCenter, hit.point, Color.red, 1f);
        Debug.DrawRay(hitNormal, hitNormal.normalized, Color.blue);

        Debug.Break();


        // �ݶ��̴��� ��(���, �� ȭ��)�ų� ����� ��츸 ĳ��
        if(hit.collider == null || !hit.collider.CompareTag("Block"))
        return null;

        // ĳ�� �Ÿ�
        float dot = Vector2.Dot(dirToHit, hitNormal);

        // ĳ���� ���� ����� Ķ �� �ִ� �ִ� �Ÿ�
        //float digDistance = IsStuckInSand() ?
        //    NarrowDigDistance :     // �𷡸� Ķ �� �ִ� ����(0.5)
        //    1.5f / transform.localScale.x;

        //return distance < digDistance ? hit.collider.gameObject : null;

        print("����:  " + dot);

        return hit.collider.gameObject;

        //print($"block Distance: {DigDistance}, Character Distance: {distance}");
    }

}

class PickAxe : Weapon
{
    private void Start()
    {
        Init(1, -1, 1, 1);
    }
}

class Drill : Weapon
{
    private void Start()
    {
        Init(1, -1, 1, 1);
    }
}

