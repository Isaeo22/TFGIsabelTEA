using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput; //Referencia al script autogenerado por el input action asset
    public PlayerInput.OnFootActions lookingActionMap;//Referencia al mapa de acciones Walking 
    public PlayerInput.PlayerUIActions playerUIActionMap;
   
   
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
        lookingActionMap = playerInput.OnFoot;
    
        look = GetComponent<PlayerLook>();
        playerUIActionMap = playerInput.PlayerUI;
        OnTutorialScene = OnTutorial;
    }



 
    void LateUpdate()
    {
     
        if (OnVr) return;
        if (lookEnable)
        {
            look.ProcessLook(lookingActionMap.Look.ReadValue<Vector2>());
        }                  
    }
    private void OnEnable()
    {
        lookingActionMap.Enable();
        playerUIActionMap.OpenPauseMenu.Enable();

        playerUIActionMap.OpenPauseMenu.performed += Pause;
    }

    private void OnDisable()
    {
        lookingActionMap.Disable();
        playerUIActionMap.OpenPauseMenu.Disable();

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
