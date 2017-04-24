using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchWeapon : MonoBehaviour {

    public GameObject gun;
    public GameObject semiautogun;

    void Start()
    {
        semiautogun.SetActive(false);
    }

	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            switchWep();
        }		
	}

    private void switchWep()
    {
        if(gun.activeSelf)
        {
            gun.SetActive(false);
            semiautogun.SetActive(true);
        } else
        {
            gun.SetActive(true);
            semiautogun.SetActive(false);
        }
    }
}
