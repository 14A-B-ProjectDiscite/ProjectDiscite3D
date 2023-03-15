using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using System;

[Serializable]
public class Configuration
{
    public Weapon[] weapons;
    public Enemy[] enemies;
    public Class[] classes;
    public Artifact[] artifacts;
}
[Serializable]
public class Weapon
{
    public int id;
    public string name;
    public float damage;
    public float attackSpeed;
}
[Serializable]
public class Enemy
{
    public int id;
    public string name;
    public float maxHp;
    public float damage;
    public float energy;
    public float speed;
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
    public int id;
    public string name;
    public int maxLevel;
}
