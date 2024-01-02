using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipRefSO audioClipRef_SO;

    public float Volume { get; private set; }
    private const string PLAYER_PREF_SOUND_VOLUME = "SoundEffectsVolume";

    private void Awake()
    {
        Volume = PlayerPrefs.GetFloat(PLAYER_PREF_SOUND_VOLUME, 1f);
        Instance = this;
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += OnRecipeSuccess_PlaySound;
        DeliveryManager.Instance.OnRecipeFail += OnRecipeFail_PlaySound;
        CuttingCounter.OnAnyCut += OnAnyCut_PlaySound;
        Player.Instance.OnPickUp += OnPickUp_PlaySound;
        BaseCounter.OnItemDrop += OnItemDrop_PlaySound;
        TrashContainer.OnItemTrashed += OnItemTrashed_PlaySound;
    }

    private void OnRecipeSuccess_PlaySound(object sender, System.EventArgs e)
    {
        DeliveryCounter counter = DeliveryCounter.Instance;
        PlaySound(audioClipRef_SO.deliverySuccess, counter.transform.position);
    }

    private void OnRecipeFail_PlaySound(object sender, System.EventArgs e)
    {
        DeliveryCounter counter = DeliveryCounter.Instance;
        PlaySound(audioClipRef_SO.deliveryFail, counter.transform.position);
    }

    private void OnAnyCut_PlaySound(object sender, System.EventArgs e)
    {
        CuttingCounter counter = sender as CuttingCounter;
        PlaySound(audioClipRef_SO.chop, counter.transform.position);
    }

    private void OnPickUp_PlaySound(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRef_SO.objectPickup, Player.Instance.transform.position);
    }

    private void OnItemDrop_PlaySound(object sender, System.EventArgs e)
    {
        BaseCounter counter = sender as BaseCounter;
        PlaySound(audioClipRef_SO.objectDrop, counter.transform.position);
    }

    private void OnItemTrashed_PlaySound(object sender, System.EventArgs e)
    {
        TrashContainer trash = sender as TrashContainer;
        PlaySound(audioClipRef_SO.trsh, trash.transform.position);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * Volume);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    }

    public void PlayFootstepSound(Vector3 position, float volume)
    {
        PlaySound(audioClipRef_SO.footstep, position, volume);
    }

    public void ChangeVolume()
    {
        Volume += 0.1f;
        if (Volume > 1f)
        {
            Volume = 0f;
        }

        PlayerPrefs.SetFloat(PLAYER_PREF_SOUND_VOLUME, Volume);
        PlayerPrefs.Save();
    }
}
