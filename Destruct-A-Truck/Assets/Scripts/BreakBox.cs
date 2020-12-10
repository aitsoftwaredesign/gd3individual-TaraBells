using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBox : MonoBehaviour
{
    private ParticleSystem BoxExplode;
    public MeshRenderer sr;


    private void Update()
    {
        Destroy(gameObject, 10);
    }

    private void Awake()
    {
        BoxExplode = GetComponentInChildren <ParticleSystem>();
        sr = GetComponent<MeshRenderer>();

    }

    //detecting collisions using tags
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            KeepScore.Score += 5;
        }
            if (collision.gameObject.tag == "Player")
        {
            Debug.Log("CollisionSuccess");

            StartCoroutine(Break());
        }
       
    }

    // this IEnumerator Break () plays the particle system when a box is broken it will last its lifetime then be destroyed

    private IEnumerator Break()
    {
        BoxExplode.Play();

        sr.enabled = false;

        yield return new WaitForSeconds(BoxExplode.main.startLifetime.constantMax);

        Destroy(gameObject);
        
    }
}
