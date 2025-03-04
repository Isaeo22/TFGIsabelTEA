using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Button buttonVolver;
    [SerializeField] Button buttonOpciones;
    [SerializeField] Button buttonSalir;

    [SerializeField] Button buttonSi;
    [SerializeField] Button buttonNo;

    


    [SerializeField] GameObject panelPausa;
    [SerializeField] GameObject panelSalir;

    [SerializeField] TextMeshProUGUI textoPanelSalir;
    [SerializeField] GameObject panelOpciones;

    [SerializeField] GameObject panelOpcionesVolver;
    

    void Awake()
    {
      
        buttonVolver.onClick.AddListener(Volver);
        buttonOpciones.onClick.AddListener(Opciones);
        buttonSalir.onClick.AddListener(Salir);

        buttonSi.onClick.AddListener(ButtonSi);
        buttonNo.onClick.AddListener(ButtonNo);

    }
    private void Start()
    {

        EventSystem.current.SetSelectedGameObject(buttonVolver.gameObject);
    }
    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(buttonVolver.gameObject);
    }

    void Volver()
   {
        Destroy(this.gameObject);
        InputManager.isPaused = false;
        
        Time.timeScale = 1;
        AudioListener.pause = false;

        
       
    }

    void Opciones()
    {
        panelPausa.SetActive(false);
        panelOpciones.SetActive(true);
        EventSystem.current.SetSelectedGameObject(panelOpcionesVolver.gameObject);
    }

    void ButtonSi()
    {
        if (InputManager.onMission)
        {
            GameManager.Instance.SalirMission();

            Destroy(this.gameObject);

            InputManager.isPaused = false;
            Time.timeScale = 1;
            AudioListener.pause = false;

        }
        else if(!InputManager.OnTutorialScene)
        {
            InputManager.isPaused = false;

            SceneManager.LoadScene("TutorialScene");
        }
        else
        {
            Application.Quit();
        }
    
    }
    void ButtonNo()
    {
        panelSalir.SetActive(false);
      
        panelPausa.SetActive(true);
        EventSystem.current.SetSelectedGameObject(buttonVolver.gameObject);
    }

    void Salir()
    {
        panelPausa.SetActive(false);
        if (InputManager.onMission)
        {
            textoPanelSalir.text = "¿Quieres Salir de la misión?";
            panelSalir.SetActive(true);
           
            EventSystem.current.SetSelectedGameObject(buttonNo.gameObject);
        }
        else
        {
            textoPanelSalir.text = "¿Quieres Salir del juego?";
            panelSalir.SetActive(true);
            EventSystem.current.SetSelectedGameObject(buttonNo.gameObject);

        }
    }



}
