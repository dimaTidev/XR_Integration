using System.Collections;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Controller_Animation : MonoBehaviour
{
    Animator m_animator;
    Animator Animator => m_animator ??= GetComponent<Animator>();

    [SerializeField] string animGrab = "HandGrab";
    [SerializeField] string animPointer = "HandPointerGrab";
    [SerializeField, Range(0f,1f)] float minimalValue = 0.24f;

    private void Start()
    {
        Animator.speed = 0;
        Animator.Play(animGrab, -1, minimalValue);
    }

   // For test
   // private void Update()
   // {
   //     Set_Grab(minimalValue);
   //     Set_Trigger(minimalValue);
   // }

    public void Set_Grip(float value) => Animator.Play(animGrab, -1, Mathf.Max(minimalValue, value));
    
    public void Set_Trigger(float value) => Animator.Play(animPointer, -1, Mathf.Max(minimalValue, value));
}