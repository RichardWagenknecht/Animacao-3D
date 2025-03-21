using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerLegacy : MonoBehaviour
{
    public float velocidade = 5f;
    public float velocidadeRotacao = 200f;
    private int pontos;
    private CharacterController controlador;
    private Animator animator;
    public GameObject Diamante;
    public Transform[] pontosSpawn = new Transform[5];
    public TextMeshPro text;
    public AudioClip coletarAudio;
    public AudioClip andarAudio;
    public AudioClip spawnAudio;
    public AudioSource playerAudio;
    public ParticleSystem particleSystem;
    public Transform cameraTransform;

    void Start()
    {
        controlador = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.Locked; // Trava o cursor no centro da tela
        Cursor.visible = false; // Oculta o cursor
        Spawnar();
    }

    void Update()
    {
        float movimentoX = Input.GetAxis("Horizontal"); // Movimento lateral
        float movimentoZ = Input.GetAxis("Vertical"); // Movimento frontal e traseiro
        float rotacaoY = Input.GetAxis("Mouse X") * velocidadeRotacao * Time.deltaTime;

        Vector3 direcaoMovimento = new Vector3(movimentoX, 0, movimentoZ).normalized;

        if (direcaoMovimento.magnitude > 0)
        {
            Quaternion novaRotacao = Quaternion.LookRotation(direcaoMovimento);
            transform.rotation = Quaternion.Slerp(transform.rotation, novaRotacao, velocidadeRotacao * Time.deltaTime);

            if (!playerAudio.isPlaying)
            {
                playerAudio.clip = andarAudio;
                playerAudio.loop = true;
                playerAudio.Play();
            }
            animator.SetBool("Walk", true);
        }
        else
        {
            playerAudio.Stop();
            animator.SetBool("Walk", false);
        }

        Vector3 movimento = transform.forward * movimentoZ + transform.right * movimentoX;
        controlador.Move(movimento * velocidade * Time.deltaTime);
        transform.Rotate(0, rotacaoY, 0);

        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetTrigger("Fight");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Diamante"))
        {
            Destroy(other.gameObject);
            playerAudio.PlayOneShot(coletarAudio);
            Spawnar();
            pontos++;
            text.text = pontos.ToString();
            if (pontos % 10 == 0)
            {
                MetaAtingida();
            }
        }
    }

    public void Spawnar()
    {
        playerAudio.PlayOneShot(spawnAudio, 0.5f);
        Instantiate(Diamante, pontosSpawn[Random.Range(0, pontosSpawn.Length)].position, Quaternion.identity);
    }

    public void MetaAtingida()
    {
        Debug.Log("Meta Atingida");
        particleSystem.Play();
    }
}