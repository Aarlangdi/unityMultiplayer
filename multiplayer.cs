using System.Collections;
using CMF;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

[System.Serializable]
public class jsonFormat
{
    public int ID;
    public string username;
    public string active;
    public float xPOS;
    public float yPOS;
    public float zPOS;
    
    public float xROT;
    public float yROT;
    public float zROT;
}

[System.Serializable]
public class jsonFormatArray
{
    public jsonFormat[] users;
}


class spawnedUsers
{
    public int ID;
    public GameObject GO;
    
    public spawnedUsers(int ID_new, GameObject GO_new)
    {
        ID = ID_new;
        GO = GO_new;
    }
}

public class multiplayer : MonoBehaviour
{
    public GameObject mainUser;
    private int userID;
    
    private float xPOS_L;
    private float yPOS_L;
    private float zPOS_L;
    
    private float xROT_L;
    private float yROT_L;
    private float zROT_L;

    List<spawnedUsers> _spawnedUsers;

    private int userID_session;

    void Awake()
    {
        userID_session = int.Parse(PlayerPrefs.GetString("userID_session"));
    }

    // Start is called before the first frame update
    void Start()
    {
        _spawnedUsers = new List<spawnedUsers>();
        
        StartCoroutine(GetRequest("https://tutorial.aarlangdi.com"));
        InvokeRepeating("updateUserInfo", 1.0f, .3f);
    }

    void updateUserInfo()
    {
        if (_spawnedUsers.Count > 0)
        {
            foreach (var _spawnedUser  in _spawnedUsers)
            {
                if (_spawnedUser.ID != userID_session)
                {
                    _spawnedUser.GO.GetComponent<AdvancedWalkerController>().enabled = false;
                    _spawnedUser.GO.transform.GetChild(1).gameObject.SetActive(false);
                }
                else
                {
                    float xP = _spawnedUser.GO.transform.position.x;
                    float yP = _spawnedUser.GO.transform.position.y;
                    float zP = _spawnedUser.GO.transform.position.z;
                    string url = "https://tutorial.aarlangdi.com";
                    
                    var rot = _spawnedUser.GO.transform.Find("ModelRoot").localRotation.eulerAngles;

                    StartCoroutine(setUserInfo(url, xP, yP, zP, _spawnedUser.ID, rot.x, rot.y, rot.z));
                }
            }   
        }
    }

    IEnumerator setUserInfo(string url, float xPOS, float yPOS, float zPOS, int userID, float xROT, float yROT, float zROT)
    {
        WWWForm form = new WWWForm();
        form.AddField("xPOS", xPOS.ToString());
        form.AddField("yPOS", yPOS.ToString());
        form.AddField("zPOS", zPOS.ToString());
        form.AddField("userID", userID.ToString());
        
        form.AddField("xROT", xROT.ToString());
        form.AddField("yROT", yROT.ToString());
        form.AddField("zROT", zROT.ToString());

        UnityWebRequest uwr = UnityWebRequest.Post(url, form);
        yield return uwr.SendWebRequest();
        
        string retVal = uwr.downloadHandler.text;
        jsonFormatArray root = JsonUtility.FromJson<jsonFormatArray>(retVal);

        for (int i = 0; i < root.users.Length; i++)
        {
            xPOS_L = root.users[i].xPOS;
            yPOS_L = root.users[i].yPOS;
            zPOS_L = root.users[i].zPOS;
                
            xROT_L = root.users[i].xROT;
            yROT_L = root.users[i].yROT;
            zROT_L = root.users[i].zROT;
            
            userID = root.users[i].ID;

            Vector3 pos = new Vector3(xPOS_L, yPOS_L, zPOS_L);
            Quaternion rot = Quaternion.Euler(xROT_L, yROT_L, zROT_L);

            foreach (var _spawnedUser in _spawnedUsers)
            {
                if (_spawnedUser.ID == userID)
                {
                    _spawnedUser.GO.transform.position = pos;

                    _spawnedUser.GO.transform.Find("ModelRoot").rotation = rot;
                }
            }
        }
    }

    IEnumerator GetRequest(string uri)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(uri);
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            string retVal = uwr.downloadHandler.text;
            jsonFormatArray root = JsonUtility.FromJson<jsonFormatArray>(retVal);

            for (int i = 0; i < root.users.Length; i++)
            {
                xPOS_L = root.users[i].xPOS;
                yPOS_L = root.users[i].yPOS;
                zPOS_L = root.users[i].zPOS;
                
                xROT_L = root.users[i].xROT;
                yROT_L = root.users[i].yROT;
                zROT_L = root.users[i].zROT;
                
                userID = root.users[i].ID;
                
                Vector3 pos = new Vector3(xPOS_L, yPOS_L, zPOS_L);
                Quaternion rot = Quaternion.Euler(xROT_L, yROT_L, zROT_L);
                GameObject inGO = Instantiate(mainUser, pos, Quaternion.identity);

                inGO.transform.Find("ModelRoot").rotation = rot;
                
                _spawnedUsers.Add(new spawnedUsers(userID, inGO));
            }
        }
        
    }
}
