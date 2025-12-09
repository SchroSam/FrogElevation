using UnityEngine;

[System.Serializable]
public enum PowerType {Launch, Points, PointMult};

public class PowerUp : MonoBehaviour
{

    public PowerType powerType;
    public float launchForce = 100f;
    public float pointVal = 5f; 

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Tongue")
        {
            switch (powerType)
            {
                case PowerType.Launch:
                    collision.transform.parent.GetComponent<Rigidbody2D>().AddForceY(launchForce);

                    break;

                case PowerType.Points:
                    FindFirstObjectByType<PlayerController>().AddPoints(pointVal);

                    break;

                case PowerType.PointMult:
                    FindFirstObjectByType<PlayerController>().PointMult();
                    
                    break;


            }

            collision.transform.parent.GetComponent<PlayerController>().GrabbedItem();
            Destroy(gameObject);
        }
    }

}
