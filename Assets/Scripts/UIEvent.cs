using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Doozy.Engine.UI;

public class UIEvent : MonoBehaviour
{
    // Start is called before the first frame update

    private void Start()
    {
    }
    public void ChangeScene(string sceneName)
    {        
        SceneManager.LoadScene(sceneName);
        UIPopup pop = UIPopup.GetPopup("die");
        pop.Hide();
    }

    public void Show_pop_ecs()
    {
        if (!UIPopup.AnyPopupVisible)
        {
            UIPopup.GetPopup("ecs").Show();
        }
    }


}
