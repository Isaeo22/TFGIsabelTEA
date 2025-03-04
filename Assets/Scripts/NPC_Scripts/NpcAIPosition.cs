using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAIPosition : MonoBehaviour
{
    [SerializeField]public  Transform OpenAIPosition;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Start()
    {
        if (OpenAIPosition != null)
        {
            initialPosition = OpenAIPosition.position; // Guarda la posición inicial
            initialRotation = OpenAIPosition.rotation; // Guarda la rotación inicial
        }
        else
        {
            Debug.LogError("OpenAIPosition no está asignado en " + gameObject.name);
        }
    }

    // Método para obtener la posición inicial desde otro script
    public Vector3 GetOpenAIPosition()
    {
        return initialPosition;
    }
    public Quaternion GetOpenAIRotation()
    {
        return initialRotation;
    }
}
