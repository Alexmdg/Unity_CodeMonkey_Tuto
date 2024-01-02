using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuUI : MonoBehaviour
{
    public static OptionsMenuUI Instance { get; private set; }

    [SerializeField] private Button soundEffectButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button MoveUpButton;
    [SerializeField] private Button MoveDownButton;
    [SerializeField] private Button MoveLeftButton;
    [SerializeField] private Button MoveRightButton;
    [SerializeField] private Button InteractButton;
    [SerializeField] private Button InteractAltButton;
    [SerializeField] private Button PauseButton;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI soundEffectText;
    [SerializeField] private TextMeshProUGUI MoveUpText;
    [SerializeField] private TextMeshProUGUI MoveDownText;
    [SerializeField] private TextMeshProUGUI MoveLeftText;
    [SerializeField] private TextMeshProUGUI MoveRightText;
    [SerializeField] private TextMeshProUGUI InteractText;
    [SerializeField] private TextMeshProUGUI InteractAltText;
    [SerializeField] private TextMeshProUGUI PauseText;
    [SerializeField] private Transform PressToRebindScreenTransform;


    private void Awake()
    {
        Instance = this;
        soundEffectButton.onClick.AddListener(() => 
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        musicButton.onClick.AddListener(() => 
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });
        Hide();
        MoveUpButton.onClick.AddListener(() => { RebindBinding(GameInputs.Bindings.MoveUp); });
        MoveDownButton.onClick.AddListener(() => { RebindBinding(GameInputs.Bindings.MoveDown); });
        MoveLeftButton.onClick.AddListener(() => { RebindBinding(GameInputs.Bindings.MoveLeft); });
        MoveRightButton.onClick.AddListener(() => { RebindBinding(GameInputs.Bindings.MoveRight); });
        InteractButton.onClick.AddListener(() => { RebindBinding(GameInputs.Bindings.Interact); });
        InteractAltButton.onClick.AddListener(() => { RebindBinding(GameInputs.Bindings.InteractAlt); });
        PauseButton.onClick.AddListener(() => { RebindBinding(GameInputs.Bindings.Pause); });
    }

    private void Start()
    {
        GameManager.Instance.OnGameUnpaused += OnGameUnpaused_CloseOptionsMenu;
        UpdateVisual();
        HidePressToRebind();
    }

    private void UpdateVisual()
    {
        soundEffectText.text = "SOUND EFFECTS :" + Mathf.Round(SoundManager.Instance.Volume * 10f);
        musicText.text = "MUSIC :" + Mathf.Round(MusicManager.Instance.Volume * 10f);
        MoveUpText.text = GameInputs.Instance.GetBindingText(GameInputs.Bindings.MoveUp);
        MoveDownText.text = GameInputs.Instance.GetBindingText(GameInputs.Bindings.MoveDown);
        MoveLeftText.text = GameInputs.Instance.GetBindingText(GameInputs.Bindings.MoveLeft);
        MoveRightText.text = GameInputs.Instance.GetBindingText(GameInputs.Bindings.MoveRight);
        InteractText.text = GameInputs.Instance.GetBindingText(GameInputs.Bindings.Interact);
        InteractAltText.text = GameInputs.Instance.GetBindingText(GameInputs.Bindings.InteractAlt);
        PauseText.text = GameInputs.Instance.GetBindingText(GameInputs.Bindings.Pause);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnGameUnpaused_CloseOptionsMenu(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void ShowPressToRebind()
    {
        PressToRebindScreenTransform.gameObject.SetActive(true);
    }

    private void HidePressToRebind()
    {
        PressToRebindScreenTransform.gameObject.SetActive(false);
    }

    private void RebindBinding(GameInputs.Bindings binding)
    {
        ShowPressToRebind();
        GameInputs.Instance.Rebind(binding, () =>
        {
            HidePressToRebind();
            UpdateVisual();
        });
    }
}
