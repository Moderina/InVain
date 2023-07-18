using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    public AudioMixer audioMixer;
    public void SetEffectsVolume (float volume) {
        Debug.Log(volume);
        audioMixer.SetFloat("MainVolume", volume);
    }

    public void Quit() {
        SceneManager.LoadScene(0);
    }
}
