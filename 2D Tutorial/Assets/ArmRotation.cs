using UnityEngine;
using System.Collections;

public class ArmRotation : MonoBehaviour {

    public float rotationOffset = 90;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position; //roznica pomiedzy kursorem (po zmapowaniu ekran -> swiat gry) a bohaterem
        difference.Normalize(); //normalizacja (suma wektorow = 1)

        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + rotationOffset);
	}
}
