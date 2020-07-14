using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Magnet : MonoBehaviour
{

    public static Magnet Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    [SerializeField] float magnetForce;

    List<Rigidbody> affectedRigidbodies = new List<Rigidbody>();
    Transform magnet;

    // Start is called before the first frame update
    void Start()
    {
        magnet = transform;
        affectedRigidbodies.Clear();
    }

    private void FixedUpdate()
    {
        if(!DataStorage.isGameover && DataStorage.isMoving)
        {
            foreach (Rigidbody rb in affectedRigidbodies)
            {
                rb.AddForce((magnet.position - rb.position) * magnetForce * Time.fixedDeltaTime);
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        string tag = other.tag;

        if(!DataStorage.isGameover && (tag.Equals("Object")) || (tag.Equals("Obstacle")))
        {
            other.gameObject.AddComponent<Rigidbody>();
            AddToMagneticField(other.attachedRigidbody);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        string tag = other.tag;

        if (!DataStorage.isGameover && (tag.Equals("Object")) || (tag.Equals("Obstacle")))
        {
            RemoveFromMagneticField(other.attachedRigidbody);
        }
    }

    public void AddToMagneticField(Rigidbody rb)
    {
        affectedRigidbodies.Add(rb);
    }

    public void RemoveFromMagneticField(Rigidbody rb)
    {
        affectedRigidbodies.Remove(rb);
    }

}
