using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.name);
        if (col.tag == "Player") 
        {
            this.transform.parent.SetParent(col.transform);
        }
        else if (col.tag == "Work")
        {
            this.transform.parent.SetParent(col.transform.parent);
        }
        this.transform.parent.GetComponent<Die>().LiveTimeStart();
        gameObject.SetActive(false);
    }
}
