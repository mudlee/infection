using UnityEngine;

public class GameController : MonoBehaviour
{
    private SoundPlayer _soundPlayer;

    void Start()
    {
        _soundPlayer = FindObjectOfType<SoundPlayer>();
        _soundPlayer?.Play(SoundType.BACKGROUND_MUSIC);
        _soundPlayer?.Play(SoundType.BACKGROUND_MUSIC2);
        RandomScream();
    }

    void RandomScream()
    {
        _soundPlayer?.Play(SoundType.RANDOM_SCREAM);
        Invoke("RandomScream", Random.Range(30,120));
    }
}
