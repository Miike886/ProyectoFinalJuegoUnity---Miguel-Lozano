
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadCube : MonoBehaviour
{
    // Transformaciones para cada cara del cubo
    public Transform tUp;       // Cara superior
    public Transform tDown;     // Cara inferior
    public Transform tLeft;     // Cara izquierda
    public Transform tRight;    // Cara derecha
    public Transform tFront;    // Cara frontal
    public Transform tBack;     // Cara trasera

    // Listas de rayos para cada cara del cubo
    private List<GameObject> frontRays = new List<GameObject>();
    private List<GameObject> backRays = new List<GameObject>();
    private List<GameObject> upRays = new List<GameObject>();
    private List<GameObject> downRays = new List<GameObject>();
    private List<GameObject> leftRays = new List<GameObject>();
    private List<GameObject> rightRays = new List<GameObject>();

    private int layerMask = 1 << 8; // Esta máscara de capa es solo para las caras del cubo
    CubeState cubeState;
    CubeMap cubeMap;
    public GameObject emptyGO;

    // Se llama al inicio antes del primer fotograma
    void Start()
    {
        // Configura las transformaciones de los rayos
        SetRayTransforms();

        // Encuentra instancias de CubeState y CubeMap en la escena
        cubeState = FindObjectOfType<CubeState>();
        cubeMap = FindObjectOfType<CubeMap>();

        // Lee el estado actual del cubo
        ReadState();

        // Establece que el cubo ha comenzado
        CubeState.started = true;
    }

    // Se llama una vez por fotograma
    void Update()
    {
        // No realiza ninguna acción en la actualización
    }

    // Lee el estado actual del cubo
    public void ReadState()
    {
        // Encuentra instancias de CubeState y CubeMap en la escena
        cubeState = FindObjectOfType<CubeState>();
        cubeMap = FindObjectOfType<CubeMap>();

        // Establece el estado de cada posición en la lista de caras para saber
        // qué color está en qué posición
        cubeState.up = ReadFace(upRays, tUp);
        cubeState.down = ReadFace(downRays, tDown);
        cubeState.left = ReadFace(leftRays, tLeft);
        cubeState.right = ReadFace(rightRays, tRight);
        cubeState.front = ReadFace(frontRays, tFront);
        cubeState.back = ReadFace(backRays, tBack);

        // Actualiza el mapa con las posiciones encontradas
        cubeMap.Set();
    }

    // Configura las transformaciones de los rayos
    void SetRayTransforms()
    {
        // Rellena las listas de rayos con rayos que se emiten desde la transformación, inclinados hacia el cubo.
        upRays = BuildRays(tUp, new Vector3(90, 90, 0));
        downRays = BuildRays(tDown, new Vector3(270, 90, 0));
        leftRays = BuildRays(tLeft, new Vector3(0, 180, 0));
        rightRays = BuildRays(tRight, new Vector3(0, 0, 0));
        frontRays = BuildRays(tFront, new Vector3(0, 90, 0));
        backRays = BuildRays(tBack, new Vector3(0, 270, 0));
    }

    // Construye rayos desde la transformación en una dirección específica
    List<GameObject> BuildRays(Transform rayTransform, Vector3 direction)
    {
        // El recuento de rayos se utiliza para nombrar los rayos para asegurarse de que estén en el orden correcto.
        int rayCount = 0;
        List<GameObject> rays = new List<GameObject>();

        // Esto crea 9 rayos en la forma del lado del cubo con
        // Ray 0 en la esquina superior izquierda y Ray 8 en la esquina inferior derecha:
        //  |0|1|2|
        //  |3|4|5|
        //  |6|7|8|

        for (int y = 1; y > -2; y--)
        {
            for (int x = -1; x < 2; x++)
            {
                Vector3 startPos = new Vector3(rayTransform.localPosition.x + x,
                                               rayTransform.localPosition.y + y,
                                               rayTransform.localPosition.z);
                GameObject rayStart = Instantiate(emptyGO, startPos, Quaternion.identity, rayTransform);
                rayStart.name = rayCount.ToString();
                rays.Add(rayStart);
                rayCount++;
            }
        }
        rayTransform.localRotation = Quaternion.Euler(direction);
        return rays;
    }

    // Lee la cara del cubo usando rayos y devuelve la lista de objetos golpeados
    public List<GameObject> ReadFace(List<GameObject> rayStarts, Transform rayTransform)
    {
        List<GameObject> facesHit = new List<GameObject>();

        foreach (GameObject rayStart in rayStarts)
        {
            Vector3 ray = rayStart.transform.position;
            RaycastHit hit;

            // ¿El rayo interseca algún objeto en la máscara de capa?
            if (Physics.Raycast(ray, rayTransform.forward, out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(ray, rayTransform.forward * hit.distance, Color.yellow);
                facesHit.Add(hit.collider.gameObject);
                //print(hit.collider.gameObject.name);
            }
            else
            {
                Debug.DrawRay(ray, rayTransform.forward * 1000, Color.green);
            }
        }
        return facesHit;
    }
}
