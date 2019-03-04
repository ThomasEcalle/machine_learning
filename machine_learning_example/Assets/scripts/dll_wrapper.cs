using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class dll_wrapper : MonoBehaviour
{

    [DllImport("libmachine_learning")]
    public extern static int return42();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(return42());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
