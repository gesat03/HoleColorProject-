using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System;
using DG.Tweening.Core;
using System.Collections;

public class SecondHoleMovement : MonoBehaviour
{

    [Header("Hole mesh")]
    [SerializeField] MeshFilter meshFilter;
    [SerializeField] MeshCollider meshCollider;

    [Header("Hole vertices radius")]
    [SerializeField] Vector2 moveLimits;
    [SerializeField] float radius;
    [SerializeField] Transform holeCenter;
    [SerializeField] Transform rotatingCircle;

    [SerializeField] GameObject CircuitGameObject;

    [Space]
    [SerializeField] float moveSpeed = 0;

    Mesh mesh;
    List<int> holeVertices;
    List<Vector3> offsets;
    int holeVerticesCount;

    float x, y;
    Vector3 touch, targetPos;


    private void Start()
    {
        DataStorage.isMoving = false;
        DataStorage.isGameover = false;

        RotateCircleAnimation();

        holeVertices = new List<int>();
        offsets = new List<Vector3>();

        mesh = meshFilter.mesh;

        FindHoleVertices();

        holeCenter.localPosition = new Vector3(0f, 0.07f, -2.35f);

        UpdateHoleVerticesPosition(true);

    }

    bool initiateSecondPhase = false;

    private void Update()
    {
        if(DataStorage.secondPhase && !initiateSecondPhase)
        {
            initiateSecondPhase = true;
        }

#if UNITY_EDITOR

        DataStorage.isMoving = Input.GetMouseButton(0);

        if (!DataStorage.isGameover && DataStorage.isMoving && DataStorage.secondPhase)
        {
            MoveHole();

            UpdateHoleVerticesPosition(false);
        }

#else

        DataStorage.isMoving = Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved;

        if (!DataStorage.isGameover && DataStorage.isMoving && DataStorage.secondPhase)
        {
            MoveHole();

            UpdateHoleVerticesPosition(false);
        }

#endif

        if (!stopMove)
        {
            UpdateHoleVerticesPosition(false);
        }

    }

    void RotateCircleAnimation()
    {
        if (rotatingCircle != null)
        {
            rotatingCircle
                .DORotate(new Vector3(90f, 0, 90f), 0.2f)
                .SetEase(Ease.Linear)
                .From(new Vector3(90f, 0f, 0f))
                .SetLoops(-1, LoopType.Incremental);
        }
    }

    public void InitiateSecondPhase()
    {
        StartCoroutine(DeactivateTallBar());
        //TallBarHoleMovement.Instance.tallBarTransform.DOMoveY()
    }

    IEnumerator DeactivateTallBar()
    {
        yield return new WaitForSeconds(8.6f);

        LevelManager.Instance.CountObjects(false);

        UpdateHoleVerticesPosition(false);
        TallBarHoleMovement.Instance.DeactivateTallBar();
        CircuitGameObject.SetActive(true);

        yield return new WaitForSeconds(0.4f);
        LevelManager.Instance.AddRigidbodyToObjects(LevelManager.Instance.secondPhaseTotalObj, LevelManager.Instance.secondPhaseTotalObstacle);
    }

    private void UpdateHoleVerticesPosition(bool noHole)
    {
        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < holeVerticesCount; i++)
        {
            if (!noHole)
            {
                vertices[holeVertices[i]] = holeCenter.localPosition + offsets[i];
            }
            else
            {
                vertices[holeVertices[i]] = Vector3.zero;
            }
        }

        mesh.vertices = vertices;
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
    }

    bool stopMove = true;

    private void MoveHole()
    {
        x = Input.GetAxis("Mouse X");
        y = Input.GetAxis("Mouse Y");

        touch = Vector3.Lerp(holeCenter.localPosition, holeCenter.localPosition + new Vector3(x, 0f, y), moveSpeed * Time.deltaTime);

        targetPos = new Vector3(Mathf.Clamp(touch.x, -moveLimits.x, moveLimits.x),
            touch.y,
            Mathf.Clamp(touch.z, -moveLimits.y, moveLimits.y)
            );

        holeCenter.localPosition = targetPos;
    }

    private void FindHoleVertices()
    {
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            float distance = Vector3.Distance(holeCenter.localPosition, mesh.vertices[i]);
            if (distance < radius)
            {
                holeVertices.Add(i);
                offsets.Add(mesh.vertices[i] - holeCenter.localPosition);
            }
        }
        holeVerticesCount = holeVertices.Count;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(holeCenter.position, radius);
    }


}