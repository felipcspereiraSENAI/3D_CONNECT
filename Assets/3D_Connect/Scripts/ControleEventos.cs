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

    public void OnAttack(InputValue value)
    {
        if (value.isPressed && animator != null)
        {
            animator.SetTrigger("Kick");
            Debug.Log("Ataque com Mouse");
        }
    }

    // Callback da ação de correr
    public void OnSprint(InputValue value)
    {
        // value.isPressed é TRUE quando aperta, e FALSE quando solta
        estaCorrendo = value.isPressed;
        Debug.Log("Esta correndo: " + estaCorrendo);
    }

    private void Update()
    {
        if (animator == null) return;

        // Verifica se há movimento real nos eixos
        bool emMovimento = inputMovimento.sqrMagnitude > 0.01f;

        // Atualização dos parâmetros do Animator
        // Andando: Há movimento E NÃO está correndo
        animator.SetBool("isWalking", emMovimento && !estaCorrendo);

        // Correndo: Há movimento E ESTÁ correndo
        animator.SetBool("isRunning", emMovimento && estaCorrendo);



        //Keyboard keyboard = Keyboard.current;
        //if (keyboard.eKey.isPressed)
        //{
        //    animator.SetTrigger("Kick");
        //    Debug.Log("Ataque Botão E");
        //}




    }
}