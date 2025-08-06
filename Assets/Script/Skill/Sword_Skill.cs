using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}
public class Sword_Skill : Skill
{
    public SwordType swordType = SwordType.Regular;

    [Header("Spin info")]
    [SerializeField] private float maxTravelDistance; //最大移动距离
    [SerializeField] private float spinDuration; //旋转持续时间
    [SerializeField] private float spinGravity; //旋转重力
    [SerializeField] private float spinHitCooldown; //旋转冷却时间

    [Header("Bounce info")]
    [SerializeField] private int bounceCount; //反弹次数
    [SerializeField] private float bounceGravity; //反弹重力

    [Header("Pierce info")]
    [SerializeField] private int pierceCount; //穿透次数
    [SerializeField] private float pierceGravity; //穿透重力

    [Header("Sword info")]
    [SerializeField] private GameObject swordPrefab;

    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeTimeDuration; //冻结时间

    private Vector2 finaDir;

    //投掷抛物线
    [Header("Dots info")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float dotSpacing;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;

    protected override void Start()
    {
        base.Start();
        GenerateDots();
        SetupGravity();
    }
    private void SetupGravity()
    {
        switch (swordType)
        {
            case SwordType.Bounce:
                swordGravity = bounceGravity;
                break;
            case SwordType.Pierce:
                swordGravity = pierceGravity;
                break;
            case SwordType.Spin:
                swordGravity = spinGravity;
                break;
        }

    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            finaDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < numberOfDots; i++)
            {
                dots[i].transform.position = DotsPosition(i * dotSpacing);
            }
        }
    }
    public override void UseSkill()
    {
        base.UseSkill();
        CreateSword();
        AudioManager.instance.PlaySFX(18);
        AudioManager.instance.PlaySFX(19);
    }
    public void CreateSword()
    {

        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        Sword_Skill_Controller swordController = newSword.GetComponent<Sword_Skill_Controller>();

        switch (swordType)
        {
            case SwordType.Bounce:
                swordController.SetupBounce(true, bounceCount);
                break;
            case SwordType.Pierce:
                swordController.SetupPierce(pierceCount);
                break;
            case SwordType.Spin:
                swordController.SetupSpin(true, maxTravelDistance, spinDuration, spinHitCooldown);
                break;
        }

        if (swordController != null)
        {
            swordController.SetupSword(finaDir, swordGravity, player , freezeTimeDuration);
        }
        player.AssignNewSword(newSword);
        //取消投掷抛物线的点
        DotsActive(false);
    }

    #region Aim Direction
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = mousePosition - playerPosition;

        return direction;
    }


    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)//这里是否可以替换成numberOfDots？
        {
            if (dots[i] != null)
            {
                dots[i].SetActive(_isActive);
            }
        }
    }

    public void GenerateDots()
    {
        dots = new GameObject[numberOfDots];

        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    public Vector2 DotsPosition(float t)
    {
        // 计算抛物线位置
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y) * t
            + 0.5f * Physics2D.gravity * swordGravity * (t * t);
        return position;
    }
    #endregion
}
