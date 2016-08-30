using UnityEngine;
using System.Collections;

public class ParticlesScript : MonoBehaviour
{

    public float lifetime;
    void Start()
    {        
        Destroy(gameObject, lifetime);
    }



}
