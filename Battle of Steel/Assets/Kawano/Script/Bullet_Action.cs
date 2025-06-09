using UnityEngine;

enum Bullet_type
{
    Null,
    Homing,
    Explosive,
}


public class Bullet_Action : MonoBehaviour
{
    [SerializeField] float life_time;//íeä€ÇÃê∂ë∂éûä‘
    [SerializeField] Bullet_type type;
    private void Start()
    {
        Destroy(gameObject,life_time);
    }
    private void Update()
    {
        if(type == Bullet_type.Homing)
        {
            //ÉzÅ[É~ÉìÉOíe
        }
        else if(type == Bullet_type.Explosive)
        {
            //îöî≠íe

        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("ìñÇΩÇ¡ÇΩ");
            Destroy(gameObject);
        }
    }

}