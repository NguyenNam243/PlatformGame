using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpDemo : MonoBehaviour
{
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Debug.Log("Jump");
    }
}
