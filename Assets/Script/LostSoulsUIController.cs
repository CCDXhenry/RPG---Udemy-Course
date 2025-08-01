using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostSoulsUIController : MonoBehaviour
{
    private LostSoulsController lostSoulsController;
    void Start()
    {
        lostSoulsController = GetComponentInChildren<LostSoulsController>();
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
