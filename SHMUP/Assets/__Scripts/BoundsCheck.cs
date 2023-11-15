using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsCheck : MonoBehaviour
{
    [System.Flags]
    public enum eScreenLocs {
        onScreen = 0,
        offRight = 1,
        offLeft = 2,
        offUp = 4,
        offDown = 8
    }
    public enum bType {  center, inset, outset, height, width };

    [Header("GameObject Boundaries")]
    public bType boundsType = bType.center;
    public float radius = 1f;
    public bool keepOnScreen = true;

    [Header("Camera Boundaries")]
    public float camWidth;
    public float camHeight;
    public eScreenLocs screenLocs = eScreenLocs.onScreen;


    // Start is called before the first frame update
    void Awake()
    {
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;
    }

    void LateUpdate()
    {
        
        float checkRadius = 0f;
        if (boundsType == bType.inset) checkRadius = -radius;
        if (boundsType == bType.outset) checkRadius = radius;

        Vector3 pos = transform.position;
        screenLocs = eScreenLocs.onScreen;

        if ( pos.x > camWidth + checkRadius ){
            pos.x = camWidth + checkRadius;
            screenLocs |= eScreenLocs.offRight;
        }
        if ( pos.x < -camWidth - checkRadius ){
            pos.x = -camWidth - checkRadius;
            screenLocs |= eScreenLocs.offLeft;
        }
        if ( pos.y > camHeight + checkRadius ){
            pos.y = camHeight + checkRadius;
            screenLocs |= eScreenLocs.offUp;
        }
        if (pos.y < -camHeight - checkRadius ){
            pos.y = -camHeight - checkRadius;
            screenLocs |= eScreenLocs.offDown;
        }
        if (keepOnScreen && !isOnScreen){
            transform.position = pos;
            screenLocs = eScreenLocs.onScreen;
        }
    }

    public bool isOnScreen {
        get { return (screenLocs == eScreenLocs.onScreen); }
    }

    public bool LocIs(eScreenLocs checkLoc) {
        if (checkLoc == eScreenLocs.onScreen) return isOnScreen;
        return ( (screenLocs & checkLoc) == checkLoc );
    }

}
