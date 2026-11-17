using UnityEngine;
using UnityEngine.InputSystem;

public class MovimentoBasico : MonoBehaviour
{
    private CharacterController character;
private Animator animator;
    private Vector2 inputMovimento;
    private Vector3 movimento;

    private float velocidadeMovimento = 5;
    private float velocidadeVertical;
    
    public void Start()
    {
        character = GetComponent<CharacterController>(); 
        animator = GetComponent<Animator>();
    }

    public void OnWalk(InputValue value)
    {
        inputMovimento = value.Get<Vector2>(); 
    }

    public void Update()
    {
        velocidadeVertical = -20f; 


        movimento.Set(inputMovimento.x, 0, inputMovimento.y);
        if (movimento != Vector3.zero)
        { 
        animator.SetBool("isWalking", true);
        character.Move(movimento * Time.deltaTime * velocidadeMovimento);
        transform.forward = Vector3.Slerp(transform.forward, movimento, Time.deltaTime * 10);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
        
    }
}
