using FairyGUI;
using System.Collections.Generic;
using UnityEngine;

public class UIRuntimeController : MonoBehaviour
{
    Dictionary<string, List<string>> researchTree;
    public List<GameObject> panels;
    Dictionary<string, GameObject> dic_panels;

    GComponent mainView;

    // Start is called before the first frame update
    void Start()
    {
        researchTree = GameObject.FindGameObjectWithTag("DataPool").GetComponent<DataPool>().dic_researchTree;

        dic_panels = new Dictionary<string, GameObject>();
        foreach (GameObject g in panels)
        {
            dic_panels[g.name] = g;
        }

        //Research

        mainView = dic_panels["Panel_Research"].GetComponent<UIPanel>().ui;

        GTree list = mainView.GetChild("list").asTree;
        GTreeNode rootNode = list.rootNode;

        foreach (string key in researchTree.Keys)
        {
            GComponent list_item0 = list.GetFromPool("ui://5eyvfdebvxu0o").asCom;

            //rootNode.AddChild(list_item0);

            list_item0.GetChild("text").asTextField.text = key;
            list.AddChild(list_item0);
            foreach (string it in researchTree[key])
            {
                GComponent list_item1 = list.GetFromPool("ui://5eyvfdeblfbcl").asCom;
                list_item1.GetChild("text").asTextField.text = it;
                list.AddChild(list_item1);

            }
        }



    }

    // Update is called once per frame
    void Update()
    {

    }


}
