using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    private PlayerScript player;
    private RootScript root;

    Vector3 mPrevPos;
    Vector3 mPosDelta;

    public float maxY = 300;
    public float minY = 60;


    void Start()
    {
        root = RootScript.instance;
        player = GetComponent<PlayerScript>();
    }

    void Update()
    {

        if (root.levelFinished || !root.levelStarted || player.dead)
            return;
        


#if !MOBILE_INPUT
        if (Input.GetMouseButton(0))
        {
            mPosDelta = Input.mousePosition - mPrevPos;
            transform.Rotate(Camera.main.transform.up, Vector3.Dot(mPosDelta, Camera.main.transform.right), Space.World);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

            if (transform.eulerAngles.y > maxY - 70 && transform.eulerAngles.y < maxY)
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, maxY, transform.eulerAngles.z);
            if(transform.eulerAngles.y > minY && transform.eulerAngles.y < maxY)
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, minY, transform.eulerAngles.z);
        }

        mPrevPos = Input.mousePosition;
#else
        if (Input.touches.Length > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) return;

            if (Input.GetMouseButton(0))
            {
                mPosDelta = Input.mousePosition - mPrevPos;
                transform.Rotate(Camera.main.transform.up, Vector3.Dot(mPosDelta, Camera.main.transform.right), Space.World);
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

                print(transform.eulerAngles.y);

                if (transform.eulerAngles.y > maxY - 70 && transform.eulerAngles.y < maxY)
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, maxY, transform.eulerAngles.z);
                if (transform.eulerAngles.y > minY && transform.eulerAngles.y < maxY)
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, minY, transform.eulerAngles.z);
            }

            mPrevPos = touch.position;
        }
#endif

    }
}
