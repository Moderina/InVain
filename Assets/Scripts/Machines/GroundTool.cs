using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTool : MonoBehaviour
{
    [SerializeField] private ItemData data;

    void Start()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = data.sprite;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "Work")
        {
            
        }
    }
}
