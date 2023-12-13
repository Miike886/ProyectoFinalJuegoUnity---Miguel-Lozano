using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Automate : MonoBehaviour
{
    // Lista que almacenará la secuencia de movimientos a realizar.
    public static List<string> moveList = new List<string>() { };
    private Timer timer; 
    // Lista de todos los posibles movimientos en el cubo.
    private readonly List<string> allMoves = new List<string>()
        { "U", "D", "L", "R", "F", "B",
          "U2", "D2", "L2", "R2", "F2", "B2",
          "U'", "D'", "L'", "R'", "F'", "B'" 
        };

    private CubeState cubeState;
    private ReadCube readCube;

    // Método llamado al inicio antes del primer frame.
    void Start()
    {
        timer = FindObjectOfType<Timer>();
        // Buscar y asignar instancias de CubeState y ReadCube.
        cubeState = FindObjectOfType<CubeState>();
        readCube = FindObjectOfType<ReadCube>();
    }

    // Método llamado en cada frame.
    void Update()
    {
        // Verificar si hay movimientos en la lista, no hay rotación automática y el cubo ha iniciado.
        if (moveList.Count > 0 && !CubeState.autoRotating && CubeState.started)
        {
            // Realizar el movimiento en el primer índice.
            DoMove(moveList[0]);

            // Eliminar el movimiento en el primer índice.
            moveList.Remove(moveList[0]);
        }
    }

    // Método para revolver el cubo con movimientos aleatorios.
    public void Shuffle()
    {
        List<string> moves = new List<string>();
        int shuffleLength = Random.Range(10, 30);

        // Inicia el temporizador después de 5 segundos
        timer.StartTimerExternally();

        for (int i = 0; i < shuffleLength; i++)
        {
            int randomMove = Random.Range(0, allMoves.Count);
            moves.Add(allMoves[randomMove]);
        }
        moveList = moves;
    }

    // Método para realizar un movimiento específico en el cubo.
    void DoMove(string move)
    {
        // Leer y actualizar el estado del cubo.
        readCube.ReadState();
        CubeState.autoRotating = true;

        // Realizar la rotación según el movimiento especificado.
        if (move == "U")
        {
            RotateSide(cubeState.up, -90);
        }
        if (move == "U'")
        {
            RotateSide(cubeState.up, 90);
        }
        if (move == "U2")
        {
            RotateSide(cubeState.up, -180);
        }
        if (move == "D")
        {
            RotateSide(cubeState.down, -90);
        }
        if (move == "D'")
        {
            RotateSide(cubeState.down, 90);
        }
        if (move == "D2")
        {
            RotateSide(cubeState.down, -180);
        }
        if (move == "L")
        {
            RotateSide(cubeState.left, -90);
        }
        if (move == "L'")
        {
            RotateSide(cubeState.left, 90);
        }
        if (move == "L2")
        {
            RotateSide(cubeState.left, -180);
        }
        if (move == "R")
        {
            RotateSide(cubeState.right, -90);
        }
        if (move == "R'")
        {
            RotateSide(cubeState.right, 90);
        }
        if (move == "R2")
        {
            RotateSide(cubeState.right, -180);
        }
        if (move == "F")
        {
            RotateSide(cubeState.front, -90);
        }
        if (move == "F'")
        {
            RotateSide(cubeState.front, 90);
        }
        if (move == "F2")
        {
            RotateSide(cubeState.front, -180);
        }
        if (move == "B")
        {
            RotateSide(cubeState.back, -90);
        }
        if (move == "B'")
        {
            RotateSide(cubeState.back, 90);
        }
        if (move == "B2")
        {
            RotateSide(cubeState.back, -180);
        }
    }

    // Método para rotar un lado del cubo.
    void RotateSide(List<GameObject> side, float angle)
    {
        // Rotar automáticamente el lado por el ángulo especificado.
        PivotRotation pr = side[4].transform.parent.GetComponent<PivotRotation>();
        pr.StartAutoRotate(side, angle);        
    }
}