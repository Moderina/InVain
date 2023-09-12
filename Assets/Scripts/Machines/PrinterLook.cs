using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinterLook : MonoBehaviour
{
    //Machine UI
    [SerializeField] GameObject taskPanel;
    [SerializeField] private float duration = 5f;
    private Vector3 startPos;

    void Start()
    {
        startPos = taskPanel.GetComponent<RectTransform>().anchoredPosition;
        Debug.Log("lonely" + taskPanel.GetComponent<RectTransform>().anchoredPosition);
    }

    public IEnumerator ShowPrinterUI(bool hiding)
    {
        Vector3 endPosition;

        float elapsedTime = 0f;
        if(taskPanel.GetComponent<RectTransform>().anchoredPosition.x > 0)
        {
            endPosition = startPos;
            if(!hiding)
            {
                endPosition.x -= 300f;
            } 
        }
        else
        {
            endPosition = -startPos;
            if(!hiding)
            {
                endPosition.x += 300f;
            }
        }
        Debug.Log("dug this grave " + endPosition.x);

        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            Vector3 lerpedPos = Vector3.Lerp(taskPanel.GetComponent<RectTransform>().anchoredPosition, endPosition, elapsedTime/duration);
            taskPanel.GetComponent<RectTransform>().anchoredPosition = lerpedPos;

            yield return null;
        }
    }

    public void OnPrinterInteracted(bool interaction)
    {
        StartCoroutine(ShowPrinterUI(interaction));
    }
}
