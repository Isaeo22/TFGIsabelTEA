using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OpcionesManager : MonoBehaviour
{
    [SerializeField] GameObject panelOpciones;
    [Header("Volumen")]
    [SerializeField] Slider volumenSlider;
    [SerializeField] Button subirVolumen;
    [SerializeField] Button bajarVolumen;
    float volumen;

    [Header("Cambio de nombre")]
    [SerializeField] Button cambiarNombre;
    [SerializeField] GameObject panelCambiarNombre;
    [SerializeField] Button buttonVolverCambiarNombre;

    [SerializeField] Button setNombre;
    [SerializeField] TMP_InputField inputNombre;
    [SerializeField] TextMeshProUGUI infoCambioNombre;


    [Header("Cambio de micro")]
    [SerializeField] Button cambiarMicro;
    [SerializeField] GameObject panelCambiarMicro;
    [SerializeField] Button buttonVolverCambiarMicro;

    [SerializeField] Button setMicro;
    [SerializeField] TMP_Dropdown microfonoDropdown;
    [SerializeField] TextMeshProUGUI infoCambioMicro;
    int indexMicro;

    [SerializeField] Button buttonVolver;

    private string nombreJugador;
    // Start is called before the first frame update
    void Start()
    {
        //EventSystem.current.SetSelectedGameObject(buttonVolver.gameObject);
       

        //CONFIGURAR VOLUMEN
        // Cargar valores guardados
        volumen = PlayerPrefs.GetFloat("volumen", 1f);
        volumenSlider.value= volumen;
        //Configurar botones
        subirVolumen.onClick.AddListener(SubirVolumen);
        bajarVolumen.onClick.AddListener(BajarVolumen);

        //CONFIGURAR CAMBIO DE NOMBRE
        //Configurar botones
        cambiarNombre.onClick.AddListener(AbrirCambioDeNombre);
        setNombre.onClick.AddListener(SetNombre);
        buttonVolverCambiarNombre.onClick.AddListener(VolverOpcionesPaneles);

        //CONFIGURAR CAMBIO DE MICRO
        //Configurar botones
        cambiarMicro.onClick.AddListener(AbrirCambioDeMicro);
        setMicro.onClick.AddListener(SetMicro);
        buttonVolverCambiarMicro.onClick.AddListener(VolverOpcionesPaneles);

        microfonoDropdown.options.Clear();
        //Encuentra los micrófonos y los muestra en el dropdown
        foreach (var device in Microphone.devices)
        {
            microfonoDropdown.options.Add(new TMP_Dropdown.OptionData(device));
        }

        microfonoDropdown.options.Insert(0, new TMP_Dropdown.OptionData("Sin micrófono"));
        microfonoDropdown.value = 0;
        microfonoDropdown.onValueChanged.AddListener(SetMicroIndex);
        var index = PlayerPrefs.GetInt("userMic");
        microfonoDropdown.SetValueWithoutNotify(index);


    }
    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(buttonVolver.gameObject);
    }

    void VolverOpcionesPaneles()
    {
        panelCambiarMicro.SetActive(false);
        panelCambiarNombre.SetActive(false);
        panelOpciones.SetActive(true);
    }

    //VOLUMEN
    void SubirVolumen()
    {
        volumen = Mathf.Clamp(volumen + 0.1f, 0f, 1f);
        GuardarVolumen();
    }
    void BajarVolumen()
    {
        volumen = Mathf.Clamp(volumen - 0.1f, 0f, 1f);
        GuardarVolumen();
    }
    void GuardarVolumen()
    {
        AudioListener.volume = volumen; // Cambia el volumen del juego
        PlayerPrefs.SetFloat("volumen", volumen); // Guarda el nuevo valor
        volumenSlider.value = volumen;
    }
    //VOLUMEN

    //NOMBRE
    void AbrirCambioDeNombre()
    {
        panelCambiarNombre.SetActive(true);
        panelCambiarMicro.SetActive(false);
        panelOpciones.SetActive(false);
        EventSystem.current.SetSelectedGameObject(buttonVolverCambiarNombre.gameObject);
    }

    void SetNombre()
    {
        string username = inputNombre.text;  // Obtener el texto del InputField

        if (!string.IsNullOrEmpty(username))  // Asegurarse de que el campo no esté vacío
        {
            PlayerPrefs.SetString("username", username);  // Guardar el nombre de usuario en PlayerPrefs
            PlayerPrefs.Save();  // Guardar los cambios
            infoCambioNombre.text = "Nuevo nombre de usuario " + username + " guardado";
            
        }
        else
        {
            infoCambioNombre.text = "Nombre de usuario inválido";
           

        }
    }
    //NOMBRE


    //MICROFONO
    void AbrirCambioDeMicro()
    {
        panelCambiarMicro.SetActive(true);
        panelOpciones.SetActive(false);
        EventSystem.current.SetSelectedGameObject(buttonVolverCambiarMicro.gameObject);
    }
    void SetMicroIndex(int index)
    {
        indexMicro = index - 1;
       
    }
    void SetMicro()
    {
        if (microfonoDropdown.value == 0)  // Si la opción seleccionada es "Ningún micrófono"
        {
            // Mostrar el mensaje de advertencia
            infoCambioMicro.text = "Selecciona un micrófono válido";
            EventSystem.current.SetSelectedGameObject(microfonoDropdown.gameObject);
        }
        else
        {
            string selectedMic = Microphone.devices[indexMicro];
            PlayerPrefs.SetString("userMic", selectedMic);
            PlayerPrefs.Save();  // Guardamos los cambios
            infoCambioMicro.text = "Micrófono guardo";

        }
    }
}
