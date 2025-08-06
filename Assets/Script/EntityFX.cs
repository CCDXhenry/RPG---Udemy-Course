using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private Material hitMat;
    [SerializeField] private float flashDuration;
    private Material defaultMat;
    [SerializeField] private GameObject popUpTextPrefab;
    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        defaultMat = sr.material;
    }

    public void CreatePopUpTextInfo(string text)
    {
        float offsetX = Random.Range(-0.5f, 0.5f);
        float offsetY = Random.Range(0.5f, 1f);
        Vector3 positionOffset = new Vector2(offsetX, offsetY);
        GameObject popUpText = Instantiate(popUpTextPrefab,transform.position + positionOffset, Quaternion.identity);
        TextMeshPro textMeshPro = popUpText.GetComponent<TextMeshPro>();
        textMeshPro.SetText(text);
        textMeshPro.transform.localScale *= 0.8f; 
    }
    public void CreatePopUpTextDamage(int damage, float multiplier)
    {
        GameObject popUpText = Instantiate(popUpTextPrefab,transform.position,Quaternion.identity);
        
        popUpText.GetComponent<PopUpTextFx>().SetDamageValue(damage);

        TextMeshPro textMeshPro = popUpText.GetComponent<TextMeshPro>();
        // 根据 multiplier 决定颜色
        Color damageColor;

        if (multiplier > 1.1f) // 强力伤害（红色）
        {
            damageColor = Color.red;
        }
        else // 正常伤害（白色）
        {
            damageColor = Color.white;
        }
        textMeshPro.color = damageColor;
    }
    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        yield return new WaitForSeconds(flashDuration);
        sr.material = defaultMat;
    }

    private void RedColorBlink()
    {
        if (sr.color != Color.white)
        {
            sr.color = Color.white;
        }
        else
        {
            sr.color = Color.red;
        }
    }

    private void CancelRedBlink()
    {
        CancelInvoke();
        sr.color = Color.white;
    }
}
