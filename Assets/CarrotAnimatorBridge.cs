using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotAnimatorBridge : MonoBehaviour

{
    public GameObject pullableCarrotPrefab;
    public carrotgrower grower;

    // This method is called by the animation event
    public void OnCarrotAnimationFinished()
    {
        GameObject newCarrot = Instantiate(pullableCarrotPrefab, transform.position, transform.rotation);
        newCarrot.transform.localScale = transform.localScale;

        carrotpuller puller = newCarrot.GetComponent<carrotpuller>();
        if (puller != null)
        {
            puller.grower = grower;
        }

        Destroy(gameObject);
    }
}
