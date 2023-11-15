using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HeroScript : MonoBehaviour
{
    static public HeroScript S { get; private set; }
    // "S" stands for singleton which is a design concept
    // {...} is a convenience feature
    // Only allowing a public get, but set remains private
    // There will only be one unique Hero
    [Header("Ship Movement")]
    public float speed = 30f;
    public float rollMult = -45f;
    public float pitchMult = 30;

    public GameObject projectilePrefab;
    public float projectileSpeed = 40f;

    [Header("Dynamic")]
    [Range(0, 4)]
    [SerializeField]
    private int _shieldLevel = 1;
    [Tooltip("This field holds a reference to the last triggering GameObject")]
    private GameObject lastTriggerGo = null;


    void Awake()
    {
        if (S == null)
        {
            S = this;
        }
        else
        {
            Debug.LogError("HeroScript.Awake() - Attempted to create a second HeroScript");
        }
    }

    // Update is called once per frame
    void Update()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");
        
        Vector3 pos = transform.position;
        pos.x += hAxis * speed * Time.deltaTime;
        pos.y += vAxis * speed * Time.deltaTime;
        transform.position = pos;

        transform.rotation = Quaternion.Euler(vAxis * pitchMult, hAxis * rollMult, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TempFire();
        }
    }
    void TempFire(){
        GameObject projGO = Instantiate<GameObject>(projectilePrefab);
        projGO.transform.position = transform.position;
        Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
        rigidB.velocity = Vector3.up * projectileSpeed;
    }

    void OnTriggerEnter(Collider other) {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;

        if (go == lastTriggerGo) {
            return;
        }
        lastTriggerGo = go;

        Enemy enemy = go.GetComponent<Enemy>();
        if (enemy != null) {
            shieldLevel--;
            Destroy(go);
        } else {
            Debug.Log("Triggered by non-Enemy: " + go.name);
        }
    }
    public int shieldLevel {
        get { return (_shieldLevel); }
        private set {
            _shieldLevel = Mathf.Min(value, 4);
            if (value < 0) {
                Destroy(this.gameObject);
                Main.HERO_DIED();
            }
        }
    }
}
