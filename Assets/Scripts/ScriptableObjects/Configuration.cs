using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using System;

[Serializable]
public class Configuration
{
    public int id;
    public int userId;
    public int score;
    public DateTime startDate;
    public DateTime endDate;
    public int wave;
    public List<Artifact> artifacts;
    public List<Weapon> weapons;
    public List<Enemy> enemies;
}
[Serializable]
public class Weapon
{
    public int weaponId;
    public int picked;
}
[Serializable]
public class Enemy
{
    public int enemyId;
    public int deaths;
    public int seen;
    public int damage;
}
[Serializable]
public class Class
{
    public int id;
    public string name;
    public float maxHp;
    public float damage;
    public float energy;
    public float speed;
}
[Serializable]
public class Artifact
{
    public int artifactId;
    public int picked;
}

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

