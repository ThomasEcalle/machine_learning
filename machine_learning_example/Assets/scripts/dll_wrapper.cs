using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class dll_wrapper : MonoBehaviour
{

    [DllImport("lib")]
    public extern static int jose();

    public Transform[] spheres;

    // Start is called before the first frame update
    void Start()
    {
        // Create_Model



        // Train_linear_classif
        var nb_of_sphere_of_surface = 0;
        var nb_of_know_element = 0;
        var up = 0;
        var down = 0;

        foreach (var sphere in spheres)
        {
            if(sphere.position.y == 0.0){
                nb_of_sphere_of_surface += 1;

            } else if (sphere.position.y == 1.0) {
                up += 1;
            } else {
                down += 1;
            }
            nb_of_know_element = up + down;
        }

        Debug.Log("Surface -> " + nb_of_sphere_of_surface);
        Debug.Log("Resultat connu -> " + nb_of_know_element);
        Debug.Log("Up y = 1 -> " + up);
        Debug.Log("Down y = -1 -> " + down);
        //Inference
        /*
        foreach (var sphere in spheres)
        {
            var res = 1; // Inférence (sphere.position);
            sphere.position = new Vector3(sphere.position.x, res, sphere.position.z);
        }*/









        /*

        Debug.Log(jose());

        Debug.Log(sum(new double[] { 1, 2, 3, 4, 5 }, 5));

        var managedArray = new double[10];
        var rawArray = create_array(10);

        Marshal.Copy(rawArray, managedArray, 0, 10);

        delete_array(rawArray);
        foreach (var elt in managedArray)
        {
            Debug.Log(elt);
        }*/


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
