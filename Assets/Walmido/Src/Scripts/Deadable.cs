using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Deadable : MonoBehaviour
{
    public UnityEvent tellDead = new UnityEvent();

    //[SerializeField]
    //Stage stage;

    [SerializeField]
    GameObject DeadEffectPrefab;

    bool isAlive = true;
    public bool IsAlive
    {
        get => isAlive;
        private set
        {
            isAlive = value;
            if (!isAlive)
                tellDead.Invoke();
        }
    }

    void Awake()
    {
        if (DeadEffectPrefab == null)
            throw new System.Exception("Deadable doesnt have DeadEffect");
    }

    //void OnEnable()
    //{
    //    if (stage != null)
    //    {
    //        tellDead.AddListener(stage.Failed);
    //    }
    //}

    //private void OnDisable()
    //{
    //    if (stage != null)
    //    {
    //        tellDead.RemoveListener(stage.Failed);
    //    }
    //}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Deadable"))
        {
            IsAlive = false;
            this.gameObject.SetActive(false);
            Instantiate(DeadEffectPrefab, transform.position, transform.rotation);
        }
    }
}
