using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Login : MonoBehaviour
{
    //[SerializeField] TextMeshProUGUI email;
    [SerializeField] InputField email;
    [SerializeField] InputField password;

   public void LoginButtonClicked()
    {
        StartCoroutine(TryLogin(email.text, password.text));
        // asd@fgh.jkl
        // asdfghjkl
    }

    public IEnumerator TryLogin(string email, string password)
    {
        //@TODO: call API login
        // Store Token
        // Add Token to headers

        var user = new UserData();
        user.email = email;
        user.password = password;

        string json = JsonUtility.ToJson(user);

        var req = new UnityWebRequest("https://nagyilles.jedlik.cloud/api/api/users/login", "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return req.SendWebRequest();

        if (req.isNetworkError)
        {
            Debug.Log("Error While Sending: " + req.error);
        }
        else
        {
            Debug.Log("Received: " + req.downloadHandler.text);
        }

    }
}
[Serializable]
public class UserData
{
    public string email;
    public string password;
}
