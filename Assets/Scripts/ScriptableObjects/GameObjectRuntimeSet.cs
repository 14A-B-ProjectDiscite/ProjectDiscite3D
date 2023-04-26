using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(menuName = "Sets/GameObject")]
public class GameObjectRuntimeSet : RuntimeSet<GameObject>
{
    public void OnEnable()
    {
        Items.Clear();
    }
}
