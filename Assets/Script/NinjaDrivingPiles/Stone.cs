using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    [SerializeField] float maxRange = 1.0f;
    [SerializeField] float moveSpeed = 1.0f;
    private Vector3 startPos;
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var pos = Mathf.Sin(Time.time * moveSpeed) * maxRange;
        transform.position = startPos + new Vector3(0, pos);
    }
}
