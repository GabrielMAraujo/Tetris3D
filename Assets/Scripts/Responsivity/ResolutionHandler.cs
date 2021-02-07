using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script handles camera specific changes by resolution
[ExecuteInEditMode]
public class ResolutionHandler : MonoBehaviour
{
    public CameraData cameraData;

    private Camera camera;

    private void Awake()
    {
        camera = GetComponent<Camera>();
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
            //camera.rect = cameraData.landscapeViewportRect;
            //4:3
            if (camera.aspect <= 1.4f)
            {
                //camera.rect = cameraData.AspectRatio3_4ViewportRect;
                camera.transform.position = cameraData.AspectRatio4_3CameraPosition;
            }
            //16:9
            else
            {
                camera.transform.position = cameraData.AspectRatio16_9CameraPosition;
            }

        }
        else
        {
            //Check for specific portrait aspect ratios, intervals included for uncommon aspect ratios

            //3:4
            if (camera.aspect >= 0.7f)
            {
                //camera.rect = cameraData.AspectRatio3_4ViewportRect;
                camera.transform.position = cameraData.AspectRatio3_4CameraPosition;
            }

            //9:16 and thinner aspect ratios
            else
            {
                //camera.rect = cameraData.AspectRatio9_16ViewportRect;
                camera.transform.position = cameraData.AspectRatio9_16CameraPosition;
            }
        }
    }

    //Finds screen orientation by current aspect ratio
    private bool FindScreenOrientation()
    {
        //Consider square resolutions as landscape for game configs
        if(camera.aspect >= 1)
        {
            return true;
        }

        return false;
    }

}
