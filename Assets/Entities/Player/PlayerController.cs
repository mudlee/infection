﻿using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public static float MOUSE_CURSOR_THRESHOLD = 0.09f;
    public bool _moving;

    private static float SPEED = 1.4f;
    private Vector2 TEMP_velocity = new Vector2();
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleMove();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "NPC")
        {
            SceneManager.LoadScene("Died");
        }
    }

    private void HandleMove()
    {
        Vector3 movement = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        float horizontal = ConvertMouseMovement(movement.x);
        float vertical = ConvertMouseMovement(movement.y);

        _animator.SetBool("WalkingRight", horizontal > 0);
        _animator.SetBool("WalkingLeft", horizontal < 0);
        _animator.SetBool("WalkingUp", vertical > 0);
        _animator.SetBool("WalkingDown", vertical < 0);

        TEMP_velocity.Set(horizontal * SPEED, vertical * SPEED);
        _rigidbody.velocity = TEMP_velocity;

        if (Mathf.Abs(horizontal) > Mathf.Epsilon || Mathf.Abs(vertical) > Mathf.Epsilon)
        {
            if (!_moving)
            {
                _moving = true;
            }
        }
        else if (_moving)
        {
            _moving = false;
        }
    }

    private float ConvertMouseMovement(float x)
    {
        if (x > (0.5f+ MOUSE_CURSOR_THRESHOLD))
        {
            return 1f;
        }
        else if(x < (0.5f- MOUSE_CURSOR_THRESHOLD))
        {
            return -1f;
        }

        return 0f;
    }
}
