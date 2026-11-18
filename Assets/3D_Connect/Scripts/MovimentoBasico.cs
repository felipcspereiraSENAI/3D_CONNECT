using UnityEngine; 
using UnityEngine.InputSystem; 

public class MovimentoBasico : MonoBehaviour
{
    private CharacterController player; // Referência ao componente CharacterController
    private Animator animator; // Referência ao componente Animator
    private Vector2 inputMovimento;
    private Vector3 movimento;

    private float velocidadeMovimento = 5f;
    private float velocidadeVertical;
    
    public void Start()
    {
        player = GetComponent<CharacterController>(); // Inicializa a referência ao CharacterController
        animator = GetComponent<Animator>();// Inicializa a referência ao Animator
    }

    public void OnWalk(InputValue value)
    {
        inputMovimento = value.Get<Vector2>(); 
    }

    public void Update()
    {
             
       
        velocidadeVertical = -20f; 
        
        player.Move(Vector3.up * velocidadeVertical * Time.deltaTime);

        movimento.Set(inputMovimento.x, 0, inputMovimento.y);
        
        
        
        
        if (movimento != Vector3.zero)
        { 
        animator.SetBool("isWalking", true);
        
        player.Move(movimento * Time.deltaTime * velocidadeMovimento);
               
        
        transform.forward = Vector3.Slerp(transform.forward, movimento, Time.deltaTime * 5f);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
        
        

    }
}
