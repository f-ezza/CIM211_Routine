using System;
using UnityEngine;

public class EnterObjective : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private BoxCollider2D boxCollider;

    //Actions
    public Action OnObjectiveEnter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnObjectiveEnter?.Invoke();
        }
    }
}
