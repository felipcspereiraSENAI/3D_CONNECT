using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInput))]
public class MovimentoJogador : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [SerializeField] private float velocidadeMovimento = 5f;
    [SerializeField] private float velocidadeRotacao = 15f;

    [Header("Física")]
    [SerializeField] private float gravidade = -9.81f;

    private CharacterController controller;
    private Transform myCamera;
    private Animator animator;

    private Vector2 inputMovimento;
    private float velocidadeVertical;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (Camera.main != null)
        {
            myCamera = Camera.main.transform;
        }

        if (animator != null)
        {
            animator.applyRootMotion = false; // Desativa Root Motion para evitar conflito
        }
    }

    public void OnMove(InputValue value)
    {
        inputMovimento = value.Get<Vector2>();
    }
    public void OnLowKick(InputValue value)
    {
        if (animator != null)
        {
            animator.SetTrigger("LowKick");
            Debug.Log("Ataque Baixo");
        }
    }
    public void OnFireBall(InputValue value)
    {
        if (animator != null)
        {
            animator.SetTrigger("Fireball");
            Debug.Log("Ataque de Fogo");
        }
    }



    private void Update()
    {
        if (myCamera == null && Camera.main != null)
        {
            myCamera = Camera.main.transform;
        }

        // 1. ZONA MORTA: Só processa a rotação se houver entrada real do teclado/gamepad (> 0.1)
        if (inputMovimento.sqrMagnitude > 0.01f && myCamera != null)
        {
            // Pega a orientação da câmera
            Vector3 frenteCamera = myCamera.forward;
            Vector3 direitaCamera = myCamera.right;
            frenteCamera.y = 0f;
            direitaCamera.y = 0f;
            frenteCamera.Normalize();
            direitaCamera.Normalize();

            // Direção pretendida no chão
            Vector3 direcaoDesejada = (frenteCamera * inputMovimento.y + direitaCamera * inputMovimento.x).normalized;

            if (direcaoDesejada != Vector3.zero)
            {
                // Aplica rotação APENAS se o vetor for válido
                Quaternion rotacaoAlvo = Quaternion.LookRotation(direcaoDesejada, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotacaoAlvo, velocidadeRotacao * Time.deltaTime);

                // Move para a frente da direção calculada
                Vector3 movimentoHorizontal = direcaoDesejada * velocidadeMovimento;
                AplicarMovimento(movimentoHorizontal);
            }
        }
        else
        {
            // Parado: move apenas a gravidade no eixo Y (sem alterar rotação)
            AplicarMovimento(Vector3.zero);
        }

        // Animação
        if (animator != null)
        {
            animator.SetBool("isWalking", inputMovimento.sqrMagnitude > 0.1f);
        }
    }

    private void AplicarMovimento(Vector3 movimentoHorizontal)
    {
        if (controller.isGrounded && velocidadeVertical < 0f)
        {
            velocidadeVertical = -2f;
        }

        velocidadeVertical += gravidade * Time.deltaTime;
        movimentoHorizontal.y = velocidadeVertical;

        controller.Move(movimentoHorizontal * Time.deltaTime);
    }
}