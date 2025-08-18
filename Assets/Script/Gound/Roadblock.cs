using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roadblock : MonoBehaviour
{
    private bool canMove = false;
    [SerializeField] float moveSpeed;
    [SerializeField] float maxY = 12;
    BoxCollider2D cd;
    // Start is called before the first frame update
    void Start()
    {
        cd = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cd.bounds.max.y >= maxY - 0.1f)
        {
            canMove = false;
        }
        else
        {
            canMove = true;
        }
        if (canMove)
        {
            var posx = Mathf.Lerp(transform.position.y, maxY - cd.bounds.extents.y, moveSpeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, posx);
        }
    }
}
