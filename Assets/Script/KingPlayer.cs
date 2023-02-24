using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingPlayer : MonoBehaviour
{
    // Update is called once per frame
    int speed = 10;
    void Update()
    {
        float xMove = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float yMove = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        this.transform.Translate(new Vector3(xMove, yMove, 0));
    }
}
