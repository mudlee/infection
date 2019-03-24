using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] Text _infectedCnt;
    [SerializeField] Text _normalCnt;
    [SerializeField] Text _rescuedCnt;
    private const int requiredRescued = 3;
    private SoundPlayer _soundPlayer;
    private int rescued;
    private int infected;
    private int normal;

    void Start()
    {
        _soundPlayer = FindObjectOfType<SoundPlayer>();
        RandomScream();
    }

    void RandomScream()
    {
        _soundPlayer?.Play(SoundType.RANDOM_SCREAM);
        Invoke("RandomScream", Random.Range(30,120));
    }

    public void NPCInfectedSpawned()
    {
        _infectedCnt.text = (++infected).ToString();
}

    public void NPCNormalSpawned()
    {
        _normalCnt.text = (++normal).ToString();
    }

    public void NPCInfected()
    {
        _normalCnt.text = (--normal).ToString();

        if ((normal + rescued) < requiredRescued)
        {
            SceneManager.LoadScene("NoMatter");
        }
    }

    public void NPCRescued()
    {
        _normalCnt.text = (--normal).ToString();
        _rescuedCnt.text = "Rescued: " + (++rescued) + "/"+ requiredRescued;

        if(rescued>=requiredRescued)
            SceneManager.LoadScene("Won");
    }
}
