using System.Collections;
using Cinemachine;
using TMPro;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private Player player;
    private SpriteRenderer sr;
    [SerializeField] private Material hitMat;
    [SerializeField] private float flashDuration;
    private Material defaultMat;
    [SerializeField] private GameObject popUpTextPrefab;
    [Header("Screen shake FX")]
    private CinemachineImpulseSource screenShake;
    [SerializeField] private float shakeMultiplier;
    public Vector3[] shakeDamage;
    public Vector3 shakeSwordImpact;
    private void Start()
    {
        player = PlayerManager.instance.player;
        sr = GetComponentInChildren<SpriteRenderer>();
        defaultMat = sr.material;
        screenShake = GetComponent<CinemachineImpulseSource>();
    }

    public void ScreenShake(Vector3 _shakePower)
    {
        screenShake.m_DefaultVelocity = new Vector3(_shakePower.x * player.facingDirection, _shakePower.y) * shakeMultiplier;
        screenShake.GenerateImpulse();
    }

    public void CreatePopUpTextInfo(string text)
    {
        float offsetX = Random.Range(-0.5f, 0.5f);
        float offsetY = Random.Range(0.5f, 1f);
        Vector3 positionOffset = new Vector2(offsetX, offsetY);
        GameObject popUpText = Instantiate(popUpTextPrefab, transform.position + positionOffset, Quaternion.identity);
        TextMeshPro textMeshPro = popUpText.GetComponent<TextMeshPro>();
        textMeshPro.SetText(text);
        textMeshPro.transform.localScale *= 0.8f;
    }
    public void CreatePopUpTextDamage(int damage, float multiplier)
    {
        GameObject popUpText = Instantiate(popUpTextPrefab, transform.position, Quaternion.identity);

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
