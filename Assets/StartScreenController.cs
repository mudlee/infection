using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenController : MonoBehaviour
{
    private SoundPlayer _soundPlayer;

    void Start()
    {
        _soundPlayer = FindObjectOfType<SoundPlayer>();
        _soundPlayer?.Play(SoundType.BACKGROUND_MUSIC);
        _soundPlayer?.Play(SoundType.BACKGROUND_MUSIC2);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }
}
