using UnityEngine;

public class Weapon : MonoBehaviour
{
    float _pickdamage;   // 채굴 속도 [default: 6]
    int _durability;     // 내구도
    float _vertiRange;   // 채굴 범위(세로)
    float _horRange;     // 채굴 범위(가로)

    public void Init(float pickdamge, int durability, float vertiRange, float horRange) // 무기 세팅 초기화
    {
        _pickdamage = pickdamge;
        _durability = durability;
        _vertiRange = vertiRange;
        _horRange = horRange;
    }

    public void Digging()
    {
        
    }

    // 마우스 위치 기준으로 파괴 가능한 블록 반환
    private GameObject GetVaildBlockUnderMouse()
    {
        // 마우스 레이저
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

        // 거리
        Vector2 characterCenter = transform.position;
        Vector2 dirToHit = hit.point - characterCenter;
        Vector2 hitNormal = hit.normal;


        // 시각적으로 레이저 확인
        Debug.DrawLine(characterCenter, hit.point, Color.red, 1f);
        Debug.DrawRay(hitNormal, hitNormal.normalized, Color.blue);

        Debug.Break();


        // 콜라이더가 없(배경, 빈 화면)거나 블록인 경우만 캐기
        if(hit.collider == null || !hit.collider.CompareTag("Block"))
        return null;

        // 캐기 거리
        float dot = Vector2.Dot(dirToHit, hitNormal);

        // 캐릭터 기준 블록을 캘 수 있는 최대 거리
        //float digDistance = IsStuckInSand() ?
        //    NarrowDigDistance :     // 모래만 캘 수 있는 범위(0.5)
        //    1.5f / transform.localScale.x;

        //return distance < digDistance ? hit.collider.gameObject : null;

        print("내적:  " + dot);

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

