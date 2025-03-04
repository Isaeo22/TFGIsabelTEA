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
    private MissionFather missionFather;
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
        string fileName = userInfo.NombreUsuario +"_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".json";

        // Save the JSON file to the specified path
        File.WriteAllText(Application.dataPath + "/" + fileName, json);

        Debug.Log("JSON saved as: " + fileName);


    }

    public void SetMission(MissionFather m)
    {
        missionFather = m;
    }

    public void SetManagerOpenAI(ManagerOpenAI m) {
        managerOpenAI = m;
    }

}
