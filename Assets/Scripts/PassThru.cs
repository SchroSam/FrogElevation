using UnityEngine;

public class PassThru : MonoBehaviour
{
    GameObject player;
    void Awake()
    {
        player = GameObject.Find("Frog");
    }
    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.y - 0.2f < transform.position.y)
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
        else
        {
            GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }
}
