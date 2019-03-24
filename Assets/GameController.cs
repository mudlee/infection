using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] Text _infectedCnt;
    [SerializeField] Text _normalCnt;
    [SerializeField] Text _rescuedCnt;
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
    }

    public void NPCRescued()
    {
        _normalCnt.text = (--normal).ToString();
        _rescuedCnt.text = "Rescued: " + (++rescued) + "/3";
    }
}
