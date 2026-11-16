using UnityEngine;
using UnityEngine.InputSystem;

public class MovimentoBasico : MonoBehaviour
{
    private CharacterController character;

    private Vector2 inputMovimento;
    private Vector3 movimento;

    private float velocidadeMovimento = 2f;
    
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

        movimento.Set(inputMovimento.x, 0, inputMovimento.y);
        if (movimento != Vector3.zero)
        { 
        
        character.Move(movimento * Time.deltaTime * velocidadeMovimento);
        transform.forward = Vector3.Slerp(transform.forward, movimento, Time.deltaTime * 10);
        }
           
        
    }
}
