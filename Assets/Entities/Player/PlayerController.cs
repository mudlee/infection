using System;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    private static float SPEED = 1f;
    private static float MOUSE_CURSOR_THRESHOLD = 0.05f;
    private Vector2 TEMP_velocity = new Vector2();
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private bool _moving;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleMove();
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

        if (System.Math.Abs(horizontal) > Mathf.Epsilon || System.Math.Abs(vertical) > Mathf.Epsilon)
        {
            if (!_moving)
            {
                _moving = true;
                /*if (_soundPlayer != null)
                {
                    _stepSoundAudioSource = _soundPlayer.Play(Sound.PLAYER_STEP);....
                }*/
            }
        }
        else if (_moving)
        {
            _moving = false;
            //_soundPlayer?.Stop(_stepSoundAudioSource);
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
