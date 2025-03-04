using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestrictRotation : MonoBehaviour
{
    // Velocidad de ajuste de la rotación y posición
    public float rotationSpeed = 2.0f;
    public float positionSpeed = 2.0f;

    // Posiciones y rotaciones iniciales
    private Quaternion startingRotation;
    private Vector3 startingPosition;

    // Margen permitido para rotación y posición
    private const float allowedRotationOffset = 2f;
    private const float allowedPositionOffset = 0.3f;

    private void Start()
    {
        startingRotation = transform.rotation;
        startingPosition = transform.position;
    }

    void Update()
    {
        // Ajustar la rotación en el eje Y dentro del margen permitido
        float targetYRotation = ClampRotation(transform.eulerAngles.y, startingRotation.eulerAngles.y, allowedRotationOffset);

        // Ajustar la posición dentro del margen permitido
        Vector3 targetPosition = ClampPosition(transform.position, startingPosition, allowedPositionOffset);

        // Interpolación suave para rotación y posición
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, Mathf.LerpAngle(transform.eulerAngles.y, targetYRotation, Time.deltaTime * rotationSpeed), transform.eulerAngles.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * positionSpeed);
    }

    // Método para limitar la rotación en el eje Y dentro de un rango
    private float ClampRotation(float currentYRotation, float startingYRotation, float offset)
    {
        return Mathf.Clamp(currentYRotation, startingYRotation - offset, startingYRotation + offset);
    }

    // Método para limitar la posición dentro de un rango en los tres ejes
    private Vector3 ClampPosition(Vector3 currentPosition, Vector3 startingPosition, float offset)
    {
        return new Vector3(
            Mathf.Clamp(currentPosition.x, startingPosition.x - offset, startingPosition.x + offset),
            Mathf.Clamp(currentPosition.y, startingPosition.y - offset, startingPosition.y + offset),
            Mathf.Clamp(currentPosition.z, startingPosition.z - offset, startingPosition.z + offset)
        );
    }
}
