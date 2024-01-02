using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGO;
    [SerializeField] private Image barImage;

    private IHasProgress task;


    private void Start()
    {
        task = hasProgressGO.GetComponent<IHasProgress>();
        if (task == null)
        {
            Debug.LogError("GameObject does not implement the required IHasProgress Interface...");
        }
        task.OnProgressChanged += UpdateProgressBar;
        barImage.fillAmount = 0f;
        Hide();
    }

    private void UpdateProgressBar(object sender, IHasProgress.OnProgressChangedEventArgs e )
    {
        if (e.progressNormalized == 0f || e.progressNormalized == 1f)
        {
            Hide();
        }
        else
        {
            Show();
        }

        barImage.fillAmount = e.progressNormalized;
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive (false);
    }
}
