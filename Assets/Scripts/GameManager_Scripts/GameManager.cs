using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Administra la mision actual y la posicion del jugador
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("Jugador")]
    [SerializeField] GameObject player;
    [SerializeField] private InputManager playerUIController;
    [SerializeField] List<Transform> spawnPoints;
    Vector3 initialSpawnPos;
    Quaternion initialSpawnRot;

    [Header("Wolfie")]
    [SerializeField] List<AudioClip> audios;
    [SerializeField] AudioSource audioSource;

    [Header("Misiones")]
    [SerializeField] GameObject missionCanvas;
    [SerializeField] List<GameObject> missions;
    [SerializeField] List<MissionFather> missionScriptsList;
    int currentMission;
    public bool onMission = false;

   
    [SerializeField] ButtonsManager buttonsManager;   
    [SerializeField] JsonManager jsonManager;

   
    public static GameManager Instance { get; private set; }
 
    void Start()
    {
        Instance = this;
        initialSpawnPos = player.transform.position;
        initialSpawnRot = player.transform.rotation;
        
        Cursor.lockState = CursorLockMode.Locked;

        missionCanvas.SetActive(true);
        audioSource.clip = audios[0];
        audioSource.Play();
        buttonsManager.GetChildSelectables();

      
    }



    public void StartMission(int indexGame)
    {
        
        currentMission = indexGame;
        
        //Coloca al jugador en la posicion correspondiente a la mision
        SetPlayerTransform(spawnPoints[currentMission].position,spawnPoints[currentMission].rotation);
        
        missionCanvas.SetActive(false);

        missions[currentMission].SetActive(true);

        jsonManager.SetMission(missionScriptsList[currentMission]);

        missionScriptsList[currentMission].StartMission();   

        buttonsManager.GetChildSelectables();

        onMission = true;
        InputManager.lookEnable = true;
        InputManager.onMission = onMission;
    }

  

    public void SalirMission()
    {
        onMission = false;
        InputManager.onMission = onMission;

        missions[currentMission].SetActive(false);
        

        missionCanvas.SetActive(true);
        

        SetPlayerTransform(initialSpawnPos, initialSpawnRot);

    }

    //Transform Jugador
    public void SetPlayerTransform(Vector3 pos, Quaternion rot)
    {
        player.transform.position = pos;
        player.transform.rotation = rot;
    }



    public Transform GetPlayerTransform()
    {
        return player.transform;
    }
}
