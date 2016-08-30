using UnityEngine;
using System.Collections;

public class DeathOnContact : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Ball"))
        {
            GameManager.instance.GameOver();
        }
    }
}
