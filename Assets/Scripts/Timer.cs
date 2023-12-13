using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    private float timeElapsed;
    private int minutes, seconds, cents;

    private bool isRunning = false;

    void Update()
    {
        if (isRunning)
        {
            timeElapsed += Time.deltaTime;
            minutes = (int)(timeElapsed / 60f);
            seconds = (int)(timeElapsed - minutes * 60f);
            cents = (int)((timeElapsed - (int)timeElapsed) * 100f);

            timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, cents);
        }
        StopTimerIfCubeSolved();
    }

    public void StartTimer()
    {
        isRunning = true;
        timeElapsed = 0f;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    // Método para detener el temporizador si el cubo está resuelto
    public void StopTimerIfCubeSolved()
    {
        CubeState cubeState = FindObjectOfType<CubeState>();
        if (cubeState != null && cubeState.IsCubeSolved())
        {
            StopTimer();
        }
    }

    // Método para iniciar el temporizador después de revolver el cubo externamente
    public void StartTimerExternally()
    {
        // Verificar si el temporizador no está en ejecución
        if (!isRunning)
        {
            // Invocar el inicio del temporizador después de 5 segundos
            Invoke("StartTimer", 7f);
        }
    }

    // Método para verificar si el temporizador está en ejecución
    public bool IsTimerRunning()
    {
        return isRunning;
    }
}