using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
    private bool isFirstUpdate = true;

    // public float delay = 2;
    // float timer;

    private void Update()
    {   

        // timer += Time.deltaTime;
        // if (timer > delay)
        //     {

                if (isFirstUpdate)
                {
                    isFirstUpdate = false;

                    
                        
                        Loader.LoaderCallback();


                        // timer = 5;


                }
        
            // }

    }
}
