using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using UnityEngine;

public class PostRun : MonoBehaviour
{
    [SerializeField] StringVariable token;
    [SerializeField] IntVariable runId;
    [SerializeField] IntVariable userId;
    [SerializeField] FloatVariable score;
    [SerializeField] IntVariable WaveNumber;
    // Start is called before the first frame update
    void Start()
    {
        //Configuration run = new Configuration();
        using (var client = new HttpClient())
        {

            var endpoint = new Uri("https://discite.jedlik.cloud/api/api/run");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
            var payload = new StringContent("", Encoding.UTF8, "application/json");
            var result = client.PostAsync(endpoint, payload).Result.Content.ReadAsStringAsync().Result;
            Debug.Log(result);
            Configuration data = JsonUtility.FromJson<Configuration>(result);
            runId.Value = data.id;
            /*
            TokenData data = JsonUtility.FromJson<TokenData>(result);
            userID.Value = int.Parse(data.id);
            username.Value = data.username;
            token.Value = data.token;
            SceneManager.LoadScene(1);*/
        }
        //run.weapons.Add();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            EndRun();
        }
    }

    public void EndRun()
    {
        Debug.Log("Endrun Called");
        using (var client = new HttpClient())
        {
            Configuration config = new Configuration();
            config.id = runId.Value;
            config.userId = userId.Value;
            config.score = Mathf.FloorToInt(score.Value);
            config.endDate = DateTime.Now;
            config.wave = WaveNumber.Value;
            var newPostJson = JsonUtility.ToJson(config);

            Debug.Log("PostData: " + newPostJson);
            //string s = "{\r\n  \"id\": 12,\r\n  \"userId\": 5,\r\n  \"score\": 50532,\r\n  \"startDate\": \"2023-05-05T11:03:27.028Z\",\r\n  \"endDate\": \"2023-05-05T11:03:27.028Z\",\r\n  \"wave\": 4,\r\n  \"artifacts\": [\r\n    {\r\n      \"artifactId\": 0,\r\n      \"picked\": 0\r\n    }\r\n  ],\r\n  \"weapons\": [\r\n    {\r\n      \"weaponId\": 1,\r\n      \"picked\": 1\r\n    },\r\n    {\r\n      \"weaponId\": 2,\r\n      \"picked\": 1\r\n    },\r\n    {\r\n      \"weaponId\": 3,\r\n      \"picked\": 1\r\n    },\r\n    {\r\n      \"weaponId\": 4,\r\n      \"picked\": 1\r\n    }\r\n  ],\r\n  \"enemies\": [\r\n    {\r\n      \"enemyId\": 1,\r\n      \"deaths\": 0,\r\n      \"seen\": 0,\r\n      \"damage\": 0\r\n    },\r\n    {\r\n      \"enemyId\": 2,\r\n      \"deaths\": 0,\r\n      \"seen\": 0,\r\n      \"damage\": 0\r\n    },\r\n    {\r\n      \"enemyId\": 3,\r\n      \"deaths\": 0,\r\n      \"seen\": 0,\r\n      \"damage\": 0\r\n    },\r\n    {\r\n      \"enemyId\": 4,\r\n      \"deaths\": 0,\r\n      \"seen\": 0,\r\n      \"damage\": 0\r\n    }\r\n    \r\n  ]\r\n}";
            var endpoint = new Uri("https://discite.jedlik.cloud/api/api/run");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
            var payload = new StringContent(newPostJson, Encoding.UTF8, "application/json");
            var result = client.PutAsync(endpoint, payload).Result.Content.ReadAsStringAsync().Result;
            Debug.Log(result);
            //client.DefaultRequestHeaders.Accept
            /*
            TokenData data = JsonUtility.FromJson<TokenData>(result);
            userID.Value = int.Parse(data.id);
            username.Value = data.username;
            token.Value = data.token;
            SceneManager.LoadScene(1);*/
        }
    }
    /*{
  "id": 0,
  "userId": 0,
  "score": 0,
  "startDate": "2023-05-03T12:21:41.105Z",
  "endDate": "2023-05-03T12:21:41.105Z",
  "wave": 0,
  "artifacts": [
    {
      "artifactId": 0,
      "picked": 0
    }
  ],
  "weapons": [
    {
      "weaponId": 0,
      "picked": 0
    }
  ],
  "enemies": [
    {
      "enemyId": 0,
      "deaths": 0,
      "seen": 0,
      "damage": 0
    }
  ]
}*/


}

