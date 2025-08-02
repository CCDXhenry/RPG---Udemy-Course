using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostSoulsUIController : MonoBehaviour
{
    [SerializeField] private LostSoulsController lostSoulsController;
    void Awake()
    {
        lostSoulsController.OnLostSoulsTriggered += HandleTrigger;
    }

    private void HandleTrigger()
    {
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        lostSoulsController.OnLostSoulsTriggered -= HandleTrigger;
    }
}
