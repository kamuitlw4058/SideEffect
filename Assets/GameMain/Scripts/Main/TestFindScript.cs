using System.Collections;
using System.Collections.Generic;
using Pangoo.Common;
using Pangoo.MetaTable;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEditor;

public class TestFindScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    [Button]
    public void Test()
    {
        var className = "Pangoo.MetaTable.VariablesOverview";
        var overviewType = AssemblyUtility.GetType("Pangoo.MetaTable.VariablesOverview");
        Debug.Log($"overviewType:{overviewType}");
        // var so = ScriptableObject.CreateInstance(className);
        var so = ScriptableObject.CreateInstance<VariablesOverview>();

        AssetDatabase.CreateAsset(so, "Assets/GameMain/StreamRes/MetaTable/ScriptableObject/VariablesOverview.asset");
        AssetDatabase.Refresh();
        Debug.Log($"so:{so}");


    }
}
