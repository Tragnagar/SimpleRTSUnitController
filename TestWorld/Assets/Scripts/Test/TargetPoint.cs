using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс отвечает за поведение целевой точки при задевании триггера её кораблем.
/// </summary>
public class TargetPoint : MonoBehaviour {

    public void OnTriggerEnter(Collider coll)
    {
        GameObject collGO = coll.gameObject;

        Debug.Log(collGO.name + " entered trigger");
    }
}
