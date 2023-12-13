using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectFace : MonoBehaviour
{
    private CubeState cubeState;  // Estado actual del cubo
    private ReadCube readCube;    // Referencia al script ReadCube
    private int layerMask = 1 << 8;  // Máscara de capa para las caras del cubo

    // Se llama al inicio antes del primer fotograma
    void Start()
    {
        // Encuentra una instancia de ReadCube y CubeState en la escena
        readCube = FindObjectOfType<ReadCube>();
        cubeState = FindObjectOfType<CubeState>();
    }

    // Se llama una vez por fotograma
    void Update()
    {
        // Si se hace clic izquierdo del ratón y no se está auto-rotando
        if (Input.GetMouseButtonDown(0) && !CubeState.autoRotating)
        {
            // Lee el estado actual del cubo
            readCube.ReadState();

            // Rayo desde el ratón hacia el cubo para ver si se golpea una cara
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f, layerMask))
            {
                GameObject face = hit.collider.gameObject;
                // Crea una lista de todas las caras (listas de GameObjects de caras)
                List<List<GameObject>> cubeSides = new List<List<GameObject>>()
                {
                    cubeState.up,
                    cubeState.down,
                    cubeState.left,
                    cubeState.right,
                    cubeState.front,
                    cubeState.back
                };
                // Si la cara golpeada existe dentro de un lado
                foreach (List<GameObject> cubeSide in cubeSides)
                {
                    if (cubeSide.Contains(face))
                    {
                        // Recoge la cara
                        cubeState.PickUp(cubeSide);
                        // Inicia la lógica de rotación del lado
                        cubeSide[4].transform.parent.GetComponent<PivotRotation>().Rotate(cubeSide);
                    }
                }
            }
        }
    }
}