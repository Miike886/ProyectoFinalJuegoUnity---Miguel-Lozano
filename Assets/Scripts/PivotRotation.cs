
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotRotation : MonoBehaviour
{
    private List<GameObject> activeSide;
    private Vector3 localForward;
    private Vector3 mouseRef;
    private bool dragging = false;

    private bool autoRotating = false;
    private float sensitivity = 0.4f;
    private float speed = 300f;
    private Vector3 rotation;

    private Quaternion targetQuaternion;

    private ReadCube readCube;
    private CubeState cubeState;
       
    // Método llamado al inicio antes del primer frame.
    void Start()
    {
        readCube = FindObjectOfType<ReadCube>();
        cubeState = FindObjectOfType<CubeState>();
    }

    // Método llamado en cada frame después de Update.
    void LateUpdate()
    {
        // Si está arrastrando y no está rotando automáticamente.
        if (dragging && !autoRotating)
        {
            // Girar el lado activo.
            SpinSide(activeSide);

            // Si se suelta el botón del ratón, detener el arrastre y rotar al ángulo correcto.
            if (Input.GetMouseButtonUp(0))
            {
                dragging = false;
                RotateToRightAngle();
            }
        }

        // Si está rotando automáticamente.
        if (autoRotating)
        {
            AutoRotate();
        }                
    }

    // Método para girar un lado del cubo en respuesta al movimiento del ratón.
    private void SpinSide(List<GameObject> side)
    {
        // Resetear la rotación.
        rotation = Vector3.zero;

        // Calcular el desplazamiento del ratón desde la última posición.
        Vector3 mouseOffset = (Input.mousePosition - mouseRef);        

        // Determinar la dirección de la rotación según el lado del cubo.
        if (side == cubeState.up)
        {
            rotation.y = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        }
        if (side == cubeState.down)
        {
            rotation.y = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        }
        if (side == cubeState.left)
        {
            rotation.z = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        }
        if (side == cubeState.right)
        {
            rotation.z = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        }
        if (side == cubeState.front)
        {
            rotation.x = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        }
        if (side == cubeState.back)
        {
            rotation.x = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        }

        // Aplicar la rotación.
        transform.Rotate(rotation, Space.Self);

        // Almacenar la posición del ratón.
        mouseRef = Input.mousePosition;
    }

    // Método para iniciar la rotación manual de un lado del cubo.
    public void Rotate(List<GameObject> side)
    {
        activeSide = side;
        mouseRef = Input.mousePosition;
        dragging = true;

        // Crear un vector para rotar alrededor.
        localForward = Vector3.zero - side[4].transform.parent.transform.localPosition;
    }

    // Método para iniciar la rotación automática de un lado del cubo.
    public void StartAutoRotate(List<GameObject> side, float angle)
    {
        cubeState.PickUp(side);
        Vector3 localForward = Vector3.zero - side[4].transform.parent.transform.localPosition;
        targetQuaternion = Quaternion.AngleAxis(angle, localForward) * transform.localRotation;
        activeSide = side;
        autoRotating = true;
    }

    // Método para rotar al ángulo correcto al soltar el ratón.
    public void RotateToRightAngle()
    {
        Vector3 vec = transform.localEulerAngles;
        // Redondear los ángulos al múltiplo de 90 grados más cercano.
        vec.x = Mathf.Round(vec.x / 90) * 90;
        vec.y = Mathf.Round(vec.y / 90) * 90;
        vec.z = Mathf.Round(vec.z / 90) * 90;

        targetQuaternion.eulerAngles = vec;
        autoRotating = true;
    }

    // Método para realizar la rotación automática.
    private void AutoRotate()
    {
        dragging = false;
        var step = speed * Time.deltaTime;
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetQuaternion, step);

        // Si está a menos de un grado, establecer el ángulo objetivo y finalizar la rotación.
        if (Quaternion.Angle(transform.localRotation, targetQuaternion) <= 1)
        {
            transform.localRotation = targetQuaternion;
            // Desvincular los cubitos.
            cubeState.PutDown(activeSide, transform.parent);
            readCube.ReadState();
            CubeState.autoRotating = false;
            autoRotating = false;
            dragging = false;                                                               
        }
    }         
}
