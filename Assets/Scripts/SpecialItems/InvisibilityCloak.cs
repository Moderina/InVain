using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using UnityEngine.UI;

public class InvisibilityCloak : ElympicsMonoBehaviour, IUpdatable
{
    public Image itemSprite, cooldown;
    public SpriteRenderer ownSprite;
    [SerializeField] private GameObject sprite;
    [SerializeField] private GameObject nick;
    public ElympicsBool taken = new ElympicsBool(false);
    public ElympicsFloat duration = new ElympicsFloat(10);
    private bool invisible = false;
    public void Start()
    {
        itemSprite = GameObject.Find("MainUI").transform.Find("ItemTimer").GetComponent<Image>();
        cooldown = itemSprite.transform.Find("ImageCooldown").GetComponent<Image>();
    }

    public void Update()
    {
        if(!taken.Value) return;
        var player = transform.parent.GetComponent<ElympicsBehaviour>().PredictableFor;
        if(player != Elympics.Player) return;
        itemSprite.sprite = ownSprite.sprite;
        cooldown.sprite = itemSprite.sprite;
        cooldown.fillAmount = duration.Value / 10;
    }

    // Update is called once per frame
    public void ElympicsUpdate()
    {
        if (!Elympics.IsServer) return;
        if(sprite == null)
        {
            try
            {
                if(transform.parent.tag != "Player") return;
                sprite = transform.parent.GetChild(0).gameObject;
                nick = transform.parent.GetChild(2).gameObject;
            }
            catch{}
        }
        else 
        {
            taken.Value = true;
            if(!invisible)
            {
                sprite.SetActive(false);
                nick.SetActive(false);
                invisible = true;
            }
            else
            {
                duration.Value -= Elympics.TickDuration;
                if(duration.Value < 0)
                {
                    sprite.SetActive(true);
                    nick.SetActive(true);
                    transform.SetParent(null);
                    ElympicsDestroy(gameObject);
                }
            }
        }

    }
}
