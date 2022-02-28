using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(TreeBuilder))]
public class TreeBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TreeBuilder targetBuilder = (TreeBuilder)target;

        if(GUILayout.Button("Build Tree"))
        {
            targetBuilder.BuildTrees();
        }
    }
}
#endif

//My legendary Tree Generation Code™
public class TreeBuilder : MonoBehaviour
{
    public GameObject treePrefab;

    public int treeAmount = 1;
    public int rowAmount = 1;
    public int rowDistanceX = 10;
    public int rowDistanceZ = 12;

    public bool buildTowardsRight = true; //doesnt actually do what it says you'll be disappointed i didn't have time to fix sorry

    public Dictionary<int, List<GameObject>> treeRows = new Dictionary<int, List<GameObject>>();
    public List<GameObject> allTrees = new List<GameObject>(); //allTrees list because Dictionaries don't serialize


    public bool doRandomRotation = false;
    public Vector2 randomRotationRange = Vector2.zero;


    private Quaternion oldRotation = Quaternion.identity;
    
    public void Reset()
    {
        gameObject.name = "TreeBuilder";
        ClearTrees();

        treePrefab = Resources.Load<GameObject>("tree");
    }

    void ClearTrees()
    {
        for(int i = 0; i < allTrees.Count; i++)
        {
            if (allTrees[i] != null)
                DestroyImmediate(allTrees[i]);
        }

        treeRows.Clear();
        allTrees.Clear();
    }

    public void BuildTrees()
    {
        ClearTrees();

        oldRotation = transform.rotation;

        transform.rotation = Quaternion.identity;

        for(int i = 0; i < rowAmount; i++)
        {
            treeRows[i] = new List<GameObject>();

            for(int k = 0; k < treeAmount; k++)
            {
                GameObject newTree = Instantiate(treePrefab);
                newTree.name = "Tree_Row" + i.ToString() + "_" + k.ToString();
                newTree.transform.parent = transform;


                Random.InitState(UnityEngine.Random.Range(0, 1000));

                newTree.transform.position = new Vector3((k == 0 ? transform.position.x : treeRows[i][k - 1].transform.position.x + (buildTowardsRight ? rowDistanceX : -rowDistanceX)), transform.position.y, (i == 0 ? transform.position.z : treeRows[i - 1][0].transform.position.z + rowDistanceZ));

                if (doRandomRotation)
                    newTree.transform.eulerAngles = new Vector3(transform.eulerAngles.x, Random.Range(randomRotationRange.x, randomRotationRange.y), transform.eulerAngles.z);

                treeRows[i].Add(newTree);
                allTrees.Add(newTree);

            }
        }

        transform.rotation = oldRotation;
    }
}
