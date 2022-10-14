using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    GameManager gameManager;
    void FixedUpdate()
    {
        gameObject.transform.Rotate(2f, 0f, 0.0f, Space.Self);
    }
}