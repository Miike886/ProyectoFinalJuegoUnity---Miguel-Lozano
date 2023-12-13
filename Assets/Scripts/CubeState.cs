using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeState : MonoBehaviour
{
    
    
    // Caras del cubo
    public List<GameObject> front = new List<GameObject>();
    public List<GameObject> back = new List<GameObject>();
    public List<GameObject> up = new List<GameObject>();
    public List<GameObject> down = new List<GameObject>();
    public List<GameObject> left = new List<GameObject>();
    public List<GameObject> right = new List<GameObject>();

    // Variables de control de rotación automática y estado de inicio.
    public static bool autoRotating = false;
    public static bool started = false;

    
    void Start()
    {

    }

    void Update()
    {
        
    }

    // Método para recoger las piezas de un lado del cubo.
    public void PickUp(List<GameObject> cubeSide)
    {
        foreach (GameObject face in cubeSide)
        {
            // Adjuntar el padre de cada pieza (el cubito)
            // al padre del índice 4 (el cubito en el medio) 
            // a menos que ya sea el índice 4
            if (face != cubeSide[4])
            {
                face.transform.parent.transform.parent = cubeSide[4].transform.parent;
            }
        }
    }    

    // Método para colocar las piezas en un lado del cubo.
    public void PutDown(List<GameObject> littleCubes, Transform pivot)
    {
        foreach (GameObject littleCube in littleCubes)
        {
            if (littleCube != littleCubes[4])
            {
                littleCube.transform.parent.transform.parent = pivot;
            }
        }
    }

    // Método para obtener una cadena de caracteres que representa una cara del cubo.
    string GetSideString(List<GameObject> side)
    {
    string sideString = "";
    foreach (GameObject face in side)
        {
        sideString += face.name[0].ToString().ToUpper();
        }
    return sideString;
    }
    
    public bool IsCubeSolved()
    {
        // Obtén el estado actual del cubo como una cadena
        string currentState = GetStateString();

        // Define el estado resuelto del cubo (puedes ajustarlo según tu lógica)
        string solvedState = "UUUUUUUUURRRRRRRRRFFFFFFFFFDDDDDDDDDLLLLLLLLLBBBBBBBBB";

        // Compara el estado actual con el estado resuelto
        return currentState.Equals(solvedState);
    }

    // Método para obtener una cadena de caracteres que representa el estado completo del cubo.
    public string GetStateString()
    {
        string stateString = "";
        stateString += GetSideString(up);
        stateString += GetSideString(right);
        stateString += GetSideString(front);
        stateString += GetSideString(down);
        stateString += GetSideString(left);
        stateString += GetSideString(back);
        return stateString;
    }
}