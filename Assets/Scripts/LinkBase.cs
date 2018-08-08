using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkBase : MonoBehaviour {

    public static LinkBase Instance;
    public List<string> links;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

}
