using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

[System.Serializable]
class userLoginInfo
{
    public string userID;
    public string response;
}

public class login : MonoBehaviour
{
    public InputField username;
    public InputField password;
    public Text statusText;

    public void loginUser()
    {
        StartCoroutine(checkLogin());
    }
    
    IEnumerator checkLogin()
    {
        print(username.text);
        string url = "https://tutorial.aarlangdi.com/login.php";
        WWWForm form = new WWWForm();
        form.AddField("username", username.text);
        form.AddField("password", password.text);

        UnityWebRequest uwr = UnityWebRequest.Post(url, form);
        yield return uwr.SendWebRequest();
        
        string retVal = uwr.downloadHandler.text;

        userLoginInfo root = JsonUtility.FromJson<userLoginInfo>(retVal);
        
        if (root.response == "ok")
        {
            PlayerPrefs.SetString("username_session", username.text);
            PlayerPrefs.SetString("userID_session", root.userID);
            
            SceneManager.LoadScene("main", LoadSceneMode.Single);
        }
        else
        {
            statusText.text = root.response;
        }
    }
}
