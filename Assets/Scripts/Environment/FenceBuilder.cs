using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(FenceBuilder))]
public class FenceBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        FenceBuilder targetEditor = (FenceBuilder)target;

        if (GUILayout.Button("Generate Fence")) 
        {
            targetEditor.BuildFence();
        }
    }
}
#endif

public class FenceBuilder : MonoBehaviour
{
    [HideInInspector] public List<GameObject> createdFencesLeft = new List<GameObject>();
    [HideInInspector] public List<GameObject> createdFencesRight = new List<GameObject>();

    public int fenceAmount = 1;

    public bool buildLeft = true;
    public bool buildRight = true;

    public float leftX = 3;
    public float rightX = 3;

    private float zOffset = 6;


    private Quaternion oldRotation = Quaternion.identity;

    public void Reset()
    {
        ClearFences();
        gameObject.name = "FenceBuilder";
    }


    void Awake() => enabled = false;

    void ClearFences()
    {
        if (createdFencesLeft.Count > 0)
        {
            for (int i = 0; i < createdFencesLeft.Count; i++)
            {
                if (createdFencesLeft[i] != null)
                    DestroyImmediate(createdFencesLeft[i]);
            }

            createdFencesLeft.Clear();
        }

        if (createdFencesRight.Count > 0)
        {
            for (int i = 0; i < createdFencesRight.Count; i++)
            {
                if (createdFencesRight[i] != null)
                    DestroyImmediate(createdFencesRight[i]);
            }

            createdFencesRight.Clear();
        }
    }

    public void BuildFence()
    {
        ClearFences();
        GameObject fencePrefab = Resources.Load<GameObject>("fence");

        oldRotation = transform.rotation;
        transform.rotation = Quaternion.identity;

        for(int i = 0; i < fenceAmount; i++)
        {
            if (buildLeft)
            {
                GameObject newFence = Instantiate(fencePrefab);
                newFence.name = "LeftFence_" + i.ToString();
                newFence.transform.parent = transform;

                createdFencesLeft.Add(newFence);

                newFence.transform.position = new Vector3(transform.position.x + leftX, transform.position.y, (i == 0 ? transform.position.z : createdFencesLeft[i - 1].transform.position.z + zOffset));
            }

            if (buildRight)
            {
                GameObject newFence = Instantiate(fencePrefab);
                newFence.name = "LeftFence_" + i.ToString();
                newFence.transform.parent = transform;

                createdFencesRight.Add(newFence);

                newFence.transform.position = new Vector3(transform.position.x - rightX, transform.position.y, (i == 0 ? transform.position.z : createdFencesRight[i - 1].transform.position.z + zOffset));
            }

        }

        transform.rotation = oldRotation;
    }
}
