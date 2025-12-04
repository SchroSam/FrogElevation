using UnityEngine;

[System.Serializable]
public enum PowerType {Launch};

public class PowerUp : MonoBehaviour
{

    public PowerType powerType;
    public float launchForce = 100f;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Tongue")
        {
            switch (powerType)
            {
                case PowerType.Launch:
                    collision.transform.parent.GetComponent<Rigidbody2D>().AddForceY(launchForce);

                    break;



            }

            collision.transform.parent.GetComponent<PlayerController>().GrabbedItem();
            Destroy(gameObject);
        }
    }

}
