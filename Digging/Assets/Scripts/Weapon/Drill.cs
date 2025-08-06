using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Drill: MonoBehaviour
{
    [SerializeField] private AudioSource diggingAudioSource;

    private bool isDigging = false;
    private bool isDigSound = false;

    // ������ ī��Ʈ
    float decreaseEnergy = 10f;  // ������ ���ҷ� - 1�ʸ��� 0.1����
    float cooldown = 1f;        // ��Ÿ��
    float timer;                // �ð�          

    // �ִϸ��̼� ī��Ʈ
    private float t;
    private float angle;

    private Vector3 pivot;

    public WeaponInstance _instance;

    public Action<Drill> OnEnergyChanged; // �̺�Ʈ �ݹ� - ������ ���� - ���͸�
    public Action<Drill> OnDecreaseEnergy; // ������ ����

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
        timer = cooldown;
    }

    public void Digging(Vector2 worldMousePos, Player player)
    {
        RaycastHit2D hit = Physics2D.Raycast(worldMousePos, Vector2.zero);

        // �� ��������
        List<GameObject> blocks = GetVaildBlocksUnderMouse(hit);
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

        // Ÿ�̸�
        timer -= Time.deltaTime;
        if(timer < 0f)
        {
            timer = cooldown;   // Ÿ�̸� ����
            DecreaseEnergy(decreaseEnergy);
        }

        AnimateDrill();
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

    private void AnimateDrill()
    {
        pivot = transform.parent.Find("Pivot").position;

        t += Time.fixedDeltaTime * (_instance._damage / 4);
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
        Vector2 clickBlockPos = hit.collider.transform.position;

        var blocksDict = hit.collider.GetComponent<Block>().blocksDictionary;

        return CanDigBlocks(playerPos, clickBlockPos, blocksDict, new Vector2(1, 1));//_instance._range);
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
            {
                TryAddBlock(origin, blocksDict, diggableBlocks);    // �߾�
                TryAddCrossBlocks(origin, blocksDict, diggableBlocks);  // ���ڰ�
            }
                

            return diggableBlocks;
        }
        else
        {
            // ���� ���� ó��(range��ŭ �ݺ�)
            AddBlocksInLine(origin, step, direction, range, blocksDict, diggableBlocks); // �߾�
            TryAddCrossBlocks(origin, blocksDict, diggableBlocks);  // ���ڰ�
        }

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

    // Ķ �� �ִ� �� ã�ƿ���
    private void TryAddCrossBlocks(Vector2 origin, BlocksDictionary dict, List<GameObject> list)
    {
        Vector2[] offsets = new Vector2[]
        {
            Vector2.left,
            Vector2.right,
            Vector2.up,
            Vector2.down
        };

        foreach(var offset in offsets)
        {
            TryAddBlock(origin + offset, dict, list);
        }
    }

    // ������ ����(�׼�)
    public void ChargeEnergy()
    {
        // PrograssBar�� ������ �ؼ� �� value ���� �ϵ�
    }

    // ������ ����
    public void DecreaseEnergy(float amount)
    { 
        if(_instance._energy <= 0)
            return;

        _instance._energy -= amount;
        OnDecreaseEnergy?.Invoke(this);
    }
}
