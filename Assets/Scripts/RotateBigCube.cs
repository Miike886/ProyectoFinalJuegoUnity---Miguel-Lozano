using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBigCube : MonoBehaviour
{
    private Vector2 firstPressPos;         // Posición del primer clic del mouse
    private Vector2 secondPressPos;        // Posición del segundo clic del mouse
    private Vector2 currentSwipe;           // Dirección del deslizamiento actual
    private Vector3 previousMousePosition;  // Posición anterior del mouse
    private Vector3 mouseDelta;             // Cambio en la posición del mouse
    private float speed = 200f;             // Velocidad de rotación
    public GameObject target;               // Objeto objetivo para rotar

    // Se llama al inicio antes del primer fotograma
    void Start()
    {
        // No realiza ninguna acción en el inicio
    }

    // Se llama una vez por fotograma
    void Update()
    {
        // Llama a las funciones Swipe() y Drag() en cada fotograma
        Swipe();
        Drag();
    }

    // Permite arrastrar el cubo o rotarlo automáticamente hacia una posición objetivo
    void Drag()
    {
        if (Input.GetMouseButton(1))
        {
            // Mientras el botón derecho del mouse esté presionado, el cubo se puede mover alrededor de su eje central para proporcionar retroalimentación visual
            mouseDelta = Input.mousePosition - previousMousePosition;
            mouseDelta *= 0.1f; // Reducción de la velocidad de rotación
            transform.rotation = Quaternion.Euler(mouseDelta.y, -mouseDelta.x, 0) * transform.rotation;
        }
        else
        {
            // Mover automáticamente hacia la posición objetivo
            if (transform.rotation != target.transform.rotation)
            {
                var step = speed * Time.deltaTime;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, target.transform.rotation, step);
            }
        }
        previousMousePosition = Input.mousePosition;
    }

    // Detecta gestos de deslizamiento del mouse y rota el objetivo en consecuencia
    void Swipe()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // Obtener la posición 2D del primer clic del mouse
            firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        if (Input.GetMouseButtonUp(1))
        {
            // Obtener la posición 2D del segundo clic del mouse
            secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            // Crear un vector a partir de las posiciones de los clics primero y segundo
            currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

            // Normalizar el vector 2D
            currentSwipe.Normalize();

            // Rotar el objetivo según la dirección del deslizamiento
            if (LeftSwipe(currentSwipe))
            {
                target.transform.Rotate(0, 90, 0, Space.World);
            }
            else if (RightSwipe(currentSwipe))
            {
                target.transform.Rotate(0, -90, 0, Space.World);
            }
            else if (UpLeftSwipe(currentSwipe))
            {
                target.transform.Rotate(90, 0, 0, Space.World);
            }
            else if (UpRightSwipe(currentSwipe))
            {
                target.transform.Rotate(0, 0, -90, Space.World);
            }
            else if (DownLeftSwipe(currentSwipe))
            {
                target.transform.Rotate(0, 0, 90, Space.World);
            }
            else if (DownRightSwipe(currentSwipe))
            {
                target.transform.Rotate(-90, 0, 0, Space.World);
            }
        }
    }

    // Funciones que verifican la dirección del deslizamiento para determinar si es un deslizamiento hacia la izquierda, derecha, arriba, abajo, etc.
    bool LeftSwipe(Vector2 swipe)
    {
        return currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f;
    }

    bool RightSwipe(Vector2 swipe)
    {
        return currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f;
    }

    bool UpLeftSwipe(Vector2 swipe)
    {
        return currentSwipe.y > 0 && currentSwipe.x < 0f;
    }

    bool UpRightSwipe(Vector2 swipe)
    {
        return currentSwipe.y > 0 && currentSwipe.x > 0f;
    }

    bool DownLeftSwipe(Vector2 swipe)
    {
        return currentSwipe.y < 0 && currentSwipe.x < 0f;
    }

    bool DownRightSwipe(Vector2 swipe)
    {
        return currentSwipe.y < 0 && currentSwipe.x > 0f;
    }
}
