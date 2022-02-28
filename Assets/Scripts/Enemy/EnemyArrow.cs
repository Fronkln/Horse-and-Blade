using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : MonoBehaviour
{
    public AudioSource arrowHitSound;

    private bool hitObstacle = false;
    private float arrowSpeed = 24;

    private float projectileLife = 6; //dead after 6 seconds;
    private float curProjectileTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (RootScript.instance.player.dead) { DestroyImmediate(gameObject); return; }


        transform.Translate(new Vector3(0, 0, (Time.deltaTime * arrowSpeed)));

        curProjectileTime += Time.deltaTime;

        if (curProjectileTime >= projectileLife) DestroyImmediate(gameObject);
    }

    void OnCollisionEnter(Collision coll)
    {
        if (hitObstacle) return;

        if (coll.transform.name == "Player")
        {
            RootScript.instance.player.dead = true;
            Destroy(gameObject);
        }
        else if (coll.transform.gameObject.tag == "Obstacle") //if we are hitting an obstacle stop the arrow for a nice effect
        {
            arrowHitSound.Play();
            hitObstacle = true;
            enabled = false;
        }
    }
}
