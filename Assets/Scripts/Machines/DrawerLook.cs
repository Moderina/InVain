using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class DrawerLook : ElympicsMonoBehaviour
{
    //Machine UI
    [SerializeField] GameObject itemPanel;
    [SerializeField] private float duration = 0.5f;
    private Vector3 startPos;
    void Start()
    {
        startPos = itemPanel.transform.position;
    }

    private IEnumerator ShowDrawerUI(bool hiding)
    // public void Update()
    {
        Debug.Log(itemPanel.transform.position);
        //taskPanel.transform.position = new Vector3()
        // if(taskPanel.transform.position.y == startPos.y && hiding) yield break;
        Vector3 endPosition;

        float elapsedTime = 0f;
        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            
            if(hiding)
            {
                endPosition = startPos;
            }
            else
            {
                endPosition = startPos;
                endPosition.y += 175f;
            }
            Vector3 lerpedPos = Vector3.Lerp(itemPanel.transform.position, endPosition, elapsedTime/duration);
            itemPanel.transform.position = lerpedPos;

            yield return null;
        }
    }
}
