using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttackZone : MonoBehaviour
{
    PlayerScript player;

    // Start is called before the first frame update
    void Awake()
    {
        player = transform.root.GetComponent<PlayerScript>();
    }


    void OnCollisionEnter(Collision coll)
    {
        if (player.dead) return;

        if(coll.transform.tag == "Enemy")
        {
            player.swordSlash.pitch = Random.Range(0.85f, 1.15f);
            player.swordSlash.PlayOneShot(player.swordSlash.clip);
            player.anim.Play("swing", 0, 0);
            // Destroy(coll.gameObject);
            coll.transform.gameObject.SetActive(false);
        }
    }

}
