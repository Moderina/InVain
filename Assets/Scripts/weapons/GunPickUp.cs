using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Elympics;
using System.Globalization;

public class GunPickUp : ElympicsMonoBehaviour
{
    
    public void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.tag);
        if (col.tag == "Player") 
        {
            if (col.transform.childCount > 4) return;
            if (col.transform.GetChild(0).childCount > 3) return;

            var player = col.transform.GetComponent<ElympicsBehaviour>();
            if(Elympics.Player == player.PredictableFor) 
            {
                Image itemSprite = GameObject.Find("MainUI").transform.Find("ItemTimer").GetComponent<Image>();
                itemSprite.gameObject.SetActive(true);
                itemSprite.sprite = transform.parent.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                itemSprite.transform.GetChild(1).gameObject.SetActive(true);
                itemSprite.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = transform.parent.GetComponent<Pistol>().bulletammount.ToString();
            }
            if (Elympics.IsServer) for(int i=0; i<100; i++) {}

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
            Debug.Log("wynocha");
            var player = col.transform.parent.GetComponent<ElympicsBehaviour>();
            Debug.Log(Elympics.Player == player.PredictableFor);
            if(Elympics.Player == player.PredictableFor) 
            {
                Debug.Log("inside");
                Image itemSprite = GameObject.Find("MainUI").transform.Find("ItemTimer").GetComponent<Image>();
                itemSprite.gameObject.SetActive(true);
                Debug.Log("ppppppp" + itemSprite.gameObject.activeSelf);
                itemSprite.sprite = transform.parent.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                itemSprite.transform.GetChild(1).gameObject.SetActive(true);
                itemSprite.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = transform.parent.GetComponent<Pistol>().bulletammount.ToString();
            }

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
