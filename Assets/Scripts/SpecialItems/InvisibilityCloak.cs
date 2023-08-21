using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class InvisibilityCloak : ElympicsMonoBehaviour, IUpdatable
{
    [SerializeField] private GameObject sprite;
    public float duration = 10;
    private bool invisible = false;
    void Start()
    {
        
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
            }
            catch{}
        }
        else 
        {
            if(!invisible)
            {
                sprite.SetActive(false);
                invisible = true;
            }
            else
            {
                duration -= Elympics.TickDuration;
                if(duration < 0)
                {
                    sprite.SetActive(true);
                    transform.SetParent(null);
                    ElympicsDestroy(gameObject);
                }
            }
        }

    }
}
