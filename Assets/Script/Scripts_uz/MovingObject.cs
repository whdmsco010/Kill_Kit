using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    float moveX, moveY;

    [Header("이동속도 조절")]
    [SerializeField]    [Range(1f,30f)] float moveSpeed = 20f;

    void Update()
    {
        moveX=Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
        moveY=Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime;

        transform.position = new Vector2(transform.position.x + moveX, transform.position.y + moveY);
    }
}
