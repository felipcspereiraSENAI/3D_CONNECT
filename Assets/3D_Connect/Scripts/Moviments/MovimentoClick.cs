using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class MovimentoClick : MonoBehaviour
{
    private CharacterController character;
    private Camera cameraPrincipal;
    private Animator animator;
    private Vector3 destinoAlvo;
    private bool temDestino = false;

    private float velocidadeVertical;

    [SerializeField] private float velocidadeMovimento = 4f;
    [SerializeField] private float distanciaParada = 0.15f;
    

    void Start()
    {
        animator = GetComponent<Animator>();
        character = GetComponent<CharacterController>();
        cameraPrincipal = Camera.main;
        destinoAlvo = transform.position;
    }

    // Crie uma Action "Click" (Button) vinculada ao Left Button [Mouse] no Input Actions
    public void OnClick(InputValue value)
    {
        if (value.isPressed && cameraPrincipal != null)
        {
            // Lê a posição atual do cursor na tela pelo New Input System
            Vector2 posicaoMouse = Mouse.current.position.ReadValue();
            Ray raio = cameraPrincipal.ScreenPointToRay(posicaoMouse);

            // Verifica onde o raio atingiu algum Collider (chão/cenário)
            if (Physics.Raycast(raio, out RaycastHit hit))
            {
                destinoAlvo = hit.point;
                temDestino = true;
                Debug.DrawLine(raio.origin, hit.point, Color.cyan, 1f);
            }
        }
    }

    void Update()
    {
        if (character.isGrounded)
        {
            if (velocidadeVertical < 0f)
                velocidadeVertical = -5f;
        }
        else
        {
            velocidadeVertical += -25f * Time.deltaTime;
        }

        Vector3 movimentoHorizontal = Vector3.zero;

        if (temDestino)
        {
            Vector3 direcao = destinoAlvo - transform.position;
            direcao.y = 0f;

            if (direcao.magnitude > distanciaParada)
            {
                Vector3 direcaoNormalizada = direcao.normalized;
                movimentoHorizontal = direcaoNormalizada * velocidadeMovimento;
                transform.forward = Vector3.Slerp(transform.forward, direcaoNormalizada, Time.deltaTime * 10f);
                animator.SetBool("isWalking", true);
            }
            else
            {
                PararMovimento();
            }
        }

        Vector3 deslocamentoFinal = movimentoHorizontal + (Vector3.up * velocidadeVertical);
        character.Move(deslocamentoFinal * Time.deltaTime);
    }

    // Detecta colisões do CharacterController durante o movimento
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Verifica se está tentando andar e se a colisão foi lateral (ignora o chão)
        if (temDestino && hit.normal.y < 0.5f)
        {
            PararMovimento();
        }
    }

    // Método auxiliar para centralizar a parada do personagem e da animação
    private void PararMovimento()
    {
        temDestino = false;
        destinoAlvo = transform.position;
        if (animator != null)
        {
            animator.SetBool("isWalking", false);
        }
    }
}