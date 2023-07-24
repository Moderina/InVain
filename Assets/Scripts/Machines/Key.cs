using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnCreate(Transform parent)
    {
        transform.SetParent(parent);
        transform.position = parent.position;
        transform.localPosition.Set(0f,2f,0f);
    }
}
