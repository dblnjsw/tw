using FairyGUI;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class UIRuntimeController : MonoBehaviour
{
    Dictionary<string, List<string>> researchTree;
    public List<GameObject> panels;
    Dictionary<string, GameObject> dic_panels;

    GComponent mainView, mainView_login;

    //login
    string login_username, login_password;
    string login_url = "http://127.0.0.1:8083/login2";

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

        //mainView = dic_panels["Research"].GetComponent<UIPanel>().ui;

        //GTree list = mainView.GetChild("list").asTree;
        //GTreeNode rootNode = list.rootNode;

        //foreach (string key in researchTree.Keys)
        //{
        //    GComponent list_item0 = list.GetFromPool("ui://5eyvfdebvxu0o").asCom;

        //    //rootNode.AddChild(list_item0);

        //    list_item0.GetChild("text").asTextField.text = key;
        //    list.AddChild(list_item0);
        //    foreach (string it in researchTree[key])
        //    {
        //        GComponent list_item1 = list.GetFromPool("ui://5eyvfdeblfbcl").asCom;
        //        list_item1.GetChild("text").asTextField.text = it;
        //        list.AddChild(list_item1);

        //    }
        //}

        //Login
        mainView_login = dic_panels["Login"].GetComponent<UIPanel>().ui;

        GObject btn_cancel = mainView_login.GetChild("cancel");
        btn_cancel.onClick.Add(() =>
        {
            mainView_login.visible = false;
        });

        GObject btn_login = mainView_login.GetChild("btn_login");
        btn_login.onClick.Add(onClickLogin);


    }

    IEnumerator UnityWebRequestPost(string url, byte[] postBytes)
    {
        UnityWebRequest request = new UnityWebRequest(url, "POST");

        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(postBytes);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
        }
    }
    void onClickLogin()
    {
        var task = requestLogin();
        Debug.Log("start coroutine");
    }
    private async Task requestLogin()
    {
        login_username = mainView_login.GetChild("input_username").text;
        login_password = mainView_login.GetChild("input_password").text;

        JsonData data = new JsonData();
        data["username"] = login_username;
        data["password"] = login_password;
        byte[] postBytes = System.Text.Encoding.Default.GetBytes(data.ToJson());
        //UnityWebRequestPost(, postBytes);
        UnityWebRequest www = UnityWebRequest.Get(login_url);
        UnityWebRequestAsyncOperation request = www.SendWebRequest();

        while (!request.isDone)
            await Task.Delay(100);
        


        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
        Debug.Log("username:" + login_username);
    }

    // Update is called once per frame
    void Update()
    {

    }


}
