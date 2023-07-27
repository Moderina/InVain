using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class GameManager : ElympicsMonoBehaviour
{
    public IEnumerator WaitToEnd()
    {
        yield return new WaitForSeconds(5);
        Elympics.EndGame();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
