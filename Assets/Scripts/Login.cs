using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Login : MonoBehaviour
{
    //[SerializeField] TextMeshProUGUI email;
    [SerializeField] InputField email;
    [SerializeField] InputField password;
    [SerializeField] Text errorsText;
    [SerializeField] StringVariable token;
    [SerializeField] IntVariable userID;
    [SerializeField] StringVariable username;


    public void LoginButtonClicked()
    {
        TryLogin(email.text, password.text);
        //StartCoroutine(TryLogin(email.text, password.text));
        // asd@fgh.jkl
        // asdfghjkl
    }
    public void GetConfig()
    {
        using (var client = new HttpClient())
        {

            var endpoint = new Uri("https://discite.jedlik.cloud/api/api/config");
            var result = client.GetAsync(endpoint).Result;
            var json = result.Content.ReadAsStringAsync().Result;
            Debug.Log(json);
        }
    }
    public void GuestButton()
    {
        SceneManager.LoadScene(1);
    }
    public void RegisterButton()
    {
        Application.OpenURL("https://discite.jedlik.cloud");
    }
    public void TryLogin(string email, string inputtedPassword)
    {
        // Solution Number 2
        using(var client = new HttpClient())
        {

            var endpoint = new Uri("https://discite.jedlik.cloud/api/api/user/login");

            var newPost = new UserData() { email = email, password = inputtedPassword };
            var newPostJson = JsonUtility.ToJson(newPost);
            Debug.Log(newPostJson);
            var payload = new StringContent(newPostJson, Encoding.UTF8, "application/json");
            var result = client.PostAsync(endpoint, payload).Result.Content.ReadAsStringAsync().Result;
            Debug.Log(result);
            
            TokenData data = JsonUtility.FromJson<TokenData>(result);
            if (data.id == null) { errorsText.text = "The email or password was incorrect."; }
            userID.Value = int.Parse(data.id);
            username.Value = data.username;
            token.Value = data.token;
            SceneManager.LoadScene(1);
        }

    }
}
[Serializable]
public class UserData
{
    public string email;
    public string password;
}
[Serializable]
public class TokenData
{
    public string id;
    public string username;
    public string email;
    public string token;
}
