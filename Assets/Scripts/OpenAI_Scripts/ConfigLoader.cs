using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ConfigLoader : MonoBehaviour
{
    public static ConfigLoader Instance { get; private set; }

    public string ApiKey { get; private set; }
    public string OrganizationId { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantener este objeto al cambiar de escena
            LoadConfig();
        }
        else
        {
            Destroy(gameObject); // Evitar duplicados
        }
    }

    private void LoadConfig()
    {
        string path = "D:/Uni/TFG Cueva/StreamingAssets/config.json";//Aquí poner la ruta del json con key y org

        if (File.Exists(path))
        {
            string jsonContent = File.ReadAllText(path);
            ConfigData config = JsonUtility.FromJson<ConfigData>(jsonContent);
            ApiKey = config.openai_api_key;
            OrganizationId = config.organization_id;
            Debug.Log("API Key y Organization ID cargados correctamente.");
        }
        else
        {
            Debug.LogError("No se encontró el archivo config.json");
        }
    }
}

[System.Serializable]
public class ConfigData
{
    public string openai_api_key;
    public string organization_id;
}

