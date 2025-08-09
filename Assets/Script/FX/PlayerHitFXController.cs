using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitFXController : MonoBehaviour
{
    // Start is called before the first frame update
    private void FinishAnimation()
    {
        Destroy(gameObject);
    }
}
