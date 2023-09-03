using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinterLook : MonoBehaviour
{
    //Machine UI
    [SerializeField] GameObject taskPanel;
    [SerializeField] private float duration = 0.5f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    public IEnumerator ShowPrinterUI(bool hiding)
    {
        Vector3 endPosition;

        float elapsedTime = 0f;
        if(transform.position.x > 0)
        {
            endPosition = startPos;
            if(!hiding)
            {
                endPosition.x -= 310f;
            } 
        }
        else
        {
            endPosition = -startPos;
            if(!hiding)
            {
                endPosition.x += 310f;
            }
        }

        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            Vector3 lerpedPos = Vector3.Lerp(taskPanel.transform.position, endPosition, elapsedTime/duration);
            taskPanel.transform.position = lerpedPos;

            yield return null;
        }
    }

    public void OnPrinterInteracted(bool interaction)
    {
        StartCoroutine(ShowPrinterUI(interaction));
    }
}
