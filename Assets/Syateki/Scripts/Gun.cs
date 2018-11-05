using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syateki;

public class Gun : MonoBehaviour {
    [SerializeField] private int number;
    private Alignment alignment;
    private bool canShot = true;
    [SerializeField] private AudioClip shotSound;
    private AudioSource audioSource;
    private Action gunApdate;

    // Use this for initialization
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.3f;
        if (GameManager.Instance.JoyconMode) gunApdate = () => { if (JoyconController.Instance.PusedRightButtonDownLength(number) && canShot) Shoting(); };
        else gunApdate = () => { if (Input.GetMouseButtonDown(0) && canShot) Shoting(); };
    }
    public void Init () {
        alignment = AlignmentContorller.Instance.GetAlignment(number);
    }
	
	// Update is called once per frame
	void Update () {
        
        if (alignment == null)
        {
            Debug.Log("alignment null");
            return;
        }

        gunApdate();
	}

    private void Shoting(){
        canShot = false;
        alignment.Shot();
        StartCoroutine(ShotInterval());
        audioSource.PlayOneShot(shotSound);
    }
    //射撃間隔
    private IEnumerator ShotInterval()
    {
        yield return new WaitForSeconds(Constants.GunSetting.INTERVAL_TIME);
        canShot = true;
    }
}
