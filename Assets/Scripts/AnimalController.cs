using UnityEditor;
using UnityEngine;

public class AnimalController: MonoBehaviour
{
   public float speed = 6.0f;         // Velocidad de movimiento
    public float turnSpeed = 3.0f;     // Velocidad de giro
    public float gravity = 20.0f;      // Fuerza de gravedad
    public float slopeForce = 6.0f;    // Fuerza adicional para subir pendientes
    public float slopeForceRayLength = 1.5f; // Longitud del raycast para detectar pendientes
    public float stepHeight = 0.5f;    // Altura de los pasos para subir escaleras
    public float stepRayDistance = 0.3f; // Distancia para detectar el paso

    public float defaultSlopeLimit = 45f; // Límite de inclinación predeterminado
    public float maxSlopeLimit = 90f;     // Límite máximo de inclinación para escaleras

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        controller.slopeLimit = defaultSlopeLimit;
    }

    void Update()
    {
        // Obtener la entrada del usuario para el movimiento
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Calcular la dirección de movimiento en función de la entrada
        moveDirection = new Vector3(moveHorizontal, 0.0f, moveVertical);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;

        // Rotar el animal en la dirección de movimiento
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
        }

        // Ajustar dinámicamente el slope limit
        AdjustSlopeLimit();

        // Aplicar gravedad
        if (controller.isGrounded)
        {
            moveDirection.y = -0.5f; // Pequeña fuerza hacia abajo para mantener al personaje pegado al suelo
        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Ajustar la dirección de movimiento para subir pendientes y escaleras
        if (OnSlope())
        {
            moveDirection.y = slopeForce; // Aplicar fuerza hacia arriba para subir la pendiente
        }
        else if (OnStep())
        {
            moveDirection.y = stepHeight; // Aplicar fuerza hacia arriba para subir los escalones
        }

        // Mover el controlador
        controller.Move(moveDirection * Time.deltaTime);
    }

    private void AdjustSlopeLimit()
    {
        RaycastHit hit;
        Vector3 rayStart = transform.position + Vector3.up * 0.1f; // Levantar un poco el raycast
        if (Physics.Raycast(rayStart, Vector3.down, out hit, slopeForceRayLength))
        {
            float angle = Vector3.Angle(hit.normal, Vector3.up);
            if (angle > defaultSlopeLimit && angle < 90) // Solo considerar ángulos entre el límite de inclinación y 90 grados
            {
                controller.slopeLimit = maxSlopeLimit;
            }
            else
            {
                controller.slopeLimit = defaultSlopeLimit;
            }
        }
        Debug.DrawRay(rayStart, Vector3.down * slopeForceRayLength, Color.green); // Dibujar el raycast
    }

    private bool OnSlope()
    {
        RaycastHit hit;
        Vector3 rayStart = transform.position + Vector3.up * 0.1f; // Levantar un poco el raycast
        if (Physics.Raycast(rayStart, Vector3.down, out hit, slopeForceRayLength))
        {
            float angle = Vector3.Angle(hit.normal, Vector3.up);
            return angle > defaultSlopeLimit && angle < 90; // Solo considerar ángulos entre el límite de inclinación y 90 grados
        }
        return false;
    }

    private bool OnStep()
    {
        if (controller.isGrounded)
        {
            RaycastHit hit;
            Vector3 rayStart = transform.position + Vector3.up * stepHeight;
            if (Physics.Raycast(rayStart, transform.forward, out hit, stepRayDistance))
            {
                if (hit.distance <= stepRayDistance)
                {
                    return true;
                }
            }
            Debug.DrawRay(rayStart, transform.forward * stepRayDistance, Color.blue); // Dibujar el raycast
        }
        return false;
    }
}