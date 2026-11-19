using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ControleAnimacaoBlendTree : MonoBehaviour
{
    [Header("Configurações da Blend Tree")]
    [Tooltip("Deve ser igual à velocidadeCorrer do script de movimentação")]
    [SerializeField] private float velocidadeMaxima = 7.5f;
    [SerializeField] private float suavizacaoBlend = 0.1f;

    private Animator animator;
    private Vector3 ultimaPosicao;
    private int hashVelocidade;

    private void Start()
    {
        animator = GetComponent<Animator>();
        ultimaPosicao = transform.position;
        hashVelocidade = Animator.StringToHash("Velocidade"); // Cache do hash para otimização
    }

    private void LateUpdate()
    {
        // 1. Calcula o deslocamento real no plano horizontal (X e Z) entre frames
        Vector3 deslocamento = transform.position - ultimaPosicao;
        deslocamento.y = 0f;

        // 2. Converte distância percorrida em velocidade (metros por segundo)
        float velocidadeReal = deslocamento.magnitude / Time.deltaTime;
        ultimaPosicao = transform.position;

        // 3. Normaliza de 0 a 1 e envia para o Animator
        float parametroNormalizado = Mathf.Clamp01(velocidadeReal / velocidadeMaxima);
        animator.SetFloat(hashVelocidade, parametroNormalizado, suavizacaoBlend, Time.deltaTime);
    }
}