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

    [Header("Dynamic")]
    [Range(0, 4)]
    public int shieldLevel = 1;


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
    }
}
