using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ridable : MonoBehaviour
{
    [SerializeField]
    bool parentIsRidable = false;
    public bool ParentIsRidable { get => parentIsRidable; }

}
