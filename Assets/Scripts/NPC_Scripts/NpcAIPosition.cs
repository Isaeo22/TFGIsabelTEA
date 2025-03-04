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
            initialPosition = OpenAIPosition.position; // Guarda la posici�n inicial
            initialRotation = OpenAIPosition.rotation; // Guarda la rotaci�n inicial
        }
        else
        {
            Debug.LogError("OpenAIPosition no est� asignado en " + gameObject.name);
        }
    }

    // M�todo para obtener la posici�n inicial desde otro script
    public Vector3 GetOpenAIPosition()
    {
        return initialPosition;
    }
    public Quaternion GetOpenAIRotation()
    {
        return initialRotation;
    }
}
