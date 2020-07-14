using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System;
using DG.Tweening.Core;
using System.Collections;

public class TallBarHoleMovement : MonoBehaviour
{

    public static TallBarHoleMovement Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    [Header("Hole mesh")]
    [SerializeField] MeshFilter meshFilter;
    [SerializeField] MeshCollider meshCollider;

    [SerializeField] GameObject circuitGameObject;

    public Transform tallBarTransform;

    [Header("Hole vertices radius")]
    [SerializeField] Vector2 moveLimits;
    [SerializeField] float radius;
    [SerializeField] Transform holeCenter;
    [SerializeField] Transform rotatingCircle;

    Mesh mesh;
    List<int> holeVertices;
    List<Vector3> offsets;
    int holeVerticesCount;

    bool stopMove = true;

    private void Start()
    {
        DataStorage.isMoving = false;
        DataStorage.isGameover = false;

        RotateCircleAnimation();

        holeVertices = new List<int>();
        offsets = new List<Vector3>();

        mesh = meshFilter.mesh;

        FindHoleVertices();

        holeCenter.localPosition = new Vector3(0f, 0.077f, -3.532f);

        tallBarTransform.position = new Vector3(0f, -0.04f, 6.81f);

        UpdateHoleVerticesPosition(true);
    }

    private void Update()
    {
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

    public void DeactivateTallBar()
    {
        tallBarTransform.position = new Vector3(0f, -0.04f, 5.88f);
        tallBarTransform.eulerAngles = new Vector3(0, 180, 0);
        UpdateHoleVerticesPosition(true);
    }

    public void SetTallBarAction()
    {
        tallBarTransform.DOMoveZ(5.9f, 0.001f)
            .SetDelay(2.75f)
            .OnComplete(() => UpdateHoleVerticesPosition(false));

        Invoke("AutoTallBarHoleMove", 4f);

        StartCoroutine(ActivateCircuits());
    }

    IEnumerator ActivateCircuits()
    {
        yield return new WaitForSeconds(2.75f);
        circuitGameObject.SetActive(true);
    }

    [ContextMenu("MoveHole")]
    public void AutoTallBarHoleMove()
    {
        stopMove = false;
        DOTween.To(() =>
            holeCenter.localPosition,
            x => holeCenter.localPosition = x,
            new Vector3(0f, 0.077f, 4.85f), 5f)
            .From(new Vector3(0f, 0f, -3.532f))
            .SetEase(Ease.Linear)
            .OnComplete(() => stopMove = true)
            .OnComplete(() => DataStorage.secondPhase = true);

       //tallBarTransform.DOMoveZ(5.95f, 4f);

        CameraMovement.Instance.MoveCamera();
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