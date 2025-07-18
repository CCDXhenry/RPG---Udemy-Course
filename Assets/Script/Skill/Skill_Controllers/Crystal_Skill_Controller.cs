using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private float crystalTimer;
    private float crystalDuration; // Duration for which the crystal remains active
    private float maxDistance; // Maximum distance the crystal can be placed from the player
    private bool canGrow;
    private float growSpeed;
    private float maxSize;

    private bool _isExploding = false;
    public bool isExploding
    {
        get => _isExploding;
        set => _isExploding = value;
    }

    private GameObject currentCrystal; // Prefab for the crystal
    private Animator anim;
    private CircleCollider2D circleCollider2D;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        circleCollider2D = GetComponent<CircleCollider2D>();
    }
    public void SetupCrystal(GameObject _currentCrystal, float _crystalDuration, float _maxDistance, bool _canGrow, float _growSpeed, float _maxSize)
    {
        currentCrystal = _currentCrystal;
        crystalDuration = _crystalDuration;
        maxDistance = _maxDistance;
        canGrow = _canGrow;
        growSpeed = _growSpeed;
        maxSize = _maxSize;
        crystalTimer = crystalDuration;
    }

    private void Update()
    {
        crystalTimer -= Time.deltaTime;
        float distanceToPlayer = Vector2.Distance(PlayerManager.instance.player.transform.position, currentCrystal.transform.position);
        if (crystalTimer <= 0f || maxDistance < distanceToPlayer)
        {
            isExploding = true;
        }
        if (isExploding)
        {
            anim.SetTrigger("Explode");
            if (canGrow)
            {
                currentCrystal.transform.localScale = Vector2.Lerp(currentCrystal.transform.localScale, Vector2.one * maxSize, growSpeed * Time.deltaTime);
            }
        }

    }
    public void AnimationAttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(currentCrystal.transform.position, circleCollider2D.radius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out Enemy enemy))
            {
                Vector2 knockbackVector = new Vector2(5, 0);
                int damageFacingDirection = (currentCrystal.transform.position.x > enemy.transform.position.x) ? -1 : 1;
                enemy.Damage(knockbackVector, damageFacingDirection);
            }
        }
    }

    public void AnimationFinishTrigger()
    {
        DestroyCrystal();
    }

    private void DestroyCrystal()
    {
        Destroy(gameObject);
        Destroy(currentCrystal);
    }
}
