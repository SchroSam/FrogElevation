using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpChargeMax = 2f;
    public float jumpCharge = 0f;
    public float chargeMultiplier = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpCharge += Time.deltaTime * chargeMultiplier;

            if(jumpCharge > jumpChargeMax)
                jumpCharge = jumpChargeMax;
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            GetComponent<Rigidbody2D>().AddForceY(jumpCharge);
            jumpCharge = 0;
        }
    }
}
