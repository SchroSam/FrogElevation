using System;
using System.Collections.Generic; // Needed for HashSet
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
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
    public float totalPoints = 0f;
    public float currentMult = 1f;
    public float multTimeLeft = 0f;
    public float multTimeToAdd = 3f;
    public float multValAdd = 0.3f;
    private bool multTimerOn = false;
    private GameObject chargeMeter;
    private GameObject dirMeter;
    private GameObject tongue;
    private TMP_Text timerText;
    private TMP_Text pointText;
    private TMP_Text multText;

    // NEW: Death handling
    public float fallVelocityToDie = 10f;  // Positive Y velocity threshold
    public GameObject deathCanvas;         // Assign in Inspector
    private bool isDead = false;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;

    // NEW: Track platforms that have already given points
    private HashSet<GameObject> scoredPlatforms = new HashSet<GameObject>();

    void Start()
    {
        chargeMeter = GameObject.Find("ChargeMeter");
        dirMeter = GameObject.Find("DirMeter");
        tongue = GameObject.Find("Tongue");
        timerText = GameObject.Find("Timer").GetComponent<TMP_Text>();
        pointText = GameObject.Find("Points").GetComponent<TMP_Text>();
        multText = GameObject.Find("Mult").GetComponent<TMP_Text>();

        dirMeter.GetComponent<Slider>().maxValue = dirAbsMax;
        dirMeter.GetComponent<Slider>().minValue = -dirAbsMax;
        dirMeter.GetComponent<Slider>().value = 0f;

        startTongueSize = tongue.transform.localScale.x;

        // NEW
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        if (deathCanvas != null)
            deathCanvas.SetActive(false);
    }

    void Update()
    {
        if (isDead) return; // stop input after death

        // ------------------------------
        // NEW: Check death by velocity
        // ------------------------------
        if (rb.linearVelocity.y < -fallVelocityToDie)
        {
            EndGame();
            return;
        }

        // ------------------------------
        // Jump Logic
        // ------------------------------
        if (Input.GetKey(KeyCode.Space))
        {
            jumpCharge += Time.deltaTime * chargeMultiplier;

            if (jumpCharge > jumpChargeMax)
                jumpCharge = jumpChargeMax;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            rb.AddForceY(jumpCharge);
            rb.AddForceX(dirVal);
            dirVal = 0;
            jumpCharge = 0;
        }

        chargeMeter.GetComponent<Slider>().value = jumpCharge / jumpChargeMax;

        // ------------------------------
        // Aiming Logic
        // ------------------------------
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            dirVal -= Time.deltaTime * dirMult;
            if (Math.Abs(dirVal) > dirAbsMax)
                dirVal = -dirAbsMax;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            dirVal += Time.deltaTime * dirMult;
            if (Math.Abs(dirVal) > dirAbsMax)
                dirVal = dirAbsMax;
        }

        dirMeter.GetComponent<Slider>().value = dirVal;

        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 direction = mousePos - tongue.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        tongue.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Tongue logic
        if (Input.GetKeyDown(KeyCode.Mouse0) && !tongueTime)
        {
            tongueTime = true;
        }

        if (tongueTime && !tongueReverse)
        {
            tongue.transform.localScale = new Vector3(tongue.transform.localScale.x + Time.deltaTime * tongueGrowSpeed, tongue.transform.localScale.y, tongue.transform.localScale.z);

            if (tongue.transform.localScale.x >= maxTongueSize)
                tongueReverse = true;
        }
        else if (tongueTime && tongueReverse)
        {
            tongue.transform.localScale = new Vector3(tongue.transform.localScale.x - Time.deltaTime * tongueGrowSpeed, tongue.transform.localScale.y, tongue.transform.localScale.z);

            if (tongue.transform.localScale.x <= startTongueSize)
            {
                tongueReverse = false;
                tongueTime = false;
            }
        }

        // Multiplier timer
        if (multTimerOn)
        {
            multTimeLeft -= Time.deltaTime;
            if (multTimeLeft <= 0)
            {
                multTimeLeft = 0;
                multTimerOn = false;
                currentMult = 1f;
                multText.text = $"Multiplier: {currentMult:F2}";
            }
        }
        timerText.text = $"MultTime: {multTimeLeft:F2}";
    }

    private void EndGame()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("GAME OVER");

        // Turn off BoxCollider2D so frog falls through platforms
        if (boxCollider != null)
            boxCollider.enabled = false;

        // Show death canvas
        if (deathCanvas != null)
            deathCanvas.SetActive(true);
    }

    public void GrabbedItem()
    {
        tongueTime = true;
        tongueReverse = true;
    }

    public void AddPoints(float val)
    {
        totalPoints += val * currentMult;
        pointText.text = $"Points: {totalPoints:F2}";
    }

    public void PointMult()
    {
        multTimerOn = true;
        multTimeLeft += multTimeToAdd;
        currentMult += multValAdd;
        multText.text = $"Multiplier: {currentMult:F2}";
    }

    // ------------------------------
    // NEW: Score when landing on platforms (only once per platform)
    // ------------------------------
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            // Only give points if this platform hasn't been scored yet
            if (!scoredPlatforms.Contains(collision.gameObject))
            {
                scoredPlatforms.Add(collision.gameObject);
                ScoreManager.Instance.AddPoints((int)(100 * currentMult));
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Nothing needed here for scoring now
    }
}
