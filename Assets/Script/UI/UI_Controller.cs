using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Controller : MonoBehaviour
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
    private bool isGameOver = false;
    private void Awake()
    {
        SwitchTo(skillTreeUI);
        SwitchTo(optionsUI);
    }
    private void Start()
    {

        SwitchInGameUI();

        itemToolTip.gameObject.SetActive(false);
        skillToolTip.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isGameOver)
        {
            return;
        }

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
            if (GameManage.instance != null)
            {
                if (_menu != inGameUI)
                {
                    GameManage.instance.PauseGame(true);
                }
                else
                {
                    GameManage.instance.PauseGame(false);
                }
            }
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
        isGameOver = true;
        yield return new WaitForSeconds(1);
        gameOverUI.SetActive(true);
        yield return new WaitForSeconds(1);
        restartGameUI.SetActive(true);
    }

    public void RestartGameButton()
    {
        GameManage.instance.RestartScene();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
