using UnityEngine;
using UnityEngine.InputSystem;

public class MovimentoBasicoTank : MonoBehaviour
{
    private CharacterController character;

    private Vector2 inputMovimento;
    private Vector3 movimento;

    private float velocidadeMovimento = 2f;
    private float velocidadeRotacao = 150f;

    void Start()
    {
        character = GetComponent<CharacterController>();
    }

    public void OnWalk(InputValue value)
    {
        inputMovimento = value.Get<Vector2>();
    }

    void Update()
    {

        float giro = inputMovimento.x * velocidadeRotacao * Time.deltaTime;
        Quaternion mudancaRotacao = Quaternion.Euler(0f, giro, 0f);
        transform.rotation *= mudancaRotacao; // Multiplica o Quaternion para somar a rotação

        // 3. Movimento para frente/trás na direção em que o personagem olha (Teclas W / S)
        Vector3 direcaoFrente = transform.forward * inputMovimento.y;
        Vector3 deslocamentoFinal = (direcaoFrente * velocidadeMovimento);

        character.Move(deslocamentoFinal * Time.deltaTime);


    }
}
