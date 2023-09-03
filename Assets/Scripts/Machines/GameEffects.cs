using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class GameEffects : MonoBehaviour
{
    [SerializeField] private GameInitializer gameInitializer;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameFinisher gameFinisher;
    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private Light2D lobbyLight;

    private float fadeTime = 2;
    private int lastRememberedNumber = -1;
    private bool phase = false;
    void Start()
    {
        gameManager.CurrentGameState.ValueChanged += StateChanged;
    }

    public void Update()
    {
        switch(gameManager.CurrentGameState.Value)
        {
            case 0:
                PreMatch();
                break;
            case 1:
                break;
            case 2:
                End();
                break;
        }
        if (gameFinisher.startEnd.Value == true)
        {
            LoadCutScene();
            if(!phase) fadeTime -= Time.deltaTime;
            if(phase) fadeTime += Time.deltaTime;
        }
    }

    private void End()
    {
        GameObject.Find("QuitMenu").SetActive(true);
    }

    private void PreMatch()
    {
        if (gameInitializer.CurrentTimeToStartMatch.Value < 11)
        {
            textUI.gameObject.SetActive(true);
            if(lastRememberedNumber != (int)gameInitializer.CurrentTimeToStartMatch.Value)
            {
                lastRememberedNumber = (int)gameInitializer.CurrentTimeToStartMatch.Value;
                UpdateTimer();
            }
        }
    }

    private void UpdateTimer()
    {
        textUI.text = lastRememberedNumber.ToString();
    }

    private void StateChanged(int lastValue, int newValue)
    {
        if (lastValue == 0)
        {
            textUI.transform.parent.gameObject.SetActive(false);
            GameObject.Find("Light").GetComponent<Light2D>().intensity = 1;
            lobbyLight.intensity = 0f;
        }
    }


    private void LoadCutScene()
    {
        if(!phase)
        {
            Light2D light = GameObject.Find("Light").GetComponent<Light2D>();
            light.intensity = fadeTime / 2;
            if (light.intensity < 0)
            {
                phase = !phase;
                GameObject.Find("MainUI").transform.GetChild(9).gameObject.SetActive(true);
            }
        }
        else
        {
            var image = GameObject.Find("MainUI").transform.GetChild(9).GetComponent<Image>();
            var tempcolor = image.color;
            tempcolor.a = fadeTime / 2;
            image.color = tempcolor;
        }
    }
}
