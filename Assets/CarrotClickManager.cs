using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotClickManager : MonoBehaviour

   {
    private Camera cam;
    private carrotpuller activePuller;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                carrotpuller puller = hit.collider.GetComponent<carrotpuller>();
                if (puller != null)
                {
                    activePuller = puller;
                    activePuller.BeginDrag();
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && activePuller != null)
        {
            activePuller.EndDrag();
            activePuller = null;
        }
    }
}
