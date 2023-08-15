using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using System.Globalization;

public class GunPickUp : MonoBehaviour
{
    
    public void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.name);
        if (col.tag == "Player") 
        {
            this.transform.parent.SetParent(col.transform.GetChild(0));
            if (col.transform.GetChild(0).localScale.x == -1)
            {
                Vector3 newScale = transform.parent.localScale;
                newScale.x *= -1;
                transform.parent.localScale = newScale;
                newScale = transform.parent.localPosition;
                newScale.x = -1;
                transform.parent.localPosition = newScale;
            }
        }
        else if (col.tag == "Work")
        {
            this.transform.parent.SetParent(col.transform.parent.GetChild(0));
            Debug.Log(col.transform.parent.GetChild(0).position.x);
            if (col.transform.parent.GetChild(0).localScale.x == -1)
            {
                Vector3 newScale = transform.parent.localScale;
                newScale.x *= -1;
                transform.parent.localScale = newScale;
                newScale = transform.parent.localPosition;
                newScale.x = 1;
                transform.parent.localPosition = newScale;
            }
        }
        gameObject.SetActive(false);
    }
}
