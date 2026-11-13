using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInput))]
public class ControleEventos : MonoBehaviour
{
    private Vector2 inputMovimento;
    private Animator animator;
    private bool estaCorrendo = false;

    [Header("Integração LED / MQTT")]
    [SerializeField] private ControleLed controleLed;
    [Tooltip("Tempo em segundos que o LED fica aceso a cada golpe")]
    [SerializeField] private float duracaoPulsoLed = 0.5f;

    private Coroutine rotinaPulso;

    private void Start()
    {
        animator = GetComponent<Animator>();

        // Localiza o script ControleLed na cena automaticamente caso não seja arrastado no Inspector
        if (controleLed == null)
        {
            controleLed = FindObjectOfType<ControleLed>();
        }
    }

    public void OnWalk(InputValue value)
    {
        inputMovimento = value.Get<Vector2>();
        Debug.Log("Esta andando:" + inputMovimento);
    }

    public void OnLowKick(InputValue value)
    {
        if (value.isPressed && animator != null)
        {
            animator.SetTrigger("LowKick");
            Debug.Log("Ataque Baixo");
            AcionarLedAtaque();
        }
    }

    public void OnFireBall(InputValue value)
    {
        if (value.isPressed && animator != null)
        {
            animator.SetTrigger("FireBall");
            Debug.Log("Ataque de Fogo");
            AcionarLedAtaque();
        }
    }

    private void AcionarLedAtaque()
    {
        if (controleLed == null)
        {
            Debug.LogWarning("[ControleEventos] Nenhum objeto com o script ControleLed foi encontrado na cena.");
            return;
        }

        // Reinicia o temporizador caso o jogador ataque novamente antes do LED apagar
        if (rotinaPulso != null)
        {
            StopCoroutine(rotinaPulso);
        }

        rotinaPulso = StartCoroutine(PulsarLed());
    }

    private IEnumerator PulsarLed()
    {
        controleLed.BotaoLigarLED();
        yield return new WaitForSeconds(duracaoPulsoLed);
        controleLed.BotaoDesligarLED();
        rotinaPulso = null;
    }

    private void Update()
    {
        if (animator == null) return;

        bool emMovimento = inputMovimento.sqrMagnitude > 0.01f;
        animator.SetBool("isWalking", emMovimento && !estaCorrendo);
    }
}