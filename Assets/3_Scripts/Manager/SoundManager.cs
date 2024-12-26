using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipRefsSO audioClipRefsSO;

    private float musicVolume;
    private float sfxVolume;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        VolumeSettings.Instance.OnMusicVolumeChanged += VolumeSettings_OnMusicVolumeChanged;
        VolumeSettings.Instance.OnSFXVolumeChanged += VolumeSettings_OnSFXVolumeChanged;
    }

    private void VolumeSettings_OnMusicVolumeChanged( object sender, System.EventArgs e ) {
        musicVolume = VolumeSettings.Instance.GetMusicVolume();
    }

    private void VolumeSettings_OnSFXVolumeChanged( object sender, System.EventArgs e ) {
        sfxVolume = VolumeSettings.Instance.GetSFXVolume();
    }

    public void PlayPunchSound() {
        PlaySound(audioClipRefsSO.punch, Player.Instance.transform.position, sfxVolume);
    }

    public void PlayKickSound() {
        PlaySound(audioClipRefsSO.kick, Player.Instance.transform.position, sfxVolume);
    }

    public void PlayRoundSound(int currentRound) {
        PlaySound(audioClipRefsSO.round[currentRound], Camera.main.transform.position, musicVolume);
    }

    private void PlaySound( AudioClip audioClip, Vector3 position, float volume) {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }

    private void PlaySound( AudioClip[] audioClipArray, Vector3 position, float volume) {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    }
}
