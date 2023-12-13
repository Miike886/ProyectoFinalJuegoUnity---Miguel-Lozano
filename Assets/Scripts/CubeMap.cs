using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeMap : MonoBehaviour
{
    private CubeState cubeState;

  
    public Transform up;
    public Transform down;
    public Transform left;
    public Transform right;
    public Transform front;
    public Transform back;

    void Start()
    {
        
    }

   
    void Update()
    {
        
    }

    // Método para establecer el mapeo de colores en el cubo.
    public void Set()
    {
        // Buscar e instanciar el estado actual del cubo.
        cubeState = FindObjectOfType<CubeState>();

        // Actualizar el mapeo para cada cara del cubo.
        UpdateMap(cubeState.front, front);
        UpdateMap(cubeState.back, back);
        UpdateMap(cubeState.left, left);
        UpdateMap(cubeState.right, right);
        UpdateMap(cubeState.up, up);
        UpdateMap(cubeState.down, down);
    }

    // Método para actualizar el mapeo de colores en una cara del cubo.
    void UpdateMap(List<GameObject> face, Transform side)
    {
        int i = 0;
        foreach (Transform map in side)
        {
            // Asignar colores según el nombre de las piezas en la cara.
            if (face[i].name[0] == 'F')
            {
                map.GetComponent<Image>().color = new Color(1, 0.5f, 0, 1); // Color naranja
            }
            if (face[i].name[0] == 'B')
            {
                map.GetComponent<Image>().color = Color.red; // Color rojo
            }
            if (face[i].name[0] == 'U')
            {
                map.GetComponent<Image>().color = Color.yellow; // Color amarillo
            }
            if (face[i].name[0] == 'D')
            {
                map.GetComponent<Image>().color = Color.white; // Color blanco
            }
            if (face[i].name[0] == 'L')
            {
                map.GetComponent<Image>().color = Color.green; // Color verde
            }
            if (face[i].name[0] == 'R')
            {
                map.GetComponent<Image>().color = Color.blue; // Color azul
            }
            i++;
        }               
    }
}
