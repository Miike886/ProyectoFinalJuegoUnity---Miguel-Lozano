using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kociemba;

public class SolveTwoPhase : MonoBehaviour
{
    public ReadCube readCube;   // Referencia al script ReadCube
    public CubeState cubeState; // Referencia al script CubeState
    private bool doOnce = true;  // Variable de control para realizar una acción una vez

    // Se llama al inicio antes del primer fotograma
    void Start()
    {
        // Encuentra instancias de ReadCube y CubeState en la escena
        readCube = FindObjectOfType<ReadCube>();
        cubeState = FindObjectOfType<CubeState>();
    }

    // Se llama una vez por fotograma
    void Update()
    {
        // Si el cubo ha comenzado y aún no se ha realizado la acción
        if (CubeState.started && doOnce)
        {
            // Marca que la acción ya se ha realizado
            doOnce = false;
            // Llama a la función Solver
            Solver();
        }
    }

    // Resuelve el cubo y automatiza la lista de movimientos
    public void Solver()
    {
        // Lee el estado actual del cubo
        readCube.ReadState();

        // Obtiene el estado del cubo como una cadena de texto
        string moveString = cubeState.GetStateString();
        print(moveString);

        // Resuelve el cubo
        string info = "";

        // Primera vez, construye las tablas
        string solution = SearchRunTime.solution(moveString, out info, buildTables: true);

        // En cada otra ocasión
        //string solution = Search.solution(moveString, out info);

        // Convierte los movimientos resueltos de una cadena a una lista
        List<string> solutionList = StringToList(solution);

        // Automatiza la lista de movimientos
        Automate.moveList = solutionList;

        print(info);

        FindObjectOfType<Timer>().StopTimerIfCubeSolved();
    }

    // Convierte una cadena de movimientos en una lista de movimientos
    List<string> StringToList(string solution)
    {
        List<string> solutionList = new List<string>(solution.Split(new string[] { " " }, System.StringSplitOptions.RemoveEmptyEntries));
        return solutionList;
    }
}