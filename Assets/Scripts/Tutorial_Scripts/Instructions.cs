using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Instructions : MonoBehaviour
{
    [SerializeField] List<GameObject> controles;
    [SerializeField] Button previous;
    [SerializeField] Button next;

    GameObject actual;

    int index;

    public event Action OnEndInstructions;
    // Start is called before the first frame update
    public void StartInstructions()
    {
        this.gameObject.SetActive(true);
        previous.onClick.AddListener(Previous);
        next.onClick.AddListener(Next);

        EventSystem.current.SetSelectedGameObject(next.gameObject);
        index = 0;
        actual = controles[index];
    }

    // Update is called once per frame
   void Next()
   {

        actual.SetActive(false);
        index++;
        if (index < controles.Count) {
            actual = controles[index];
            actual.SetActive(true);

        }
        else
        {
            OnEndInstructions?.Invoke();
            this.gameObject.SetActive(false);         
        }
        
   }

    void Previous()
    {
        if (index != 0)
        {
            actual.SetActive(false);
            index--;


            actual = controles[index];
            actual.SetActive(true);
        }
        
    }
}
