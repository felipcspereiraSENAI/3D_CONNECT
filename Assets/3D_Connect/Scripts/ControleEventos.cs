using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInput))]
public class ControleEventos : MonoBehaviour
{
    private Vector2 inputMovimento; // Armazena o valor do input de movimento
    private Animator animator; // Referência ao componente Animator
    private bool estaCorrendo = false; // Variável para controlar o estado de corrida

    private void Start()
    {
        animator = GetComponent<Animator>();
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
        }
    }

    public void OnFireBall(InputValue value)
    {
        if (value.isPressed && animator != null)
        {
            animator.SetTrigger("FireBall");
            Debug.Log("Ataque de Fogo");
        }
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && animator != null)
        {
            animator.SetTrigger("Jump");
            Debug.Log("Pulo");
        }
    }


    private void Update()
    {
        if (animator == null) return;

        // Verifica se há movimento real nos eixos
        bool emMovimento = inputMovimento.sqrMagnitude > 0.01f;

        // Atualização dos parâmetros do Animator
        // Andando: Há movimento E NÃO está correndo
        animator.SetBool("isWalking", emMovimento && !estaCorrendo);
    }
}