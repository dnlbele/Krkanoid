using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour {

    public GameObject introText;
    public GameObject ballLight;

    private Rigidbody2D rb2D;
    private LineRenderer laserLine;
    private GameObject stick;
    private float lastYpos = 0;
    private float weaponRange = 1f;
    private bool ballInPlay;
    private int countLastPos = 0;
    private float ballInitialVelocityY = 500f;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        laserLine = GetComponent<LineRenderer>();
    }


    void Update()
    {
        if (!ballInPlay)
        {
            laserLine.SetPosition(0, transform.position);
            laserLine.SetPosition(1, Camera.main.transform.forward * weaponRange);
        } else
        {
            transform.localScale = new Vector3(2.7f, 2.7f, 1f);
            if (transform.position.y - lastYpos < 0.01f)
            {
                countLastPos++;
            } else
            {
                countLastPos = 0;
            }
            if (countLastPos > 10)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - 0.01f, transform.position.z);
            }
        }

        if (Input.GetButtonDown("Jump") && !ballInPlay)
        {
            stick = transform.parent.gameObject; 
            transform.parent = null;
            stick.GetComponent<Animator>().SetBool("shoot", true);
            ballInPlay = true;
            rb2D.isKinematic = false;
            introText.SetActive(false);
            StartCoroutine("Shoot");
        }
        transform.Rotate(new Vector3(0, 0, 360 * Time.deltaTime));
        if (transform.position.y < -8.8f)
        {
            ballLight.SetActive(false);
        }
        else
        {
            ballLight.SetActive(true);
        }

        lastYpos = transform.position.y;
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(0.1f);
        float velocityX = (laserLine.transform.position.x / laserLine.transform.position.y) * ballInitialVelocityY;
        rb2D.AddForce(new Vector2(velocityX, ballInitialVelocityY));
        laserLine.enabled = false;
        stick.GetComponent<Animator>().SetBool("shoot", false);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Stick"))
        {
            float x = hitFactor(transform.position,
                                col.transform.position,
                                col.collider.bounds.size.x);
            Vector2 dir = new Vector2(x, 1).normalized;
            rb2D.velocity = dir * 3 + rb2D.velocity;
        }
    }

    float hitFactor(Vector2 ballPos, Vector2 racketPos,
               float racketWidth)
    {
        return (ballPos.x - racketPos.x) / racketWidth*10;
    }


}
