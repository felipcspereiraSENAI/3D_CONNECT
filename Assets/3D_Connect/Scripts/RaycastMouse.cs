using UnityEngine;
using UnityEngine.InputSystem;
public class RaycastMouse : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float distanciaMaxima = 100f;
    private bool isDragging = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray raio = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(raio, out hit, distanciaMaxima))
        {
            Debug.DrawLine(raio.origin, hit.point, Color.cyan);
            Debug.Log("Objeto identificado: " + hit.collider.name);
            Debug.Log("Tipo de objeto: " + hit.collider.tag);
        }
        if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    if (Physics.Raycast(raio, out hit, distanciaMaxima))
                    {
                        Debug.Log("Objeto clicado: " + hit.collider.name);
                        Debug.Log("Tipo de objeto clicado: " + hit.collider.tag);
                        if (hit.collider.CompareTag("Inimigo"))
                        {
                            Debug.Log("O inimigo foi destruido!");
                            Destroy(hit.collider.gameObject);
                        }
                        if (hit.collider.CompareTag("Item"))
                        {
                            
                            Debug.Log("O item foi coletado!");
                            hit.collider.gameObject.transform.position = new Vector3(Random.Range(-10f, 10f),hit.collider.gameObject.transform.position.y,Random.Range(-10f, 10f));
                        }
                    
                    }

                }
                


    }
}
