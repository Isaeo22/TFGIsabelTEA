using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimationManager : MonoBehaviour
{

    private Animator animator;
    private float timer;
    [SerializeField]float timeBetweenAnimations;
    // Start is called before the first frame update

    void Start()
    {
        animator = GetComponent<Animator>();
        ChangeAnimation();

    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeBetweenAnimations)
        {
            ChangeAnimation();
            timer = 0f;
        }
    }

    void ChangeAnimation()
    {
        int randomIndex = Random.Range(0, 3); // Genera un número aleatorio entre 0 y 2
        animator.SetInteger("RandomInt", randomIndex);
    }
}
