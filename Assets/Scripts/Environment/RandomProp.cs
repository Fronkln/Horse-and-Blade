using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomProp : MonoBehaviour
{
    private RootScript root;

    [Header("Used to force pick a specific element from random props, -1 means it's random")]
    public int forceObjectID = -1;

    private GameObject curModel;


    // Start is called before the first frame update
    void Start()
    {
        root = RootScript.instance;
        PickProp();
    }

   public void PickProp()
    {
        GameObject newProp = null;

        if (curModel != null) DestroyImmediate(curModel);

        if (forceObjectID == -1) newProp = Instantiate(root.randomPropObjects[Random.Range(0, root.randomPropObjects.Length)]);
        else newProp = Instantiate(root.randomPropObjects[forceObjectID]);

        newProp.transform.position = transform.position;
        newProp.transform.parent = transform;

        curModel = newProp;
    }
}
