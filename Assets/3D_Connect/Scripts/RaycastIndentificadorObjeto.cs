using UnityEngine;

public class RaycastIndentificadorObjeto : MonoBehaviour
{
   [SerializeField]   private GameObject origemRaycast;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(origemRaycast.transform.position , transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
           
            Debug.Log("Objeto identificado: " + hit.collider.name);
            Debug.Log("Tipo de objeto: " + hit.collider.tag);
        }
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red);
    }
}
