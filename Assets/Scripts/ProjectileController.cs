using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * 50f);
        Destroy(gameObject, 5f);
    }


}
