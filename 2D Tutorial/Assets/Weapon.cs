using UnityEngine;
using System.Collections;
using System;

public class Weapon : MonoBehaviour {

    public float fireRate = 0;  //0 = single fire, 1 = burst
    public float Damage = 10;
    public float weaponRange = 100;
    public LayerMask toHit; // pozwala wybrać warstwy (layer) które nie będą zatrzymywały pocisków
    public Transform BulletTrailPrefab;
    public float effectSpawnRate = 10;
    public Transform MuzzleFlashPrefab;

    float timeToFire = 0;
    Transform firePoint;
    float timeToSpawnEffect = 0;


    // Use this for initialization
    void Start () {
	    
	}

    void Awake() {
        firePoint = transform.FindChild("FirePoint");
        if (firePoint == null)
        {
            Debug.LogError("No firepoint...");
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (fireRate == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetMouseButton(0) && Time.time > timeToFire)
            {
                timeToFire = Time.time + 1 / fireRate;
                Shoot();
            }
        }
	}

    private void Shoot()
    {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2((firePoint.position).x, (firePoint.position).y);
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition - firePointPosition, weaponRange, toHit);
        if (Time.time >= timeToSpawnEffect) //this line is to prevent generate to many objects at the same time
        {
            MuzzleFlash();
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }
        //Debug.DrawLine(firePointPosition, mousePosition, Color.yellow);   // from point to point
        //Debug.DrawLine(firePointPosition, (mousePosition - firePointPosition) *100, Color.yellow);   // from point to infinite
        
        //if (hit.collider != null) {
        //    Debug.DrawLine(firePointPosition, hit.point, Color.red);
        //}
    }

    private void MuzzleFlash()
    {
        Instantiate(BulletTrailPrefab, firePoint.position, firePoint.rotation);
        Transform clone = Instantiate(MuzzleFlashPrefab, firePoint.position, firePoint.rotation) as Transform;
        clone.parent = firePoint;
        float size = UnityEngine.Random.Range(0.6f, 0.9f);
        clone.localScale = new Vector3(size, size, 0);

        //TRICK TO RENDER ELEMENT FOR ONLY 1 FRAME
        //yield return 0;
        //Destroy(clone);

        //TRICK TO RENDER ELEMENT FOR AMOUNT OF TIME
        Destroy(clone.gameObject,0.02f);
    }
}
