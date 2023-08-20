using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class SliderSliding : ElympicsMonoBehaviour, IUpdatable
{
    [SerializeField] private RectTransform area;
    public ElympicsFloat speed = new ElympicsFloat();
    private int LWall = -2;
    private int RWall = 2;
    private bool right = true;
    void Start()
    {
        transform.localPosition = new Vector3(-2,0,0);
    }

    // Update is called once per frame
    public void ElympicsUpdate()
    {
        if(Elympics.IsServer)
        if (right)
        {
            transform.Translate(Vector3.right * 0.1f);
            if(transform.localPosition.x > 2) right = !right;
        }
        else
        {
            transform.Translate(Vector3.left * 0.1f);
            if(transform.localPosition.x < -2) right = !right;
        }
    }

    public void SetTask(float width)
    {
        if(!Elympics.IsServer) return;
        area.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal, width);
        float rand = Random.Range(-200, 200)/100;
        rand = Mathf.Clamp(rand,-2 + width/2, 2 - width/2);
        area.anchoredPosition = new Vector2(rand, 0);
    }

    public bool IsInside()
    {
        float borders = area.rect.width/2;
        Debug.Log(transform.localPosition.x + "   " + (area.transform.localPosition.x + borders));
        Debug.Log(transform.localPosition.x + "   " + (area.transform.localPosition.x - borders));
        if(transform.localPosition.x < area.transform.localPosition.x + borders &&
            transform.localPosition.x > area.transform.localPosition.x - borders) return true;
        return false;
    }
}
