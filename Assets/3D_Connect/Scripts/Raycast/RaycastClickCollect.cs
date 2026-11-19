using UnityEngine;
using UnityEngine.InputSystem;
public class RaycastClickCollect : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float distanciaMaxima = 100f;
    private int quantidadeItens;
    private int quantidadeInimigos;
    GameObject[] Inimigos;
    
    void Start()
    {
        AtualizarContagem();
    }

    // Update is called once per frame
    void Update()
    {
        Ray raio = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        // if (Physics.Raycast(raio, out hit, distanciaMaxima))
        // {
        //     Debug.DrawLine(raio.origin, hit.point, Color.cyan);
        //     Debug.Log("Objeto identificado: " + hit.collider.name);
        //     Debug.Log("Tipo de objeto: " + hit.collider.tag);
        // }
        if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    if (Physics.Raycast(raio, out hit, distanciaMaxima))
                    {
                        Debug.Log("Objeto clicado: " + hit.collider.name);
                        Debug.Log("Tipo de objeto clicado: " + hit.collider.tag);
                        if (hit.collider.CompareTag("Inimigo"))
                        {
                            
                            
                            Destroy(hit.collider.gameObject);
                            
                        }
                        if (hit.collider.CompareTag("Item"))
                        {
                            
                            quantidadeItens++;
                            
                            hit.collider.gameObject.transform.position = new Vector3(Random.Range(-10f, 10f),hit.collider.gameObject.transform.position.y,Random.Range(-10f, 10f));
                            AtualizarContagem();
                        }
                    
                    }

                }
                


    }

    // public void LateUpdate()
    // {
    //    AtualizarContagem(); 
    // }
    public void AtualizarContagem()
    {
        Inimigos = GameObject.FindGameObjectsWithTag("Inimigo");
        quantidadeInimigos = Inimigos.Length;
        Debug.Log("Quantidade de inimigos na cena: " + quantidadeInimigos);
        Debug.Log(quantidadeItens + " itens coletados.");
    }


}
