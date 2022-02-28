using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompletorScript : MonoBehaviour
{
    private RootScript root;
    [HideInInspector]public bool completeLevelDoOnce = false;

    // Start is called before the first frame update
    void Start()
    {
        root = RootScript.instance;
        root.currentLevelFinisher = this;
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
            if (!completeLevelDoOnce)
            {
                completeLevelDoOnce = true;
                root.OnPlayerCompleteLevel();
            }
    }

}
