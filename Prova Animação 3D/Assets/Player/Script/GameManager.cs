using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    void Awake()
    {
        instance = this;
    }

    public TextMeshProUGUI restartTimerText;
    public GameObject restartPanel;
    public TextMeshPro textTimer;
    public Transform boneca;
    public AudioSource audioController;
    public AudioClip tiroA, alarmeA, timerA, vitoriaA;
    public ParticleSystem vitoriaParticula;
    public ParticleSystem[] tirosParticula = new ParticleSystem[16];
    private float tempoRestante;
    private float tempoLimite = 60f;
    private bool bonecaVirada = false;
    private float restartTime = 10f;
    private bool vivo = true;

    void Start()
    {
        tempoRestante = tempoLimite;
        StartCoroutine(ControlarBoneca());
        audioController.clip = timerA;
        audioController.Play();
    }
    void Update()
    {
        tempoRestante -= Time.deltaTime;
        textTimer.text = tempoRestante.ToString("F2");

        if (tempoRestante <= 15f )
        {
            audioController.PlayOneShot(alarmeA);
        }
        if (Player.instance.andando && !bonecaVirada && vivo == true || tempoRestante <= 0 && vivo == true)
        {
            GameOver();
        }
    }
    private IEnumerator ControlarBoneca()
    {
        while (tempoRestante > 0)
        {
            bonecaVirada = true;
            boneca.rotation = Quaternion.Euler(-90, 0, 0);
            yield return new WaitForSeconds(2f);

            bonecaVirada = false;
            boneca.rotation = Quaternion.Euler(-90, 180, 0);
            yield return new WaitForSeconds(3f);
        }
    }
    void GameOver()
    {
        vivo = false;
        audioController.PlayOneShot(tiroA);

        foreach (var particula in tirosParticula)
        {
            if (particula != null)
            {
                particula.Play();
            }
        }

        Player.instance.Morrer();
        restartPanel.SetActive(true);
        StartCoroutine(ContagemRegressivaRestart());
    }

    private IEnumerator ContagemRegressivaRestart()
    {
        float contador = restartTime;

        while (contador > 0)
        {
            restartTimerText.text = contador.ToString("F0");
            yield return new WaitForSeconds(1f);
            contador--;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Reinicia a cena
    }

    public void Vitoria()
    {
        Debug.Log("Ganhou");
        vitoriaParticula.Play();
        audioController.PlayOneShot(vitoriaA);
        restartPanel.SetActive(true);
        StartCoroutine(ContagemRegressivaRestart());
    }
}
