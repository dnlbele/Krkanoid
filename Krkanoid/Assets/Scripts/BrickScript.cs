using UnityEngine;
using System.Collections;
using System;

public class BrickScript : MonoBehaviour {

    public GameObject explosion;
    public Sprite redX;
    public StickScript stickScript;

    private AudioSource audioSource;
    private float ballConstantVelocity = 10f;
    private int brickScoreValue = 10;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Vector2 velocity = other.gameObject.GetComponent<Rigidbody2D>().velocity;
        other.gameObject.GetComponent<Rigidbody2D>().velocity = ballConstantVelocity * (velocity.normalized);
        audioSource.Play();
        Instantiate(explosion, transform.position, transform.rotation);
        this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        this.gameObject.GetComponent<Animator>().SetTrigger("hit");
        StartCoroutine(ShowRedX());
        
        if (!this.gameObject.CompareTag("Joker"))
        {
            GuiManager.instance.Damage(this.gameObject.tag);
        } else
        {
            stickScript.EnlargeStick();
        }
        GuiManager.instance.AddScore(brickScoreValue);
    }

    private IEnumerator ShowRedX()
    {
        yield return new WaitForSeconds(.6f);
        this.gameObject.GetComponent<SpriteRenderer>().sprite = redX;
    }
}
