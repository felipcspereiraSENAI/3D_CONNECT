using UnityEngine;
using UnityEngine.InputSystem;

public class ArrastarLivre3D : MonoBehaviour
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
            Ray raio = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(raio, out RaycastHit hit, distanciaMaxima))
            {
                if (hit.collider.TryGetComponent(out Rigidbody rb))
                {
                    rbSelecionado = rb;
                    isDragging = true;

                    rbSelecionado.useGravity = false;
                    rbSelecionado.linearVelocity = Vector3.zero;
                    rbSelecionado.angularVelocity = Vector3.zero;

                    distanciaZDaCamera = mainCamera.WorldToScreenPoint(rbSelecionado.position).z;

                    Vector3 posMouseMundo = ObterPosicaoMouseNoMundo();
                    offset = rbSelecionado.position - posMouseMundo;
                }
            }
        }

        // 2. AJUSTAR PROFUNDIDADE COM O SCROLL (Enquanto segura o objeto)
        if (isDragging && rbSelecionado != null)
        {
            float scrollY = Mouse.current.scroll.y.ReadValue();

            if (scrollY != 0f)
            {
                distanciaZDaCamera += scrollY * velocidadeScroll;
                distanciaZDaCamera = Mathf.Clamp(distanciaZDaCamera, distanciaMinimaCamera, distanciaMaximaCamera);
            }
        }

        // 3. SOLTAR O OBJETO (Soltou o botão direito)
        if (Mouse.current.rightButton.wasReleasedThisFrame && isDragging)
        {
            if (rbSelecionado != null)
            {
                rbSelecionado.useGravity = true;
            }

            isDragging = false;
            rbSelecionado = null;
        }
    }

    void FixedUpdate()
    {
        if (isDragging && Mouse.current.rightButton.isPressed && rbSelecionado != null)
        {
            Vector3 destino = ObterPosicaoMouseNoMundo() + offset;
            Vector3 direcao = destino - rbSelecionado.position;
            rbSelecionado.linearVelocity = direcao * suavizacaoMovimento;
        }
    }

    private Vector3 ObterPosicaoMouseNoMundo()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 pontoComProfundidade = new Vector3(mouseScreenPos.x, mouseScreenPos.y, distanciaZDaCamera);
        return mainCamera.ScreenToWorldPoint(pontoComProfundidade);
    }
}