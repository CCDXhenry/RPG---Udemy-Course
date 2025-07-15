using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill : Skill
{
    [Header("Sword info")]
    [SerializeField] private GameObject swordPrefab;

    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;

    private Vector2 finaDir;

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            finaDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);
        }
    }

    public void CreateSword()
    {

        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);

        Sword_Skill_Controller swordController = newSword.GetComponent<Sword_Skill_Controller>();
        if (swordController != null)
        {
            swordController.SetupSword(finaDir, swordGravity);
        }

    }

    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        Vector2 direction = mousePosition - playerPosition;

        Debug.Log(direction);
        return direction;
    }
}
