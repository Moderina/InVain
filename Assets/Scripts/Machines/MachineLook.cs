using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using System;
using Unity.VisualScripting;

public class MachineLook : ElympicsMonoBehaviour, IObservable
{
    //Machine UI
    [SerializeField] GameObject taskPanel;
    [SerializeField] private float duration = 0.5f;
    private Vector3 startPos;

    public void Start()
    {
        startPos = taskPanel.transform.position;
    }

    private IEnumerator ShowMachineUI(bool hiding)
    // public void Update()
    {
        Debug.Log(taskPanel.transform.position);
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
            Vector3 lerpedPos = Vector3.Lerp(taskPanel.transform.position, endPosition, elapsedTime/duration);
            taskPanel.transform.position = lerpedPos;

            yield return null;
        }
    }

    public void OnMachineInteracted(bool interaction)
    {
        Debug.Log("sadness");
        StartCoroutine(ShowMachineUI(interaction));
    }
}
