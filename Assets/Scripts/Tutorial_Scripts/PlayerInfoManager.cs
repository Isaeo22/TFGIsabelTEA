using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInfoManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI rolText;
    [SerializeField] private GameObject textBox;

  
  


    //VARIABLES SERIALIZE FIELD
    [SerializeField] TextMeshProUGUI instrucciones;
    [SerializeField] GameObject instruccionesGO;

    [SerializeField] TMP_Dropdown dropDownMics;
    [SerializeField] Button buttonContinuarMic;

    [SerializeField] TMP_InputField usernameInputField;  // Referencia al InputField
    [SerializeField]Button buttonContinuarUsername;  // Botón para enviar el nombre de usuario

    [SerializeField] WolfieManager wolfie;

    [SerializeField] TutorialManager tutorialController;

    private void Start()
    {
        buttonContinuarMic.onClick.AddListener(ButtonContinuarMic);
        buttonContinuarUsername.onClick.AddListener(ButtonContinuarUsername);
        wolfie.OnEndRun += StartExplicacion;
       // usernameInputField.onEndEdit.AddListener(OnSubmit);
    }
    public void StartPlayerInfo()
    {
        this.gameObject.SetActive(true);
        dropDownMics.gameObject.SetActive(true);

        dropDownMics.options.Clear();
        //Encuentra los micrófonos y los muestra en el dropdown
        foreach (var device in Microphone.devices)
        {
            dropDownMics.options.Add(new TMP_Dropdown.OptionData(device));
        }

        dropDownMics.options.Insert(0, new TMP_Dropdown.OptionData("Sin micrófono"));
        dropDownMics.value = 0;
        dropDownMics.onValueChanged.AddListener(ChangeMicrophone);
        var index = PlayerPrefs.GetInt("userMic");
        dropDownMics.SetValueWithoutNotify(index);

        EventSystem.current.SetSelectedGameObject(dropDownMics.gameObject);
    }
    private void ChangeMicrophone(int index)
    {
        if (index != 0)
        {
            string selectedMic = Microphone.devices[index - 1];
            PlayerPrefs.SetString("userMic", selectedMic);
            PlayerPrefs.Save();  // Guardamos los cambios
            buttonContinuarMic.gameObject.SetActive(true);
        }
    }

    void ButtonContinuarMic()
    {
        if (dropDownMics.value == 0)  // Si la opción seleccionada es "Ningún micrófono"
        {
            // Mostrar el mensaje de advertencia
            instrucciones.text = "¡Debes seleccionar un micrófono!";
            EventSystem.current.SetSelectedGameObject(dropDownMics.gameObject);
        }
        else
        {
            dropDownMics.gameObject.SetActive(false);
            buttonContinuarMic.gameObject.SetActive(false);
            SetUsername();
        }
        
    }

    void SetUsername()
    {
        instrucciones.text = "¿Cómo te llamas?";
        usernameInputField.gameObject.SetActive(true);

        EventSystem.current.SetSelectedGameObject(usernameInputField.gameObject);
    }

    public void OnSubmit(string input)
    {
        buttonContinuarUsername.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(buttonContinuarUsername.gameObject);
    }

    void ButtonContinuarUsername()
    {
        string username = usernameInputField.text;  // Obtener el texto del InputField

        if (!string.IsNullOrEmpty(username))  // Asegurarse de que el campo no esté vacío
        {
            PlayerPrefs.SetString("username", username);  // Guardar el nombre de usuario en PlayerPrefs
            PlayerPrefs.Save();  // Guardar los cambios
            Debug.Log("Nombre de usuario guardado: " + username);
            EmpezarTutorial();
        }
        else
        {
            instrucciones.text="Por favor, ingrese un nombre de usuario";
            EventSystem.current.SetSelectedGameObject(usernameInputField.gameObject);
        }
    }

    void EmpezarTutorial()
    {
        usernameInputField.gameObject.SetActive(false);
        buttonContinuarUsername.gameObject.SetActive(false);
        instruccionesGO.SetActive(false);
        wolfie.Saludar();
    }

    void StartExplicacion()
    {
       tutorialController.StartTutorial();
        InputManager.lookEnable = true;
    }
}
