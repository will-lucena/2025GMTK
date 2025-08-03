using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitApplication : MonoBehaviour
{
    private void Start()
    {
        SceneManager.Instance.LoadMenu();
    }
}
