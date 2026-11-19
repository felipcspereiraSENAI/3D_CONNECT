using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class MovimentacaoAvancada3D : MonoBehaviour
{
    [Header("Velocidades e Suavização")]
    [SerializeField] private float velocidadeAndar = 4f;
    [SerializeField] private float velocidadeCorrer = 7.5f;
    [SerializeField] private float aceleracao = 10f;
    [SerializeField] private float suavizacaoRotacao = 12f;

    [Header("Pulo e Gravidade")]
    [SerializeField] private float alturaPulo = 5f;
    [SerializeField] private float gravidade = -20f;

    private CharacterController controller;
    private Animator animator;
    private Transform cameraPrincipal;

    private Vector2 inputMovimento;
    private bool estaCorrendo;
    private float velocidadeAtualSuavizada;
    private float velocidadeVertical;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (Camera.main != null) cameraPrincipal = Camera.main.transform;
    }

    // Callbacks do New Input System (Send Messages)
    public void OnWalk(InputValue value) => inputMovimento = value.Get<Vector2>();
    public void OnSprint(InputValue value) => estaCorrendo = value.isPressed;

    public void OnJump(InputValue value)
    {
        // Só permite pular se estiver no chão E não estiver atacando
        if (value.isPressed && controller.isGrounded && !EstaAtacando())
        {
            // Fórmula física para atingir exatamente 'alturaPulo' em metros
            velocidadeVertical = Mathf.Sqrt(alturaPulo * -2f * gravidade);
        }
    }

    private void Update()
    {
        AplicarGravidade();
        MoverERotacionar();
    }

    private void AplicarGravidade()
    {
        if (controller.isGrounded && velocidadeVertical < 0f)
        {
            // Mantém o personagem "colado" no chão e em rampas descendentes
            velocidadeVertical = -2f;
        }

        velocidadeVertical += gravidade * Time.deltaTime;
    }

    private void MoverERotacionar()
    {
        bool bloqueadoPorAtaque = EstaAtacando();
        bool temInput = inputMovimento.sqrMagnitude > 0.01f;

        if (bloqueadoPorAtaque)
        {
            // Zera a inércia imediatamente para o personagem não deslizar durante o golpe
            velocidadeAtualSuavizada = 0f;
        }
        else
        {
            if (temInput && cameraPrincipal != null)
            {
                Vector3 frente = cameraPrincipal.forward;
                Vector3 direita = cameraPrincipal.right;
                frente.y = 0f;
                direita.y = 0f;

                Vector3 direcaoDesejada = (frente.normalized * inputMovimento.y + direita.normalized * inputMovimento.x).normalized;

                // Rotaciona na direção do movimento
                Quaternion rotacaoAlvo = Quaternion.LookRotation(direcaoDesejada);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotacaoAlvo, suavizacaoRotacao * Time.deltaTime);
            }

            // Interpola a velocidade para criar sensação de arranque e frenagem
            float velocidadeAlvo = temInput ? (estaCorrendo ? velocidadeCorrer : velocidadeAndar) : 0f;
            velocidadeAtualSuavizada = Mathf.Lerp(velocidadeAtualSuavizada, velocidadeAlvo, aceleracao * Time.deltaTime);
        }

        // Combina movimento horizontal e vertical em um único vetor
        Vector3 vetorFinal = (transform.forward * velocidadeAtualSuavizada) + (Vector3.up * velocidadeVertical);
        controller.Move(vetorFinal * Time.deltaTime);
    }

    private bool EstaAtacando()
    {
        if (animator == null) return false;

        AnimatorStateInfo estadoAtual = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo proximoEstado = animator.GetNextAnimatorStateInfo(0);

        return estadoAtual.IsTag("Attack") ||
               (animator.IsInTransition(0) && proximoEstado.IsTag("Attack"));
    }
}