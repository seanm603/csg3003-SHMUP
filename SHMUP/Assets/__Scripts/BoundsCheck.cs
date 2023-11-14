using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsCheck : MonoBehaviour
{
    public enum bType {  center, inset, outset, height, width };

    [Header("GameObject Boundaries")]
    public bType boundsType = bType.center;
    public float radius = 1f;

    [Header("Camera Boundaries")]
    public float camWidth;
    public float camHeight;

    // Start is called before the first frame update
    void Awake()
    {
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;
    }

    void LateUpdate()
    {
        float checkHeight = 0;
        float checkWidth = 0;

        if (boundsType == bType.inset) 
        {
            checkHeight = -radius;
            checkWidth = -radius;
        };
        if (boundsType == bType.outset)
        {
            checkHeight = radius;
            checkWidth = radius;
        }
        if (boundsType == bType.height)
        {
            checkHeight = -radius;
            checkWidth = radius;
        }
        if (boundsType == bType.width)
        {
            checkHeight = radius;
            checkWidth = -radius;
        }
        Vector3 pos = transform.position;

        pos.x = Mathf.Min(Mathf.Max(pos.x, -camWidth-checkWidth), camWidth+checkWidth);
        pos.y = Mathf.Min(Mathf.Max(pos.y, -camHeight-checkHeight), camHeight+checkHeight);

        transform.position = pos;
    }

}
