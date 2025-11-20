using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpChargeMax = 2f;
    public float jumpCharge = 0f;
    public float chargeMultiplier = 1f;
    public float sinPeriod = 0.006f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            jumpCharge += Time.deltaTime * chargeMultiplier;

            //GetComponent<SpriteRenderer>().color = new Color(Mathf.Sin(400 * jumpCharge * sinPeriod) * 125 + 125, Mathf.Sin(400 * jumpCharge * sinPeriod) * 125 + 125, Mathf.Sin(400 * jumpCharge * sinPeriod) * 125 + 125);

            if(jumpCharge > jumpChargeMax)
                jumpCharge = jumpChargeMax;
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            GetComponent<Rigidbody2D>().AddForceY(jumpCharge);
            GetComponent<SpriteRenderer>().color = Color.white;
            jumpCharge = 0;
        }
    }
}
