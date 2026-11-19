using UnityEngine;
using UnityEngine.InputSystem;

public class RaycastHover : MonoBehaviour
{
    [Header("Configurações de Destaque/Hover")]
    [SerializeField] private Material materialDestaque;

    // Propriedades do objeto com tag Item para destacar
    private Renderer itemDestacado;
    private Material materialOriginal;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current == null) return;

                // --- RAYCAST ---
        Ray raio = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        // Verifica se o raio colidiu com algum objeto
        if (Physics.Raycast(raio, out hit, 100f))
        {
            Debug.DrawLine(raio.origin, hit.point, Color.red);

            // --- DESTAQUE VISUAL (HOVER) ---
            // Só destaca se o mouse estiver sobre um "Item"
            if (hit.collider.CompareTag("Item"))
            {
                Renderer rendererAtual = hit.collider.GetComponent<Renderer>();

                // Se o renderer atual for diferente de itemDestacado, atualiza o destaque
                if (rendererAtual != null && rendererAtual != itemDestacado)
                {
                    LimparDestaque(); // Restaura o item anterior, se houver
                    itemDestacado = rendererAtual;
                    materialOriginal = itemDestacado.sharedMaterial; // Armazena o material original
                    itemDestacado.material = materialDestaque; // Aplica o material de destaque
                }
            }
            else
            {
                LimparDestaque();
            }
        }
        else LimparDestaque();
    }

    private void LimparDestaque()
    {
        if (itemDestacado != null)
        {
            // Restaura o material original do item destacado
            itemDestacado.material = materialOriginal;
            itemDestacado = null;
            materialOriginal = null;
        }
    }
}
