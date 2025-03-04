using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CovertCanvasToWorldSpace : MonoBehaviour
{
    
        public Canvas canvas; // Asigna el Canvas desde el Inspector
    public Transform playerCamera; // Asigna la cámara del jugador en VR
    public float distanceFromPlayer = 2.0f; // Distancia del canvas al jugador
    public float heightOffset = 0.5f; // Ajuste de altura del canvas

   

    void Update()
    {
            // Hace que el Canvas siga la cámara del jugador en VR
            Vector3 targetPosition = playerCamera.position + playerCamera.forward * distanceFromPlayer;
            targetPosition.y += heightOffset; // Ajusta la altura
            canvas.transform.position = Vector3.Lerp(canvas.transform.position, targetPosition, Time.deltaTime * 5f); // Movimiento suave
            canvas.transform.LookAt(playerCamera); // Hace que el Canvas mire al jugador
            canvas.transform.rotation = Quaternion.LookRotation(canvas.transform.position - playerCamera.position);
        
    }
}

