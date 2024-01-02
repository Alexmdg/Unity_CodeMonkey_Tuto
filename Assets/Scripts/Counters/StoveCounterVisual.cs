using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private GameObject stoveOnGamObject;
    [SerializeField] private GameObject particlesGamObject;
    [SerializeField] private StoveCounter counter;

    private void Start()
    {
        counter.OnStateChanged += StoveAnimationStateCHanged;
    }

    private void StoveAnimationStateCHanged(object sender, StoveCounter.OnStateCHangedEventArgs e)
    {
        switch (e.state) 
        {
            case StoveCounter.State.Idle:
                Hide();
                break;
            case StoveCounter.State.Frying:
                Show();
                break;
        }
    }

    private void Show()
    {
        stoveOnGamObject.SetActive(true);
        particlesGamObject.SetActive(true);
    }

    private void Hide()
    {
        stoveOnGamObject.SetActive(false);
        particlesGamObject.SetActive(false);
    }
}
