using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public UI_ItemToolTip itemToolTip;
    public UI_SkillToolTip skillToolTip;


    public GameObject characterUI;
    public GameObject skillTreeUI;
    public GameObject craftUI;
    public GameObject optionsUI;
    public GameObject inGameUI;
    public GameObject darkScreenUI;
    public GameObject gameOverUI;
    public GameObject restartGameUI;

    private bool isUIOpen = false;
    private void Awake()
    {
        SwitchTo(skillTreeUI);//awake技能按钮监听事件
    }
    private void Start()
    {

        isUIOpen = false;
        SwitchTo(inGameUI);

        itemToolTip.gameObject.SetActive(false);
        skillToolTip.gameObject.SetActive(false);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isUIOpen)
            {
                SwitchInGameUI();
            }
            else
            {
                SwitchWithKeyTo(characterUI);
            }
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            SwitchWithKeyTo(characterUI);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            SwitchWithKeyTo(skillTreeUI);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            SwitchWithKeyTo(craftUI);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            SwitchWithKeyTo(optionsUI);
        }
    }

    public void SwitchTo(GameObject _menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject transformChild = transform.GetChild(i).gameObject;
            // 设置darkScreenUI为不隐藏状态
            if (transformChild == darkScreenUI)
            {
                if (!transformChild.activeSelf)
                {
                    transformChild.SetActive(true);
                }
            }
            else
            {
                transformChild.SetActive(false);
            }
        }
        if (_menu != null)
        {
            _menu.SetActive(true);
            isUIOpen = true;
        }
    }

    private void SwitchWithKeyTo(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            SwitchInGameUI();
            return;
        }
        SwitchTo(_menu);
    }

    private void SwitchInGameUI()
    {
        SwitchTo(inGameUI);
        isUIOpen = false;
    }

    public void SwitchOnGameOverScreen()
    {
        SwitchTo(null);
        darkScreenUI.GetComponent<UI_FadeScreen>().TriggerFadeOut();
        StartCoroutine(GameOverScreenCorutione());
    }

    IEnumerator GameOverScreenCorutione()
    {
        yield return new WaitForSeconds(1);
        gameOverUI.SetActive(true);
        yield return new WaitForSeconds(1);
        restartGameUI.SetActive(true);
    }

    public void RestartGameButton()
    {
        GameManage.instance.RestartScene();
    }
}
