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

        // �� ��������
        List<GameObject> blocks;

        if(isSanded)
        {
            // �𷡿� �������� �ڽ��� ��ġ�� �ִ� ���� ��ȯ
            GameObject sandBlock = GetCurrentPlayerBlock(player.GetComponent<PlayerController>());
            blocks = new List<GameObject>();
            if(sandBlock != null) blocks.Add(sandBlock);
        }
        else
        {
            // ���������� �ֺ� �� ��ȯ
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

        // ã�� ���� ������ŭ �� �ı�
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
        //// Dig ����
        if(blockType == 0 || blockType == 4 || blockType == 5)
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

        // ���� ���
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
        // ���� ���
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
        // �� ���
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

    // ���콺 ��ġ �������� �ı� ������ ��� ��ȯ
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

        // ���� �÷��̾�(position)
        Vector2 origin = GetCenteredPosition(blockPos);
        Vector2 playerCenter = GetCenteredPosition(playerPos);

        // ���� ���(step, �밢�� ���� ����)
        Vector2 direction = GetDirection(playerCenter, origin, out Vector2 step, out bool isDiagonal);
        if(direction == Vector2.zero)
            return diggableBlocks;

        // �Ÿ� Ȯ��(range)
        if(!IsOneStepAway(playerCenter, origin, step))
            return diggableBlocks;

        // �밢�� ���� ó��
        if(isDiagonal)
        {
            if(CanDigDiagonal(origin, direction, blocksDict))
                TryAddBlock(origin, blocksDict, diggableBlocks);

            return diggableBlocks;
        }

        // ���� ���� ó��(range��ŭ �ݺ�)
        AddBlocksInLine(origin, step, direction, range, blocksDict, diggableBlocks);
        return diggableBlocks;
    }

    // ��ǥ ����
    private Vector2 GetCenteredPosition(Vector2 pos)
    {
        return new Vector2(Mathf.Floor(pos.x) + 0.5f, Mathf.Floor(pos.y) + 0.5f);
    }

    // ���� ��������, step, �밢�� ���θ� ��ȯ
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

    // ���� range��ŭ ������ �ִ��� Ȯ��
    private bool IsOneStepAway(Vector2 from, Vector2 to, Vector2 step)
    {
        float epsilon = 0.01f;
        return Mathf.Abs((to - from).magnitude - step.magnitude) <= epsilon;
    }

    // �ش� ��ġ�� ���� ����Ʈ�� �߰�
    private void TryAddBlock(Vector2 pos, BlocksDictionary dict, List<GameObject> list)
    {
        Vector2 key = GetCenteredPosition(pos);
        if(dict.blockPosition.TryGetValue(key, out GameObject block))
            list.Add(block);
    }

    // ���� ������ �������� range��ŭ �� �߰�
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

    // �밢�� ���� Ķ �� �ִ� ���� �˻�
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
                return true; // �� �� �ϳ��� ������ Ķ �� ����
        }

        return false; // �� �� ������ Ķ �� ����
    }

    // ���� ���͸� 8���� �� �ϳ��� ����ȭ
    private Vector2 GetSimplifiedDirection(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle = (angle + 360f) % 360f;

        Vector2[] directions = {
        new Vector2(1, 0),   // ��
        new Vector2(1, 1),   // ��
        new Vector2(0, 1),   // ��
        new Vector2(-1, 1),  // ��
        new Vector2(-1, 0),  // ��
        new Vector2(-1, -1), // ��
        new Vector2(0, -1),  // ��
        new Vector2(1, -1)   // ��
    };

        int index = Mathf.RoundToInt(angle / 45f) % 8;
        return directions[index];
    }

    // ���� ����
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

    // Grid���� �������� ����ȭ
    List<GameObject> ToGrid(RaycastHit2D hit, BlocksDictionary blocksDict)
    {
        List<GameObject> blocks = new();
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Vector2 playerPos = player.transform.position;
        Vector2Int PgridPos = new Vector2Int   // �÷��̾� ���� ��ġ
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

            print($"����: {direction}, offset: {offset}, ����ġ: {blockWorldPos}");

            Vector2Int BgridPos = new Vector2Int    // �� ���� ��ġ
            (
                Mathf.FloorToInt(blockWorldPos.x / tileSize.x),
                Mathf.FloorToInt(blockWorldPos.y / tileSize.y)
            );

            // ���� �Ÿ� ���(���� O, ũ�� O)
            Vector2Int diff = BgridPos - PgridPos;

            // ����ư �Ÿ�(���� �Ÿ�)�� 1���� �Ǵ� (���� X, ũ�� O)
            int manhattanDistance = Mathf.Abs(diff.x) + Mathf.Abs(diff.y);

            bool isDiagonalBlock = CanDiagonalBlock(playerPos, diff, blocksDict);

            if(manhattanDistance <= range && !isDiagonalBlock) // ����(�����¿�)
            {
                if(blocksDict.blockPosition.TryGetValue(blockWorldPos, out GameObject obj))
                {
                    blocks.Add(obj);
                }
            }
            else if(manhattanDistance <= range + 1 && isDiagonalBlock) // �밢��
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

    // �밢�� ����
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

        //print($"����: {_left}, ������: {_right}, ����: {_up}, �Ʒ���: {_down}");

        // �밢�� ���⿡ ���� �ʿ��� �� ���⸸ �˻�
        if(diff.x > 0 && diff.y > 0)       // ��
            return !_right || !_up;
        else if(diff.x < 0 && diff.y > 0)  // ��
            return !_left || !_up;
        else if(diff.x < 0 && diff.y < 0)  // ��
            return !_left || !_down;
        else if(diff.x > 0 && diff.y < 0)  // ��
            return !_right || !_down;

        return false;
    }

}

