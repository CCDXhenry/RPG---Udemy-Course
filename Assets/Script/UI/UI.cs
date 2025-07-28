using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public UI_ItemToolTip itemToolTip;

    public GameObject characterUI;
    public GameObject skillTreeUI;
    public GameObject craftUI;
    public GameObject optionsUI;
    private bool isUIOpen = false;
    private void Start()
    {
        
        isUIOpen = false;
        SwitchTo(null);

        itemToolTip.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isUIOpen)
            {
                SwitchTo(null);
                isUIOpen = false;
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
            transform.GetChild(i).gameObject.SetActive(false);
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
            isUIOpen = false;
            return;
        }
        SwitchTo(_menu);
    }
}
