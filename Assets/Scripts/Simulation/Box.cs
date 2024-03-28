using UnityEngine;
public class Box : MonoBehaviour
{
    // Declaración de la variable privada y serializada que almacena la velocidad actual
    [SerializeField] private float currentSpeed = 0f;

    // Referencia a la cinta transportadora
    ConveyorBelt belt;

    // Variable que indica si la caja está en la cinta transportadora
    bool isInBelt = false;

    // Método llamado al inicio
    private void Start()
    {
        // Obtener la referencia a la cinta transportadora utilizando una etiqueta
        belt = GameObject.FindGameObjectWithTag("Belt").GetComponent<ConveyorBelt>();
    }

    // Método llamado cuando se detecta una colisión
    private void OnCollisionEnter(Collision collision)
    {
        // Verificar si la colisión es con un objeto etiquetado como "Belt"
        if (collision.collider.CompareTag("Belt"))
            isInBelt = true;
    }

    // Método llamado cuando se sale de una colisión
    private void OnCollisionExit(Collision collision)
    {
        // Verificar si la colisión es con un objeto etiquetado como "Belt"
        if (collision.collider.CompareTag("Belt"))
            isInBelt = false;
    }

    // Método llamado en cada fotograma
    private void Update()
    {
        // Salir si la caja no está en la cinta transportadora
        if (!isInBelt) return;

        // Mover el GameObject en la dirección positiva del eje X
        MoveGameObject();

        // Actualizar la velocidad actual de la caja según la velocidad de la cinta transportadora
        currentSpeed = belt.GetSpeed();
    }

    // Método para mover el GameObject en la dirección obtenida de belt.GetDirection()
    private void MoveGameObject()
    {
        // Obtener la posición actual del GameObject
        Vector3 currentPosition = transform.position;

        // Obtener la dirección de movimiento desde belt.GetDirection()
        Vector3 movementDirection = belt.GetDirection();

        // Calcular el desplazamiento en la dirección obtenida
        Vector3 movement = currentSpeed * Time.deltaTime * movementDirection;

        // Calcular la nueva posición sumando el desplazamiento a la posición actual
        Vector3 newPosition = currentPosition + movement;

        // Asignar la nueva posición al GameObject
        transform.position = newPosition;
    }

}
