using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustColliderChild : MonoBehaviour
{
    public CustCollider parent;

    void OnTriggerEnter(Collider other)
    {
        parent.NotifyEnterTrigger(other);
    }

    void OnTriggerExit(Collider other)
    {
        parent.NotifyExitTrigger(other);
    }
}
