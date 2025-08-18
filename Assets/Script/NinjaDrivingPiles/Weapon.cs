using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Sword,
    Dart
}
public class Weapon : MonoBehaviour
{
    public WeaponType type;
    public int score;
    [SerializeField] float rotationsPerSecond = 180f;
    //private Vector3 bottomLeft;
    //private Vector3 topRight;
    public Rigidbody2D rb;
    void Start()
    {
        //bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0));
        //topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1));
        rb = GetComponent<Rigidbody2D>();
        rb.angularVelocity = rotationsPerSecond * 2 * Mathf.PI;
    }

    void OnEnable()
    {
        //Debug.Log("物体已激活，位置：" + transform.position);
        //Debug.Log("渲染器状态：" + GetComponentInChildren<Renderer>().enabled);
        if (rb != null)
        {
            rb.angularVelocity = rotationsPerSecond * 2 * Mathf.PI;
        }
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
       
    }
    void Update()
    {
        
        //var pos = transform.position;
        //if (pos.x < bottomLeft.x || pos.x > topRight.x || pos.y < bottomLeft.y || pos.y > topRight.y)
        //{
        //    //WeaponPool.instance.RecycleToPool(this.gameObject);

        //}
    }
    void OnBecameVisible()
    {
        //Debug.Log("物体进入视野: " + gameObject.name);
    }

    void OnBecameInvisible()
    {
        //Debug.Log("物体离开视野: " + gameObject.name);
        rb.angularVelocity = 0;
        WeaponPool.instance.RecycleToPool(this.gameObject);

    }
}
