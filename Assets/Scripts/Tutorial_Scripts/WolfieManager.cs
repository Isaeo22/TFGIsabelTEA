using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfieManager : MonoBehaviour
{
    Animator animator;
    AudioSource audioSource;
    [SerializeField]AudioClip saludo;
    [SerializeField]Transform end;
    bool running = false;
    float speed;
    public Action OnEndRun;

    
    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        speed = 5f;
    }
    public void Saludar()
    {
        animator.SetBool("Saludar",true);
        audioSource.clip = saludo;
        audioSource.Play();
        Invoke("StartRun",2.0f);
    }

    void StartRun()
    {
        running = true;
        animator.SetBool("Correr", true);
    }

    private void Update()
    {
        if (running)
        {

            float step = speed * Time.deltaTime;
            this.transform.position = Vector3.MoveTowards(transform.position, end.position, step);
            if (transform.position == end.position)
            {
                animator.SetBool("Correr", false);
                running = false;
                OnEndRun?.Invoke();
            }

        }
    }
}
