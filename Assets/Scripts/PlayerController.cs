using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float jumpChargeMax = 2f;
    public float jumpCharge = 0f;
    public float chargeMultiplier = 1f;
    public float dirAbsMax = 1f;
    public float dirVal = 0f;
    public float dirMult = 1f;
    public float tongueGrowSpeed = 1f;
    public float maxTongueSize = 2f;
    private float startTongueSize;
    private bool tongueTime = false;
    private bool tongueReverse = false;
    //public float sinPeriod = 0.006f;
    private GameObject chargeMeter;
    private GameObject dirMeter;
    private GameObject tongue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        chargeMeter = GameObject.Find("ChargeMeter");
        dirMeter = GameObject.Find("DirMeter");
        tongue = GameObject.Find("Tongue");

        dirMeter.GetComponent<Slider>().maxValue = dirAbsMax;
        dirMeter.GetComponent<Slider>().minValue = -dirAbsMax;
        dirMeter.GetComponent<Slider>().value = 0f;

        startTongueSize = tongue.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        //Jump Logic
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
            GetComponent<Rigidbody2D>().AddForceX(dirVal);
            GetComponent<SpriteRenderer>().color = Color.white;
            dirVal = 0;
            jumpCharge = 0;
        }

        chargeMeter.GetComponent<Slider>().value = jumpCharge / jumpChargeMax;


        //Aiming Logic
        if (Input.GetKey(KeyCode.A))
        {
            dirVal -= Time.deltaTime * dirMult;

            if(Math.Abs(dirVal) > dirAbsMax)
                dirVal = -dirAbsMax;
        }

        else if (Input.GetKey(KeyCode.D))
        {
            dirVal += Time.deltaTime * dirMult;

            if(Math.Abs(dirVal) > dirAbsMax)
                dirVal = dirAbsMax;
        }


        dirMeter.GetComponent<Slider>().value = dirVal;

        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector2 direction = mousePos - tongue.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        tongue.transform.rotation = Quaternion.Euler(0f, 0f, angle);


        //Tongue Grab logic
        //TODO

        if (Input.GetKeyDown(KeyCode.Mouse0) && !tongueTime)
        {
            tongueTime = true;
            Debug.Log("Start Tongue");
        }

        if (tongueTime && !tongueReverse)
        {
            tongue.transform.localScale = new Vector3(tongue.transform.localScale.x + Time.deltaTime * tongueGrowSpeed, tongue.transform.localScale.y, tongue.transform.localScale.z);

            if(tongue.transform.localScale.x >= maxTongueSize){
                tongueReverse = true;
                Debug.Log("Reverse Tongue");
            }
        }

        else if(tongueTime && tongueReverse)
        {
            tongue.transform.localScale = new Vector3(tongue.transform.localScale.x - Time.deltaTime * tongueGrowSpeed, tongue.transform.localScale.y, tongue.transform.localScale.z);
            
            if(tongue.transform.localScale.x <= startTongueSize)
            {
                tongueReverse = false;
                tongueTime = false;
            }
        }

    }
}
