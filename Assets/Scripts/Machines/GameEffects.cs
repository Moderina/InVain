using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameEffects : MonoBehaviour
{
    [SerializeField] private GameInitializer gameInitializer;
    [SerializeField] private GameManager gameManager;

    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private Light2D lobbyLight;

    private int lastRememberedNumber = -1;
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
        }
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
            GameObject.Find("Light 2D").GetComponent<Light2D>().intensity = 1;
            lobbyLight.intensity = 0f;
        }
    }
}
