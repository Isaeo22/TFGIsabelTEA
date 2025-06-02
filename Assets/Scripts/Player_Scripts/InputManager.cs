using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput; //Referencia al script autogenerado por el input action asset

    public PlayerInput.PlayerActionMapActions playerActionMap;
   
   
    private PlayerLook look;

    [SerializeField] private GameObject pauseUI;

    GameObject pauseUIInstance;


    //VR
    [SerializeField] Transform UIPosition;

    public static bool isPaused;
    public static bool lookEnable;
    public static bool onMission;
   


    [SerializeField] bool OnVr;
    [SerializeField] bool OnTutorial;
    static public bool OnTutorialScene;
    [SerializeField] ButtonsManager buttonsManager;

    void Awake()
    {
        playerInput = new PlayerInput();
        
    
        look = GetComponent<PlayerLook>();
        playerActionMap = playerInput.PlayerActionMap;
        OnTutorialScene = OnTutorial;
    }



 
    void LateUpdate()
    {  
        if (OnVr) return;
        if (lookEnable)
        {
            look.ProcessLook(playerActionMap.Look.ReadValue<Vector2>());
        }                  
    }
    private void OnEnable()
    {
        playerActionMap.Look.Enable();
        playerActionMap.OpenPauseMenu.Enable();

        playerActionMap.OpenPauseMenu.performed += Pause;
    }

    private void OnDisable()
    {
        playerActionMap.Look.Disable();
        playerActionMap.OpenPauseMenu.Disable();

    }

    public void Pause(InputAction.CallbackContext context) {

        isPaused = !isPaused;

        if (isPaused)
        {
            ActivateMenu();
            
            buttonsManager.GetChildSelectables();
        }
        else
        {
            DeactivateMenu();
        }

    }

    void ActivateMenu()
    {     
        pauseUIInstance = Instantiate(pauseUI,UIPosition.position,UIPosition.rotation);  
        Time.timeScale = 0;
        AudioListener.pause = true;

    }

    void DeactivateMenu()
    {
        pauseUIInstance.SetActive(false);
        Time.timeScale = 1;
        AudioListener.pause = false;
        
        isPaused = false;
        Destroy(pauseUIInstance);
    }
}
