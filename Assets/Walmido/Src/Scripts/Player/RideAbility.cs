using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RideAbility : MonoBehaviour
{
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            throw new System.Exception("RideAbilty doesn't have Rigidbody");
    }

    void OnTriggerEnter(Collider other)
    {
        Ridable ridable = other.GetComponent<Ridable>();
        if (ridable != null)
        {
            if (ridable.ParentIsRidable)
                this.gameObject.transform.SetParent(ridable.gameObject.transform.parent);
            else
                this.gameObject.transform.SetParent(ridable.gameObject.transform);
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    void OnTriggerExit(Collider other)
    {
        Ridable ridable = other.GetComponent<Ridable>();

        if (ridable != null)
        {
            this.gameObject.transform.SetParent(null);
            rb.constraints = RigidbodyConstraints.None;
        }
    }

    void OnEnable()
    {
    }

    void OnDisable()
    {
    }
}
