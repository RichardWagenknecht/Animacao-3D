using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float velocidade = 5f;
    public float velocidadeRotacao = 10f;
    private int pontos;
    private CharacterController controlador;
    private Vector3 direcaoMovimento;
    private Animator animator;
    public GameObject Diamante;
    public Transform[] pontosSpawn = new Transform[5];
    public TextMeshPro text;
    

    void Start()
    {
        controlador = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        Spawnar();
    }

    void Update()
    {
        float movimentoX = Input.GetAxis("Horizontal");
        float movimentoZ = Input.GetAxis("Vertical");

        Vector3 movimento = new Vector3(movimentoX, 0, movimentoZ);

        if (movimento.magnitude > 0)
        {
            animator.SetBool("Walk", true);

            Vector3 direcao = new Vector3(movimentoX, 0, movimentoZ).normalized;

            Quaternion novaRotacao = Quaternion.LookRotation(direcao);

            transform.rotation = Quaternion.Slerp(transform.rotation, novaRotacao, velocidadeRotacao * Time.deltaTime);
        }
        else
        {
            animator.SetBool("Walk", false);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetTrigger("Fight");
        }

        controlador.Move(movimento * velocidade * Time.deltaTime);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Diamante")) 
        {
            Destroy(other.gameObject);
            Spawnar();
            text.text = pontos++.ToString();
        }
    }

    public void Spawnar()
    {
        Instantiate(Diamante, pontosSpawn[Random.Range(0, pontosSpawn.Length)].position, Quaternion.identity);
    }
}
