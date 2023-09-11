using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPoint : GameManager
{
    private Vector3 spawn;

    private void Awake()
    {
        spawn = transform.position;
    }
    public override void Oninit()
    {
        base.Oninit();
        Debug.Log("vi tri start: " + spawn);
    }
}
