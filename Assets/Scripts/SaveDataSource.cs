using PolyAndCode.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class SaveDataSource : MonoBehaviour, IRecyclableScrollRectDataSource
{
    [SerializeField] StringVariable token;
    [SerializeField] IntVariable userId;
    [SerializeField]
    RecyclableScrollRect _recyclableScrollRect;
    [SerializeField]
    private int _dataLength;
    //Dummy data List
    private List<Save> _contactList = new List<Save>();
    //Recyclable scroll rect's data source must be assigned in Awake.
    private void Awake()
    {
        //InitData();
        _recyclableScrollRect.DataSource = this;
    }

    public void Load()
    {
        StartCoroutine(GetRequest("https://nagyilles.jedlik.cloud/api/api/Users/" + userId.Value));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            webRequest.SetRequestHeader("Authorization", $"Bearer {token.Value}");
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    Debug.LogError($"Bearer {token.Value}");
                    Debug.LogError("https://nagyilles.jedlik.cloud/api/api/Users/" + userId.Value);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                   // Save conf = JsonUtility.FromJson<Save>(webRequest.downloadHandler.text);
                   // Debug.Log(JsonUtility.ToJson(conf));
                    break;
            }
        }
    }

    #region DATA-SOURCE
    /// <summary>
    /// Data source method. return the list length.
    /// </summary>
    public int GetItemCount()
    {
        return _contactList.Count;
    }
    /// <summary>
    /// Called for a cell every time it is recycled
    /// Implement this method to do the necessary cell configuration.
    /// </summary>
    public void SetCell(ICell cell, int index)
    {
        //Casting to the implemented Cell
        var item = cell as SaveCell;
        item.ConfigureCell(_contactList[index], index);
    }
    #endregion
}



public class Save
{
    public int Id;
    public int UserId;
    public int ClassId;
    public string Path;
    public int Gold;
    public int Score;
    public int Runtime;
    public RunStatus Status;
    public string GameVersion;
    public DateTime StartDate;
    public DateTime EndDate;
    public float CurrentHp;
    public int Seed;

    public ICollection<RunArtifactModel> Artifacts;
    public ICollection<RunRoomModel> Rooms;
    public ICollection<RunWeaponModel> Weapons;
    public ICollection<RunEnemyModel> Enemies;
}
public enum RunStatus
{
    Dead = 0,
    Alive = 1,
    Finished = 2
}
public class RunArtifactModel
{
    public int Id;

    public int RunId;
    //public RunModel Run;

    public int ArtifactId;
    //public ArtifactModel Artifact;

    public int Picked;
    public int Seen;
    public int Used;
}
public class RunEnemyModel
{
    public int Id;

    public int RunId;
    //public RunModel Run;

    public int EnemyId;
    //public EnemyModel Enemy;

    public int Deaths;
    public int Seen;
    public int Damage;
}

public class RunWeaponModel
{
    public int Id;

    public int RunId;
    //public RunModel Run;

    public int WeaponId;
    //public WeaponModel Weapon;

    public int Picked;
    public int Seen;
}

public class RunRoomModel
{
    public int Id;

    public int RunId;
    //public RunModel Run;

    public int RoomId;
    //public RoomModel Room;

    public int Seen;
}
