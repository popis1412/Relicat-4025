using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class Pickaxe : MonoBehaviour
{
    [SerializeField] private AudioSource diggingAudioSource;

    private bool isDigging = false;
    private bool isDigSound = false;

    private float t;
    private float angle;

    private Vector3 pivot;

    public WeaponInstance _instance;

    public int speed;

    public float damage;
    [SerializeField] float range = 1;

    Vector2 tileSize;

    public void Setup(WeaponInstance instance)
    {
        _instance = instance;
    }

    private void Awake()
    {
        diggingAudioSource = GameObject.FindWithTag("Player")?.GetComponent<AudioSource>();
    }

    private void Start()
    {
        tileSize = new (1, 1);
        damage = _instance._damage;
    }

    public void Digging(Vector2 worldMousePos, Player player, bool isSanded)
    {
        RaycastHit2D hit = Physics2D.Raycast(worldMousePos, Vector2.zero, 0f, LayerMask.GetMask("Block"));
        if(hit.collider == null)
            return;

        var blocksDict = hit.collider.GetComponent<Block>().blocksDictionary;
        if(blocksDict == null)
            return;

        // 블럭 가져오기
        List<GameObject> blocks;

        if(isSanded)
        {
            // 모래에 갇혔으면 자신의 위치에 있는 블럭만 반환
            GameObject sandBlock = GetCurrentPlayerBlock(player.GetComponent<PlayerController>());
            blocks = new List<GameObject>();
            if(sandBlock != null) blocks.Add(sandBlock);
        }
        else
        {
            // 정상적으로 주변 블럭 반환
            //blocks = GetVaildBlocksUnderMouse(hit);
            blocks = ToGrid(hit, blocksDict);
        }

        if(blocks == null || blocks.Count == 0)
        {
            isDigging = false;
            t = 0;
            return;
        }

        isDigging = true;

        // 찾을 블럭의 개수만큼 블럭 파괴
        foreach(GameObject blockObj in blocks)
        {
            if(blockObj.TryGetComponent(out Block block))
            {
                block.BlockDestroy(_instance._damage * Time.deltaTime, player);
                PlayDigSound(block.blockType);
            }
        }

        AnimatePickaxe();
    }

    private void PlayDigSound(int blockType)
    {
        //// Dig 사운드
        if(blockType == 0 || blockType == 4 || blockType == 5 || blockType == 101 || blockType == 102)
        {
            int idx = UnityEngine.Random.Range(5, 9);
            if(isDigging && isDigSound == false)
            {
                diggingAudioSource.PlayOneShot(SoundManager.Instance.SFXSounds[idx]);
                isDigSound = true;
            }
            if(!isDigging && diggingAudioSource.isPlaying == true)
            {
                diggingAudioSource.Stop();
                isDigSound = false;
            }
            if(diggingAudioSource.isPlaying == false)
            {
                isDigSound = false;
            }
        }

        // 광물 블록
        if(blockType == 2 || blockType == 7 || blockType == 8 || blockType == 9 || blockType == 10 || blockType == 11)
        {
            if(isDigging && isDigSound == false)
            {
                diggingAudioSource.PlayOneShot(SoundManager.Instance.SFXSounds[12]);
                isDigSound = true;
            }
            if(!isDigging && diggingAudioSource.isPlaying == true)
            {
                diggingAudioSource.Stop();
                isDigSound = false;
            }
            if(diggingAudioSource.isPlaying == false)
            {
                isDigSound = false;
            }
        }
        // 바위 블록
        if(blockType == 3 || blockType == -1)
        {
            int idx = UnityEngine.Random.Range(9, 12);
            if(isDigging && isDigSound == false)
            {
                diggingAudioSource.PlayOneShot(SoundManager.Instance.SFXSounds[idx]);
                isDigSound = true;
            }
            if(!isDigging && diggingAudioSource.isPlaying == true)
            {
                diggingAudioSource.Stop();
                isDigSound = false;
            }
            if(diggingAudioSource.isPlaying == false)
            {
                isDigSound = false;
            }
        }
        // 모래 블록
        if(blockType == 6)
        {

            if(isDigging && isDigSound == false)
            {
                diggingAudioSource.PlayOneShot(SoundManager.Instance.SFXSounds[8]);
                isDigSound = true;
            }
            if(!isDigging && diggingAudioSource.isPlaying == true)
            {
                diggingAudioSource.Stop();
                isDigSound = false;
            }
            if(diggingAudioSource.isPlaying == false)
            {
                isDigSound = false;
            }
        }
    }

    private void AnimatePickaxe()
    {
        pivot = transform.parent.Find("Pivot").position;

        t += Time.fixedDeltaTime * (_instance._damage);
        t = Mathf.Clamp01(t);

        angle = Mathf.Lerp(60, -30, t);
        float rad = angle * Mathf.Deg2Rad;

        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * 0.1f;

        if(t >= 1)
        {
            t = 0;
            transform.position = pivot;
            transform.rotation = Quaternion.Euler(0f, 0f, -30f);
        }

        transform.position = pivot + offset;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    // 마우스 위치 기준으로 파괴 가능한 블록 반환
    public List<GameObject> GetVaildBlocksUnderMouse(RaycastHit2D hit)
    {
        if(hit.collider == null || !hit.collider.CompareTag("Block"))
            return null;

        Vector2 playerPos = transform.root.position;
        Vector2 clickBlockPos = hit.point;

        var blocksDict = hit.collider.GetComponent<Block>().blocksDictionary;

        return CanDigBlocks(playerPos, clickBlockPos, blocksDict, _instance._range);
    }

    public List<GameObject> CanDigBlocks(Vector2 playerPos, Vector2 blockPos, BlocksDictionary blocksDict, Vector2 range)
    {
        List<GameObject> diggableBlocks = new();

        // 블럭과 플레이어(position)
        Vector2 origin = GetCenteredPosition(blockPos);
        Vector2 playerCenter = GetCenteredPosition(playerPos);

        // 방향 계산(step, 대각선 여부 포함)
        Vector2 direction = GetDirection(playerCenter, origin, out Vector2 step, out bool isDiagonal);
        if(direction == Vector2.zero)
            return diggableBlocks;

        // 거리 확인(range)
        if(!IsOneStepAway(playerCenter, origin, step))
            return diggableBlocks;

        // 대각선 방향 처리
        if(isDiagonal)
        {
            if(CanDigDiagonal(origin, direction, blocksDict))
                TryAddBlock(origin, blocksDict, diggableBlocks);

            return diggableBlocks;
        }

        // 직선 방향 처리(range만큼 반복)
        AddBlocksInLine(origin, step, direction, range, blocksDict, diggableBlocks);
        return diggableBlocks;
    }

    // 좌표 보정
    private Vector2 GetCenteredPosition(Vector2 pos)
    {
        return new Vector2(Mathf.Floor(pos.x) + 0.5f, Mathf.Floor(pos.y) + 0.5f);
    }

    // 방향 가져오기, step, 대각선 여부를 반환
    private Vector2 GetDirection(Vector2 from, Vector2 to, out Vector2 step, out bool isDiagonal)
    {
        Vector2 dir = (to - from).normalized;
        if(dir == Vector2.zero)
            dir = new Vector2(Mathf.Sign(from.x), 0f); // fallback

        Vector2 simplified = GetSimplifiedDirection(dir);
        isDiagonal = Mathf.Abs(simplified.x) > 0 && Mathf.Abs(simplified.y) > 0;

        step = simplified;
        return simplified;
    }

    // 블럭이 range만큼 떨어져 있는지 확인
    private bool IsOneStepAway(Vector2 from, Vector2 to, Vector2 step)
    {
        float epsilon = 0.01f;
        return Mathf.Abs((to - from).magnitude - step.magnitude) <= epsilon;
    }

    // 해당 위치의 블럭을 리스트에 추가
    private void TryAddBlock(Vector2 pos, BlocksDictionary dict, List<GameObject> list)
    {
        Vector2 key = GetCenteredPosition(pos);
        if(dict.blockPosition.TryGetValue(key, out GameObject block))
            list.Add(block);
    }

    // 직선 방향을 기준으로 range만큼 블럭 추가
    private void AddBlocksInLine(Vector2 start, Vector2 step, Vector2 dir, Vector2 range, BlocksDictionary dict, List<GameObject> list)
    {
        int repeat = dir.x != 0 ? Mathf.FloorToInt(range.x) : Mathf.FloorToInt(range.y);
        Vector2 current = start;

        for(int i = 0; i < repeat; i++)
        {
            TryAddBlock(current, dict, list);
            current += step;
        }
    }

    // 대각선 블럭을 캘 수 있는 조건 검사
    private bool CanDigDiagonal(Vector2 origin, Vector2 direction, BlocksDictionary dict)
    {
        Vector2[] checkDirs = direction switch
        {
            var d when d == new Vector2(1, 1) => new[] { new Vector2(-1f, 0f), new Vector2(0f, -1f) },
            var d when d == new Vector2(-1, 1) => new[] { new Vector2(1f, 0f), new Vector2(0f, -1f) },
            var d when d == new Vector2(-1, -1) => new[] { new Vector2(1f, 0f), new Vector2(0f, 1f) },
            var d when d == new Vector2(1, -1) => new[] { new Vector2(-1f, 0f), new Vector2(0f, 1f) },
            _ => new Vector2[0]
        };

        foreach(var check in checkDirs)
        {
            Vector2 neighbor = origin + check;
            Vector2 key = GetCenteredPosition(neighbor);
            if(!dict.blockPosition.ContainsKey(key))
                return true; // 둘 중 하나라도 없으면 캘 수 있음
        }

        return false; // 둘 다 있으면 캘 수 없음
    }

    // 방향 벡터를 8방향 중 하나로 정규화
    private Vector2 GetSimplifiedDirection(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle = (angle + 360f) % 360f;

        Vector2[] directions = {
        new Vector2(1, 0),   // →
        new Vector2(1, 1),   // ↗
        new Vector2(0, 1),   // ↑
        new Vector2(-1, 1),  // ↖
        new Vector2(-1, 0),  // ←
        new Vector2(-1, -1), // ↙
        new Vector2(0, -1),  // ↓
        new Vector2(1, -1)   // ↘
    };

        int index = Mathf.RoundToInt(angle / 45f) % 8;
        return directions[index];
    }

    // 현재 방향
    private GameObject GetCurrentPlayerBlock(PlayerController player)
    {
        Vector2 playerCenter = new Vector2(
            Mathf.Floor(player.transform.position.x) + 0.5f,
            Mathf.Floor(player.transform.position.y) + 0.5f
        );

        if(player.blocksDictionary.blockPosition.TryGetValue(playerCenter, out GameObject blockObj))
        {
            return blockObj;
        }

        return null;
    }

    // Grid격자 형식으로 정규화
    List<GameObject> ToGrid(RaycastHit2D hit, BlocksDictionary blocksDict)
    {
        List<GameObject> blocks = new();
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Vector2 playerPos = player.transform.position;
        Vector2Int PgridPos = new Vector2Int   // 플레이어 격자 위치
        (
            Mathf.FloorToInt(playerPos.x / tileSize.x),
            Mathf.FloorToInt(playerPos.y / tileSize.y)
        );

        Vector2 blockPos = hit.collider.gameObject.transform.position;

        Vector2 direction = blockPos - playerPos;

        for(int i = 0; i < range; i++)
        {
            float x = direction.x > 0 ? 1 : -1;
            float y = direction.y > 0 ? 1 : -1;

            Vector2 offset = Mathf.Round(Mathf.Abs(direction.y)) >= 1 ? new Vector2(0, i * y) : new Vector2(i * x, 0);
            Vector2 blockWorldPos = blockPos + offset;

            print($"방향: {direction}, offset: {offset}, 블럭위치: {blockWorldPos}");

            Vector2Int BgridPos = new Vector2Int    // 블럭 격자 위치
            (
                Mathf.FloorToInt(blockWorldPos.x / tileSize.x),
                Mathf.FloorToInt(blockWorldPos.y / tileSize.y)
            );

            // 격자 거리 계산(방향 O, 크기 O)
            Vector2Int diff = BgridPos - PgridPos;

            // 맨해튼 거리(격자 거리)로 1인지 판단 (방향 X, 크기 O)
            int manhattanDistance = Mathf.Abs(diff.x) + Mathf.Abs(diff.y);

            bool isDiagonalBlock = CanDiagonalBlock(playerPos, diff, blocksDict);

            if(manhattanDistance <= range && !isDiagonalBlock) // 정면(상하좌우)
            {
                if(blocksDict.blockPosition.TryGetValue(blockWorldPos, out GameObject obj))
                {
                    blocks.Add(obj);
                }
            }
            else if(manhattanDistance <= range + 1 && isDiagonalBlock) // 대각선
            {
                if(playerPos.y == diff.y)
                    return null;

                blocks.Add(hit.collider.gameObject);
            }
            else
            {
                return null;
            }
        }

        return blocks;
    }

    // 대각선 여부
    bool CanDiagonalBlock(Vector2 playerPos, Vector2 diff, BlocksDictionary blocksDict)
    {
        if(diff.x == 0 || diff.y == 0)
            return false;

        bool _right = true;
        bool _left = true;
        bool _up = true;
        bool _down = true;

        playerPos = new Vector2(Mathf.Floor(playerPos.x) + 0.5f, Mathf.Floor(playerPos.y) + 0.5f);

        Vector2[] directions = new Vector2[]
        {
            diff.x > 0 ? Vector2.right : Vector2.left,
            diff.y > 0 ? Vector2.up : Vector2.down
        };

        foreach(Vector2 dir in directions)
        {
            Vector2 neighborPos = playerPos + dir;

            if(!blocksDict.blockPosition.TryGetValue(neighborPos, out GameObject obj))
            {
                if(dir == Vector2.left)
                    _left = false;
                else if(dir == Vector2.right)
                    _right = false;
                else if(dir == Vector2.up)
                    _up = false;
                else if(dir == Vector2.down)
                    _down = false;
            }
        }

        //print($"왼쪽: {_left}, 오른쪽: {_right}, 위쪽: {_up}, 아래쪽: {_down}");

        // 대각선 방향에 따라 필요한 두 방향만 검사
        if(diff.x > 0 && diff.y > 0)       // ↗
            return !_right || !_up;
        else if(diff.x < 0 && diff.y > 0)  // ↖
            return !_left || !_up;
        else if(diff.x < 0 && diff.y < 0)  // ↙
            return !_left || !_down;
        else if(diff.x > 0 && diff.y < 0)  // ↘
            return !_right || !_down;

        return false;
    }

}

