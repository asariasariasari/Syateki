using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syateki;

public class BreakTarget : MonoBehaviour {

    [SerializeField] private float extistTime = 1f;
    [SerializeField] private AudioClip breakSound;
    private float power = 400f;
    // Use this for initialization
    void Start () {
        this.GetComponent<AudioSource>().PlayOneShot(breakSound);

        foreach(Transform child in transform){
            var r = child.GetComponent<Rigidbody>();
            if (r == null) continue;
            var forceVec = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            r.AddForce(forceVec.normalized * power);
        }
        Destroy(gameObject, extistTime);
	}
}
