using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvacuationHandler : MonoBehaviour
{
    private GameController _gameController;

    void Start()
    {
        _gameController = FindObjectOfType<GameController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "NPC_NORMAL")
        {
            collision.gameObject.SetActive(false);
            _gameController.SendMessage("NPCRescued");
        }
    }
}
