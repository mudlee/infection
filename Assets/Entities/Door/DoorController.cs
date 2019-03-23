using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private GameObject[] _openedDoors = { };
    [SerializeField] private GameObject[] _closedDoors = { };
    private bool _open = true;
    private BoxCollider2D _collider;
    private GameObject _SAP2D;

    private void Start()
    {
        Close(true);
        _SAP2D = GameObject.FindWithTag("SAP2D");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetType() == typeof(BoxCollider2D))
            Open();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetType() == typeof(BoxCollider2D))
            Close();
    }

    private void Close(bool dontCalculateAStarColliders = false)
    {
        HandleDoorVisual(false, true, dontCalculateAStarColliders);
        _open = false;
    }

    private void Open()
    {
        HandleDoorVisual(true, false, false);
        _open = true;
    }

    private void HandleDoorVisual(bool openedDoor, bool closedDoor, bool dontCalculateAStarColliders)
    {
        foreach (GameObject door in _openedDoors)
        {
            door.SetActive(openedDoor);
        }

        foreach (GameObject door in _closedDoors)
        {
            door.SetActive(closedDoor);
        }

        if (!dontCalculateAStarColliders)
        {
            _SAP2D?.SendMessage("CalculateColliders");
        }
    }
}
