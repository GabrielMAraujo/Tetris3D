using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script handles camera specific changes by resolution
[ExecuteInEditMode]
public class ResolutionHandler : MonoBehaviour
{
    public CameraData cameraData;

    private Camera cameraRef;

    private void Awake()
    {
        cameraRef = GetComponent<Camera>();
        ChangeCameraProperties();
    }

    private void Update()
    {
        ////Changes in realtime only in editor, for development purposes
        //#if UNITY_EDITOR
        //        ChangeCameraProperties();
        //#endif
    }

    private void ChangeCameraProperties()
    {
        //True - landscape / False - portrait
        bool isLandscape;

        isLandscape = FindScreenOrientation();

        if (isLandscape)
        {
            //4:3
            if (cameraRef.aspect <= 1.4f)
            {
                cameraRef.transform.position = cameraData.AspectRatio4_3CameraPosition;
            }
            //16:9
            else
            {
                cameraRef.transform.position = cameraData.AspectRatio16_9CameraPosition;
            }

        }
        else
        {
            //3:4
            if (cameraRef.aspect >= 0.7f)
            {
                cameraRef.transform.position = cameraData.AspectRatio3_4CameraPosition;
            }

            //9:16 and thinner aspect ratios
            else
            {
                cameraRef.transform.position = cameraData.AspectRatio9_16CameraPosition;
            }
        }
    }

    //Finds screen orientation by current aspect ratio
    private bool FindScreenOrientation()
    {
        //Consider square resolutions as landscape for game configs
        if(cameraRef.aspect >= 1)
        {
            return true;
        }

        return false;
    }

}
