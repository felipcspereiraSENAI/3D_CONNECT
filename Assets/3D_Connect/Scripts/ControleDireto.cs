using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInput))]
public class ControleDireto : MonoBehaviour
{
    private Vector2 inputMovimento; // Armazena o valor do input de movimento
    private Animator animator; // Referência ao componente Animator
    private bool estaCorrendo = false; // Variável para controlar o estado de corrida

    private void Start()
    {
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        if (animator == null) return;

        // Verifica se há movimento real nos eixos
        bool emMovimento = inputMovimento.sqrMagnitude > 0.01f;

        // Atualização dos parâmetros do Animator
        // Andando: Há movimento E NÃO está correndo
        animator.SetBool("isWalking", emMovimento);
        LeituraDiretaTeclas();
    }

    private void LeituraDiretaTeclas()
    {
        var teclado = Keyboard.current;
        var mouse = Mouse.current;

        if (teclado == null) return;

        // 1. Leitura contínua dos eixos WASD / Setas
        float x = 0f;
        float y = 0f;

        if (teclado.wKey.isPressed) y += 1f;
        if (teclado.sKey.isPressed) y -= 1f;
        if (teclado.dKey.isPressed) x += 1f;
        if (teclado.aKey.isPressed) x -= 1f;

        inputMovimento = new Vector2(x, y);

        // 3. Leitura de tecla pontual (Clique Esquerdo para Trigger)
        if (teclado.jKey.wasPressedThisFrame || mouse.leftButton.wasPressedThisFrame)
        {
            animator.SetTrigger("LowKick");
            Debug.Log("Ataque Baixo Direto");
        }

        if (teclado.kKey.wasPressedThisFrame || mouse.rightButton.wasPressedThisFrame)
        {
            animator.SetTrigger("FireBall");
            Debug.Log("Ataque de fogo Direto");
        }


    }


}