using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchWeapon : MonoBehaviour {

    public GameObject gun;
    public Image gunImage;
    public GameObject semiautogun;
    public Image semiImage;
    public Text ammoText;

    List<GameObject> activeWeps = new List<GameObject>();

    void Start()
    {
        activeWeps.Add(gun);
        semiautogun.SetActive(false);
        semiImage.enabled = false;
    }

	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q) && activeWeps.Count > 1)
        {
            switchWep();
        }

        if (semiautogun.transform.GetChild(4).GetComponent<PlayerShooter>().getAmmo() < 1 && semiautogun.activeSelf)
        {
            switchWep();
            activeWeps.Remove(semiautogun);
            semiautogun.transform.GetChild(4).GetComponent<PlayerShooter>().resetAmmo();
        }

        if (semiautogun.activeSelf)
        {
            ammoText.text = "Ammo: " + semiautogun.transform.GetChild(4).GetComponent<PlayerShooter>().getAmmo();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SemiAuto")
        {
            activeWeps.Add(semiautogun);
            Destroy(other.gameObject);
        }
    }

    private void switchWep()
    {
        

        if(gun.activeSelf)
        {
            gun.SetActive(false);
            semiautogun.SetActive(true);
            ammoText.text = "Ammo: " + semiautogun.transform.GetChild(4).GetComponent<PlayerShooter>().getAmmo();
            gunImage.enabled = false;
            semiImage.enabled = true;
        } else
        {
            gun.SetActive(true);
            semiautogun.SetActive(false);
            gunImage.enabled = true;
            semiImage.enabled = false;
            ammoText.text = "Ammo: Infinite";
        }
    }

    /* private class Weapon
    {

        public String name;
        public Image icon;
        public int damage;
        public int shots
    } */
}
