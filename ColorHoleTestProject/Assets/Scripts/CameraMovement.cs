using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }


    private void Start()
    {
        this.gameObject.transform.position = new Vector3(0, 6, -4);
    }

    public void MoveCamera()
    {
        this.gameObject.transform.DOMoveZ(9, 5f).SetEase(Ease.Linear);
    }

}
