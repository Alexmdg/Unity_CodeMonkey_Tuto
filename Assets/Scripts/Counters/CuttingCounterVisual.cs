using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterVISUal : MonoBehaviour
{
    private const string CUT = "Cut";
    [SerializeField] private CuttingCounter counter;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        counter.OnCut += OnCutAnimation;
    }

    private void OnCutAnimation(object sender, System.EventArgs e)
    {
        animator.SetTrigger(CUT);
    }
}
