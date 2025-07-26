using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill_Controller : MonoBehaviour
{
    [Header("Blackhole Skill Settings")]
    private float growSpeed;
    private float shrinkSpeed;
    private float maxSize;

    public bool isBlackholeActive = false; // �ڶ������Ƿ񼤻�,�ṩ�����ű���ѯ
    private bool canGrow;
    private bool canShrink;

    private List<Transform> targets = new List<Transform>();
    private float blackholeDuration; // �ڶ�����ʱ��
    private float blackholeTimer; // �ڶ���ʱ��

    [Header("Key Settings")]
    public List<KeyCode> hotKeyList = new List<KeyCode>();
    [SerializeField] private GameObject hotKeyPrefab;

    private List<GameObject> createdHotKeys = new List<GameObject>();

    [Header("Clone Settings")]
    [SerializeField] private float cloneInterval; // ��¡���ʱ��
    private float cloneTimer; // ��¡��ʱ��
    [SerializeField] private float cloneCounts; // ��¡����
    private float remainingCloneCount;
    private bool isTransparable;


    public void SetupBlackhole(float _maxSize, float _growSpeed, float _shrinkSpeed, float _cloneCounts, float _blackholeDuration)
    {
        maxSize = _maxSize; // ���úڶ����ߴ�
        growSpeed = _growSpeed; // ���úڶ������ٶ�
        shrinkSpeed = _shrinkSpeed;// ���úڶ���С�ٶ�
        cloneCounts = _cloneCounts; // ��ʼ����¡����
        blackholeDuration = _blackholeDuration; // ���úڶ�����ʱ��

        isBlackholeActive = true; // ����ڶ�����
        blackholeTimer = blackholeDuration; // ��ʼ���ڶ���ʱ��
        canGrow = true; // ��ʼ״̬��������
        canShrink = false; // ��ʼ״̬��������С
        cloneTimer = 0f; // ��ʼ����¡��ʱ��
        //cloneInterval = blackholeDuration / cloneCounts; // ���ÿ�¡���ʱ��()
        cloneInterval = 0;
        remainingCloneCount = cloneCounts; // ��ʼ��ʣ���¡����
        isTransparable = false;
    }

    private void Update()
    {
        blackholeTimer -= Time.deltaTime; // ���ٺڶ���ʱ��
        if (blackholeTimer <= 0f)
        {
            canShrink = true; // ���ڶ���ʱ������ʱ��������С
        }

        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
            if (transform.localScale.x >= maxSize - 0.1f) // ���ﵽ���ߴ�ʱ����ʼ��¡
            {
                canGrow = false; // ֹͣ����
            }
        }
        if (!canGrow && !canShrink)
        {

            if (targets.Count > 0 && remainingCloneCount > 0)
            {
                if (!isTransparable)
                {
                    PlayerManager.instance.player.MakeTransparent(true); // ʹ���͸��
                    isTransparable = true;

                    //�����Ѱ��ȼ��������ӿ�¡����
                    if (targets.Count > 0)
                    {
                        remainingCloneCount += targets.Count; // ���ӿ�¡����
                        cloneInterval = Mathf.Min(blackholeTimer / remainingCloneCount, 0.5f); // ���¼����¡���ʱ��
                    }
                    DestroyHotKeys(); // ���������ȼ�
                }
                cloneTimer -= Time.deltaTime;
                if (cloneTimer <= 0)
                {
                    remainingCloneCount--; // ���ٿ�¡����
                    int targetIndex = Random.Range(0, targets.Count); // ���ѡ��һ��Ŀ��
                                                                      //xOffset���Ϊ-2��2�����ڿ��ƿ�¡���λ��ƫ��
                    float xOffset = Random.Range(0, 2) == 0 ? -1f : 1f;
                    SkillManager.instance.clone.CreateClone(targets[targetIndex], new Vector3(xOffset, 0));

                    cloneTimer = cloneInterval;
                }
            }
            if (targets.Count <= 0 || remainingCloneCount <= 0)
            {
                DestroyHotKeys(); // ���������ȼ�
                canShrink = true; // ���û��Ŀ����¡�������꣬������С
            }
        }

        if (canShrink)
        {
            //DestroyHotKeys(); // ���������ȼ�
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);
            if (transform.localScale.x <= 0.5f)
            {
                FinishBlackholeAbility();
                isBlackholeActive = false; // ���úڶ�����Ϊ�Ǽ���״̬

                Destroy(gameObject); // ���ٺڶ����ܶ���
            }
        }
    }

    private void FinishBlackholeAbility()
    {
        PlayerManager.instance.player.MakeTransparent(false);
    }

    public void DestroyHotKeys()
    {
        if (createdHotKeys.Count > 0)
        {
            foreach (GameObject hotKey in createdHotKeys)
            {
                Destroy(hotKey); // ���������Ѵ������ȼ�
            }
            createdHotKeys.Clear(); // ����Ѵ����ȼ��б�
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Enemy enemy))
        {
            enemy.FreezeTimer(true); // �������
            if (!canGrow)
                return; // ����ڶ�û�д�������״̬���򲻴����ȼ�
            // ����Ƿ��п��õ��ȼ�
            bool flowControl = CreateAndAssignHotKey(collision);
            if (!flowControl)
            {
                return;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) => collision.GetComponent<Enemy>()?.FreezeTimer(false); // ������˶���״̬

    private bool CreateAndAssignHotKey(Collider2D collision)
    {
        if (hotKeyList.Count <= 0)
        {
            Debug.LogWarning("No hotkeys available to assign!");
            return false; // ���û�п��õ��ȼ���ֱ�ӷ���
        }
        //�����ȼ�
        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 1.5f), Quaternion.identity);
        createdHotKeys.Add(newHotKey); // ���´������ȼ���ӵ��Ѵ����ȼ��б���
        KeyCode choosenKey = hotKeyList[Random.Range(0, hotKeyList.Count)]; // ���ѡ��һ���ȼ�
        hotKeyList.Remove(choosenKey); // ���б����Ƴ���ʹ�õ��ȼ�

        Blackhole_HotKey_Controller hotKeyController = newHotKey.GetComponent<Blackhole_HotKey_Controller>();
        if (hotKeyController != null)
        {
            hotKeyController.SetupHotKey(choosenKey, collision.transform, this); // �����ȼ�
        }
        else
        {
            Debug.LogError("Blackhole_HotKey_Controller component not found on the hotkey prefab!");
        }

        return true;
    }

    public void AddEnemyTransfrom(Transform _enemyTransfrom)
    {
        targets.Add(_enemyTransfrom);
    }
}
