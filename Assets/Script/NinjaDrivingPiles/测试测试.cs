using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 测试测试 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.angularVelocity = 36000 * Mathf.Deg2Rad;
        Debug.Log("角速度（rad/s）: " + rb.angularVelocity);

    }
    private void Update()
    {
        
    }
}
