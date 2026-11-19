using UnityEngine;
using UnityEngine.InputSystem;

public class RaycastDrag : MonoBehaviour
{
    [Header("Configurações de Arrasto")]
    [SerializeField] private float distanciaMaxima = 100f;
    [SerializeField] private float suavizacaoMovimento = 20f;

    [Header("Configurações de Profundidade (Scroll)")]
    [SerializeField] private float velocidadeScroll = 0.02f;
    [SerializeField] private float distanciaMinimaCamera = 1.5f; // Evita bater na lente da câmera
    [SerializeField] private float distanciaMaximaCamera = 30f;  // Limite máximo para afastar

    private Camera mainCamera;
    private Rigidbody rbSelecionado;
    private float distanciaZDaCamera;
    private Vector3 offset;
    private bool isDragging = false;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
       
        
        
        if (Mouse.current == null) return;

        // 1. PEGAR O OBJETO (Clique com botão direito)
        if (Mouse.current.rightButton.wasPressedThisFrame) 
        {
            Ray raio = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()); // Cria um raio a partir da posição do mouse na tela

            if (Physics.Raycast(raio, out RaycastHit hit, distanciaMaxima)) 
            {
                if (hit.collider.TryGetComponent(out Rigidbody rb)) // Verifica se o objeto clicado possui um Rigidbody
                {
                    rbSelecionado = rb; // Armazena o Rigidbody do objeto selecionado
                    isDragging = true; // Indica que o arrasto começou

                    rbSelecionado.useGravity = false;  // Desativa a gravidade para que o objeto não caia enquanto é arrastado
                    rbSelecionado.linearVelocity = Vector3.zero; // Zera a velocidade linear para evitar movimentos indesejados
                    rbSelecionado.angularVelocity = Vector3.zero; // Zera a velocidade angular para evitar rotações indesejadas

                    distanciaZDaCamera = mainCamera.WorldToScreenPoint(rbSelecionado.position).z; // Calcula a distância do objeto selecionado em relação à câmera

                    Vector3 posMouseMundo = ObterPosicaoMouseNoMundo(); // Obtém a posição do mouse no mundo com base na profundidade calculada
                    offset = rbSelecionado.position - posMouseMundo; // Calcula o offset entre a posição do objeto e a posição do mouse no mundo para manter a posição relativa durante o arrasto
                }
            }

 Debug.DrawRay(raio.origin, raio.direction * distanciaMaxima, Color.red, 1f);

        }

        // 2. AJUSTAR PROFUNDIDADE COM O SCROLL (Enquanto segura o objeto)
        if (isDragging && rbSelecionado != null)
        {
            float scrollY = Mouse.current.scroll.y.ReadValue(); // Lê o valor do scroll vertical do mouse

            if (scrollY != 0f)
            {
                distanciaZDaCamera += scrollY * velocidadeScroll; // Ajusta a profundidade com base no scroll do mouse
                distanciaZDaCamera = Mathf.Clamp(distanciaZDaCamera, distanciaMinimaCamera, distanciaMaximaCamera); // Garante que a profundidade permaneça dentro dos limites definidos
            }
        }

        // 3. SOLTAR O OBJETO (Soltou o botão direito)
        if (Mouse.current.rightButton.wasReleasedThisFrame && isDragging)
        {
            if (rbSelecionado != null)
            {
                rbSelecionado.useGravity = true; // Reativa a gravidade para que o objeto caia novamente
            }

            isDragging = false; //
            rbSelecionado = null;
        }
    }

    void FixedUpdate()
    {
        if (isDragging && Mouse.current.rightButton.isPressed && rbSelecionado != null)
        {
            Vector3 destino = ObterPosicaoMouseNoMundo() + offset; // Calcula a posição de destino do objeto com base na posição do mouse no mundo e no offset
            Vector3 direcao = destino - rbSelecionado.position; // Calcula a direção do movimento do objeto em relação à sua posição atual
            rbSelecionado.linearVelocity = direcao * suavizacaoMovimento; // Aplica a velocidade linear ao Rigidbody do objeto selecionado para movê-lo suavemente em direção à posição de destino
        }
    }

    private Vector3 ObterPosicaoMouseNoMundo()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();      // Obtém a posição do mouse na tela
        Vector3 pontoComProfundidade = new Vector3(mouseScreenPos.x, mouseScreenPos.y, distanciaZDaCamera);  // Cria um ponto na tela com a profundidade calculada
        return mainCamera.ScreenToWorldPoint(pontoComProfundidade); // Converte a posição do mouse na tela para o mundo
    }
}