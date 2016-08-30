using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StickScript : MonoBehaviour {


    private float speed = 0.5f;
    private Rigidbody2D rb2D;
    private bool enlarged = false;

    public GameObject turboModeText;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private float screenWidthPlusStick = 20.6f; 

	void LateUpdate () {
        float xPos = transform.position.x + Input.GetAxis("Horizontal") * speed;
        float stickWidth = this.GetComponent<SpriteRenderer>().bounds.size.x;
        float xPositionMax = (screenWidthPlusStick - stickWidth)/2;
        Vector2 newPositon = new Vector2(Mathf.Clamp(xPos, -xPositionMax, xPositionMax), transform.position.y);
        rb2D.MovePosition(newPositon);	
	}

    public void EnlargeStick()
    {
        if (!enlarged)
        {
            StartCoroutine("Enlarge");
            enlarged = true;
        }
    }

    IEnumerator Enlarge()
    {
        turboModeText.SetActive(true);
        float start = 0;
        float end = 10f;
        float step = 0.01f;
        while (start < end)
        {
            while (this.gameObject.transform.localScale.x < 5)
            {
                this.gameObject.transform.localScale = new Vector3(this.gameObject.transform.localScale.x + step, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
                yield return new WaitForSeconds(step);
            }
            start += 1f;
            yield return new WaitForSeconds(1f);
        }
        while (this.gameObject.transform.localScale.x > 3.5)
        {
            this.gameObject.transform.localScale = new Vector3(this.gameObject.transform.localScale.x - step, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
            yield return new WaitForSeconds(step);
        }
        enlarged = false;
        turboModeText.SetActive(false);
    }
}
