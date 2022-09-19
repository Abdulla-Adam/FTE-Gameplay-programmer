using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public GameObject dustPrefab;
    public float dustOffset;

    public void JumpVfx()
    {
        GameObject go = Instantiate(dustPrefab,new Vector3(transform.position.x, transform.position.y - dustOffset, transform.position.z),transform.rotation);
    }
}
