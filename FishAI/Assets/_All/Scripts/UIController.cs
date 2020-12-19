using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    // Start is called before the first frame update
    void Awake()
    {
        if(!instance) instance = this;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
