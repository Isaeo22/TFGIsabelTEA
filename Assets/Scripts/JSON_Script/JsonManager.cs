using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Administra la creacion del Json con las Stats del jugador
/// </summary>
public class JsonManager : MonoBehaviour
{

    private ManagerOpenAI managerOpenAI;
    private OpenAIFather missionFather;
    public void SaveToJson()
    {
        UserInfo userInfo = new UserInfo();

        userInfo.NombreUsuario = PlayerPrefs.GetString("username");
        userInfo.NombreMision = missionFather.missionName;
        userInfo.PromedioPalabrasPorMensaje = missionFather.GetAverageNumberWordsPerMessage();
        userInfo.NumeroDeMensajesTotales=missionFather.GetNumMessages();
        userInfo.TiempoMedioDeRespuesta= managerOpenAI.GetAverageResponseTime();
        userInfo.NumVecesQueRepiteMensaje = managerOpenAI.GetMessageRepetition();
        userInfo.NumVecesClickInfo = missionFather.GetNumClickInfo();
        userInfo.TiempoTotalContactoVisual = missionFather.GetTotalEyetrackingTime(); 
        userInfo.TiempoTotal = missionFather.GetTotalTime();

        string json = JsonUtility.ToJson(userInfo, true);

        string fileName = userInfo.NombreUsuario + "_" + DateTime.Now.ToString("dd-MM-yyyy") + ".json";

        // RUTA para guardar en la carpeta de la build (junto al .exe o .app)
        string folderPath = Path.Combine(Application.dataPath, "..", "UserStats");
        string filePath = Path.Combine(folderPath, fileName);

        // Crear la carpeta si no existe
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        // Save the JSON file to the specified path
        File.WriteAllText(filePath, json);
    }

    public void SetMission(OpenAIFather m)
    {
        missionFather = m;
    }

    public void SetManagerOpenAI(ManagerOpenAI m) {
        managerOpenAI = m;
    }

}
