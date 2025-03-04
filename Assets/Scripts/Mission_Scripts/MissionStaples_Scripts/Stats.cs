using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    [Header("Configuración del Diccionario de Misiones")]
    [SerializeField] public List<Image> statList;
    [SerializeField]List<string> interactionNames = new List<string>();
    [SerializeField] TextMeshProUGUI contactoVisualText;

    //QUITAR CUANDO MODIFIQUE TODO
    public List<Image> chatGPTStatList;

    public Dictionary<string, Image> missionStatsDictionary; // Diccionario que vincula el nombre de las misiones con sus imágenes en la UI.

    [Header("Configuración de Imágenes")]
    public Sprite spriteCheck; // Imagen que se usará para marcar una misión como completada.

    

    public void InitializeMissionDictionary()
    {
        missionStatsDictionary = new Dictionary<string, Image>();

        // Verificar que la lista de imágenes coincida con la cantidad de misiones.
        if (statList.Count == interactionNames.Count)
        {
            for (int i = 0; i < statList.Count; i++)
            {
                missionStatsDictionary[interactionNames[i]] = statList[i];
            }
        }
        else
        {
            Debug.LogError("La cantidad de imágenes en la lista no coincide con la cantidad de misiones.");
        }
    }

    // Método para actualizar la imagen de una misión específica.
    public void UpdateStatImage(string missionName)
    {
        if (missionStatsDictionary.ContainsKey(missionName))
        {
            missionStatsDictionary[missionName].sprite = spriteCheck;
            missionStatsDictionary[missionName].color = Color.white; // Cambia el color de la imagen a blanco para indicar que está activada.
        }
        else
        {
            Debug.LogWarning($"No se encontró la misión con nombre {missionName} en el diccionario.");
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
            contactoVisualText.color = new Color(1f, 1f, 0.6f); // Amarillo si está entre 35 y 55
        }
        else if (p > 55)
        {
            contactoVisualText.color = new Color(0.6f, 1f, 0.6f); // Verde si es mayor de 55
        }

    }
}
