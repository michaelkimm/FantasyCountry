using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public enum Type
    {
        DragonSD_03,
        DragonSD_06,
        DragonSD_08,
        DragonSD_14,
        DragonSD_21,
        DragonSD_22,
        DragonSD_28,
        Size
    }

    [SerializeField]
    Type nPCType = Type.Size;
    public Type NPCType 
    { 
        get => nPCType;
    }
    
    public void Sit()
    {
        Destroy(this.gameObject);
    }
}
