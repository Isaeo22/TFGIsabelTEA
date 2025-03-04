using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    [Header("Configuraci�n del Diccionario de Misiones")]
    [SerializeField] public List<Image> statList;
    [SerializeField]List<string> interactionNames = new List<string>();
    [SerializeField] TextMeshProUGUI contactoVisualText;

    //QUITAR CUANDO MODIFIQUE TODO
    public List<Image> chatGPTStatList;

    public Dictionary<string, Image> missionStatsDictionary; // Diccionario que vincula el nombre de las misiones con sus im�genes en la UI.

    [Header("Configuraci�n de Im�genes")]
    public Sprite spriteCheck; // Imagen que se usar� para marcar una misi�n como completada.

    

    public void InitializeMissionDictionary()
    {
        missionStatsDictionary = new Dictionary<string, Image>();

        // Verificar que la lista de im�genes coincida con la cantidad de misiones.
        if (statList.Count == interactionNames.Count)
        {
            for (int i = 0; i < statList.Count; i++)
            {
                missionStatsDictionary[interactionNames[i]] = statList[i];
            }
        }
        else
        {
            Debug.LogError("La cantidad de im�genes en la lista no coincide con la cantidad de misiones.");
        }
    }

    // M�todo para actualizar la imagen de una misi�n espec�fica.
    public void UpdateStatImage(string missionName)
    {
        if (missionStatsDictionary.ContainsKey(missionName))
        {
            missionStatsDictionary[missionName].sprite = spriteCheck;
            missionStatsDictionary[missionName].color = Color.white; // Cambia el color de la imagen a blanco para indicar que est� activada.
        }
        else
        {
            Debug.LogWarning($"No se encontr� la misi�n con nombre {missionName} en el diccionario.");
        }
    }


    public void SetEyetrackingPercentage(float p)
    {
        contactoVisualText.text = (int)p + "%";

        if (p < 35)
        {
            contactoVisualText.color = new Color(1f, 0.6f, 0.6f);// Rojo si es menor que 35
        }
        else if (p >= 35 && p <= 55)
        {
            contactoVisualText.color = new Color(1f, 1f, 0.6f); // Amarillo si est� entre 35 y 55
        }
        else if (p > 55)
        {
            contactoVisualText.color = new Color(0.6f, 1f, 0.6f); // Verde si es mayor de 55
        }

    }
}
