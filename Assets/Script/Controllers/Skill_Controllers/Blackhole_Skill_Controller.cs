using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill_Controller : MonoBehaviour
{
    [Header("Blackhole Skill Settings")]
    private float growSpeed;
    private float shrinkSpeed;
    private float maxSize;

    public bool isBlackholeActive = false; // 黑洞技能是否激活,提供其他脚本查询
    private bool canGrow;
    private bool canShrink;

    private List<Transform> targets = new List<Transform>();
    private float blackholeDuration; // 黑洞持续时间
    private float blackholeTimer; // 黑洞计时器

    [Header("Key Settings")]
    public List<KeyCode> hotKeyList = new List<KeyCode>();
    [SerializeField] private GameObject hotKeyPrefab;

    private List<GameObject> createdHotKeys = new List<GameObject>();

    [Header("Clone Settings")]
    [SerializeField] private float cloneInterval; // 克隆间隔时间
    private float cloneTimer; // 克隆计时器
    [SerializeField] private float cloneCounts; // 克隆数量
    private float remainingCloneCount;
    private bool isTransparable;


    public void SetupBlackhole(float _maxSize, float _growSpeed, float _shrinkSpeed, float _cloneCounts, float _blackholeDuration)
    {
        maxSize = _maxSize; // 设置黑洞最大尺寸
        growSpeed = _growSpeed; // 设置黑洞增长速度
        shrinkSpeed = _shrinkSpeed;// 设置黑洞缩小速度
        cloneCounts = _cloneCounts; // 初始化克隆计数
        blackholeDuration = _blackholeDuration; // 设置黑洞持续时间

        isBlackholeActive = true; // 激活黑洞技能
        blackholeTimer = blackholeDuration; // 初始化黑洞计时器
        canGrow = true; // 初始状态允许增长
        canShrink = false; // 初始状态不允许缩小
        cloneTimer = 0f; // 初始化克隆计时器
        //cloneInterval = blackholeDuration / cloneCounts; // 设置克隆间隔时间()
        cloneInterval = 0;
        remainingCloneCount = cloneCounts; // 初始化剩余克隆计数
        isTransparable = false;
    }

    private void Update()
    {
        blackholeTimer -= Time.deltaTime; // 减少黑洞计时器
        if (blackholeTimer <= 0f)
        {
            canShrink = true; // 当黑洞计时器结束时，允许缩小
        }

        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
            if (transform.localScale.x >= maxSize - 0.1f) // 当达到最大尺寸时，开始克隆
            {
                canGrow = false; // 停止增长
            }
        }
        if (!canGrow && !canShrink)
        {

            if (targets.Count > 0 && remainingCloneCount > 0)
            {
                if (!isTransparable)
                {
                    PlayerManager.instance.player.MakeTransparent(true); // 使玩家透明
                    isTransparable = true;

                    //根据已按热键数量增加克隆数量
                    if (targets.Count > 0)
                    {
                        remainingCloneCount += targets.Count; // 增加克隆计数
                        cloneInterval = Mathf.Min(blackholeTimer / remainingCloneCount, 0.5f); // 重新计算克隆间隔时间
                    }
                    DestroyHotKeys(); // 销毁所有热键
                }
                cloneTimer -= Time.deltaTime;
                if (cloneTimer <= 0)
                {
                    remainingCloneCount--; // 减少克隆计数
                    int targetIndex = Random.Range(0, targets.Count); // 随机选择一个目标
                                                                      //xOffset随机为-2或2，用于控制克隆物的位置偏移
                    float xOffset = Random.Range(0, 2) == 0 ? -1f : 1f;
                    SkillManager.instance.clone.CreateClone(targets[targetIndex], new Vector3(xOffset, 0), false);

                    cloneTimer = cloneInterval;
                }
            }
            if (targets.Count <= 0 || remainingCloneCount <= 0)
            {
                DestroyHotKeys(); // 销毁所有热键
                canShrink = true; // 如果没有目标或克隆计数用完，允许缩小
            }
        }

        if (canShrink)
        {
            //DestroyHotKeys(); // 销毁所有热键
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);
            if (transform.localScale.x <= 0.5f)
            {
                FinishBlackholeAbility();
                isBlackholeActive = false; // 设置黑洞技能为非激活状态

                Destroy(gameObject); // 销毁黑洞技能对象
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
                Destroy(hotKey); // 销毁所有已创建的热键
            }
            createdHotKeys.Clear(); // 清空已创建热键列表
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Enemy enemy))
        {
            enemy.FreezeTimer(true); // 冻结敌人
            if (!canGrow)
                return; // 如果黑洞没有处于增长状态，则不创建热键
            // 检查是否有可用的热键
            bool flowControl = CreateAndAssignHotKey(collision);
            if (!flowControl)
            {
                return;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) => collision.GetComponent<Enemy>()?.FreezeTimer(false); // 解除敌人冻结状态

    private bool CreateAndAssignHotKey(Collider2D collision)
    {
        if (hotKeyList.Count <= 0)
        {
            Debug.LogWarning("No hotkeys available to assign!");
            return false; // 如果没有可用的热键，直接返回
        }
        //创建热键
        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 1.5f), Quaternion.identity);
        createdHotKeys.Add(newHotKey); // 将新创建的热键添加到已创建热键列表中
        KeyCode choosenKey = hotKeyList[Random.Range(0, hotKeyList.Count)]; // 随机选择一个热键
        hotKeyList.Remove(choosenKey); // 从列表中移除已使用的热键

        Blackhole_HotKey_Controller hotKeyController = newHotKey.GetComponent<Blackhole_HotKey_Controller>();
        if (hotKeyController != null)
        {
            hotKeyController.SetupHotKey(choosenKey, collision.transform, this); // 设置热键
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
