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
            if (col.transform.childCount > 4) return;
            if (col.transform.GetChild(0).childCount > 3) return;
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
            if (col.transform.parent.childCount > 4) return;
            if (col.transform.parent.GetChild(0).childCount > 3) return;
            this.transform.parent.SetParent(col.transform.parent.GetChild(0));
            if (col.transform.parent.GetChild(0).localScale.x == -1)
            {
                Vector3 newScale = transform.parent.localScale;
                newScale.x *= -1;
                transform.parent.localScale = newScale;
                newScale = transform.parent.localPosition;
                newScale.x = 1;
                transform.parent.localPosition = newScale;
            }
            else 
            {
                Vector3 newScale = transform.parent.localPosition;
                newScale.x = 1;
                transform.parent.localPosition = newScale;
            }
        }
        gameObject.SetActive(false);
    }
}
