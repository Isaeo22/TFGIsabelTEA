using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeviceManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titulo;

    // Asignar los botones en el Inspector
    [SerializeField] private Button buttonOrdenador;
    [SerializeField] private Button buttonRealidadVirtual;

    [SerializeField] GameObject buttonsSiNo;
    [SerializeField] Button buttonSi;
    [SerializeField] Button buttonNo;

    string sceneName;

    private void Start()
    {
        // Aseg�rate de que los botones tengan la funci�n correspondiente asignada al presionarlos
        buttonOrdenador.onClick.AddListener(()=>PreguntaSiNo("GameScene","Ordenador"));
        buttonRealidadVirtual.onClick.AddListener(()=> PreguntaSiNo("GameSceneVR","Realidad virtual"));
        buttonNo.onClick.AddListener(Volver);
    }

    void PreguntaSiNo(string nombreEscena,string nombreDevice) {

        titulo.text = "�Quieres jugar en "+nombreDevice;
        buttonOrdenador.gameObject.SetActive(false);
        buttonRealidadVirtual.gameObject.SetActive(false);
        buttonSi.onClick.AddListener(()=>LoadScene(nombreEscena));
        buttonsSiNo.SetActive(true);
    }

    void Volver()
    {
        titulo.text = "�En qu� dispositivo vas a jugar?";
        buttonsSiNo.SetActive(false);
        buttonOrdenador.gameObject.SetActive(true);
        buttonRealidadVirtual.gameObject.SetActive(true);
    }


    public void StartDevice()
    {
        this.gameObject.SetActive(true);
    }

    // Funci�n para cargar la escena
    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
