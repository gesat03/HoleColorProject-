using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    public static LevelManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    [SerializeField] ParticleSystem winVFX;

    [Space]
    public int objectsInScene;
    public int totalObjects;

    public GameObject[] firstPhaseTotalObj;
    public GameObject[] secondPhaseTotalObj;

    [Space]
    public GameObject[] firstPhaseTotalObstacle;
    public GameObject[] secondPhaseTotalObstacle;

    [SerializeField] Transform objectsParent;

    private void Start()
    {
        CountObjects(true);
    }


    public void AddRigidbodyToObjects(GameObject[] GObjects, GameObject[] GObstacles)
    {
        for (int i = 0; i < GObjects.Length; i++)
        {
            GObjects[i].AddComponent<Rigidbody>();
        }

        for (int i = 0; i < GObstacles.Length; i++)
        {
            GObstacles[i].AddComponent<Rigidbody>();
        }
    }


    public void CountObjects(bool firstPhase)
    {

        if (firstPhase)
        {
            totalObjects = firstPhaseTotalObj.Length;
            objectsInScene = totalObjects;
        }
        else
        {
            totalObjects = secondPhaseTotalObj.Length;
            objectsInScene = totalObjects; 
        }
 
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PlayWinVFX()
    {
        winVFX.Play();
    }

}
