using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class SonsPassos : MonoBehaviour
{
    [Header("Configurações de Áudio")]
    [SerializeField] private AudioClip[] clipesPassos;
    [Range(0f, 1f)]
    [SerializeField] private float volume = 0.6f;

    [Header("Variação Natural")]
    [SerializeField] private float pitchMinimo = 0.9f;
    [SerializeField] private float pitchMaximo = 1.1f;

    private AudioSource audioSource;
    private Animator animator;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    // Método chamado diretamente pelos Animation Events nos clipes de animação
    public void TocarPasso()
    {
        // Evita disparar som fantasma durante a seta de transição (ex: parando para Idle)
        if (animator.IsInTransition(0)) return;

        if (clipesPassos == null || clipesPassos.Length == 0) return;

        int indiceAleatorio = Random.Range(0, clipesPassos.Length);
        audioSource.pitch = Random.Range(pitchMinimo, pitchMaximo);
        
        audioSource.PlayOneShot(clipesPassos[indiceAleatorio], volume);
    }
}