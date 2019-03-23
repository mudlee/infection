using UnityEngine;
using SAP2D;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class NPCController : MonoBehaviour
{
    [SerializeField] private bool _infected = true;
    [SerializeField] private GameObject _infectedPrefab;
    private const float FOLLOW_PLAYER_DISTANCE_NORMAL = 1f;
    private const float AWARANESS_RADIUS_NORMAL = 3f;
    private const float AWARANESS_RADIUS_INFECTED = 5f;
    private const float SPEED = 1f;
    private List<GameObject> _npcs = new List<GameObject>();

    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private GameObject _player;
    private SAP2DAgent _agent;

    private enum Direction { LEFT, RIGHT, TOP, BOTTOM, DONT_MOVE };
    // MOVEMENT
    private Direction _currentDirection;
    private int _remainingMovementTime = 0;
    private Vector2 _velocity = new Vector2();
    private Direction? _collidedDirection;

    // TARGET
    private int _currentTarget;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _player = GameObject.FindWithTag("Player");
        _currentTarget = _player.GetHashCode();
        _agent = GetComponent<SAP2DAgent>();
    }

    private void Start()
    {
        _agent.Target = _player.transform;
        _agent.CanMove = false;

        _npcs.Add(_player);
        foreach(GameObject npc in GameObject.FindGameObjectsWithTag("NPC_NORMAL"))
        {
            _npcs.Add(npc);
        }
    }

    private void FixedUpdate()
    {
        HandleMove();
    }

    private void HandleMove()
    {
        if (_infected)
            HandleInfectedMove();
        else
            HandleNormalMove();
    }

    private void HandleNormalMove()
    {
        float distance = Vector3.Distance(transform.position, _player.transform.position);
        if (distance <= AWARANESS_RADIUS_NORMAL)
        {
            if(distance< FOLLOW_PLAYER_DISTANCE_NORMAL)
                StopAndResetAgent();
            else
                MoveViaAgent(_player);
        }
        else
        {
            Wondering();
        }
    }

    private void Wondering()
    {
        if (_agent.CanMove)
            StopAndResetAgent();

        // wondering
        if (_remainingMovementTime == 0)
        {
            UpdateDirection((Direction)Random.Range(0, System.Enum.GetValues(typeof(Direction)).Length));
        }
        else
        {
            _remainingMovementTime--;
        }
    }

    private void HandleInfectedMove()
    {
        float possibleTargetDistance = 0;
        GameObject possibleTarget = null;

        foreach (GameObject npc in _npcs)
        {
            if (!npc.activeSelf)
            {
                continue;
            }
            float distance = Vector3.Distance(transform.position, npc.transform.position);
            if(distance<=AWARANESS_RADIUS_INFECTED)
            {
                if(possibleTarget==null || distance < possibleTargetDistance)
                {
                    possibleTarget = npc;
                    possibleTargetDistance = distance;
                }
            }
        }

        if(possibleTarget == null) // no target available in this frame, let's just cruise
        {
            Wondering();
        }
        else
        {
            if (possibleTarget.GetHashCode() != _currentTarget)
            {
                _currentTarget = possibleTarget.GetHashCode();
                StopAndResetAgent();
            }

            MoveViaAgent(possibleTarget);
        }
    }

    private void MoveViaAgent(GameObject target)
    {
        if (!_agent.CanMove)
        {
            // first stop it, if it was already moving
            _velocity.Set(0, 0);
            _rigidbody.velocity = _velocity;

            _agent.path = null;
            _agent.Target = target.transform;
            _agent.CanMove = true;
            _agent.CanSearch = true;
        }

        _animator.SetBool("WalkingRight", false);
        _animator.SetBool("WalkingLeft", false);
        _animator.SetBool("WalkingUp", false);
        _animator.SetBool("WalkingDown", false);

        if (_agent.direction.x > 0 && _agent.direction.y > 0)
        {
            if (_agent.direction.x > _agent.direction.y)
                _animator.SetBool("WalkingRight", true);
            else
                _animator.SetBool("WalkingUp", true);
        }
        else if (_agent.direction.x < 0 && _agent.direction.y < 0)
        {
            if (_agent.direction.x < _agent.direction.y)
                _animator.SetBool("WalkingLeft", true);
            else
                _animator.SetBool("WalkingDown", true);
        }
        else if (_agent.direction.x < 0 && _agent.direction.y > 0)
        {
            if (Mathf.Abs(_agent.direction.x) > _agent.direction.y)
                _animator.SetBool("WalkingLeft", true);
            else
                _animator.SetBool("WalkingUp", true);
        }
        else if (_agent.direction.x > 0 && _agent.direction.y < 0)
        {
            if (Mathf.Abs(_agent.direction.y) < _agent.direction.x)
                _animator.SetBool("WalkingRight", true);
            else
                _animator.SetBool("WalkingDown", true);
        }
    }

    private void StopAndResetAgent()
    {
        _agent.CanMove = false;
        _agent.Target = null;
        _agent.CanSearch = false;
        _animator.SetBool("WalkingRight", false);
        _animator.SetBool("WalkingLeft", false);
        _animator.SetBool("WalkingUp", false);
        _animator.SetBool("WalkingDown", false);
        _velocity.Set(0, 0);
        _rigidbody.velocity = _velocity; 
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        UnityEditor.Handles.color = _infected ? Color.red : Color.blue;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, _infected ? AWARANESS_RADIUS_INFECTED : AWARANESS_RADIUS_NORMAL);
#endif
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!_infected)
        {
            if (collision.gameObject.tag == "NPC")
            {
                GameObject infectedNPC = Instantiate(_infectedPrefab) as GameObject;
                infectedNPC.transform.position = transform.position;
                gameObject.SetActive(false);
                return;
            }
        }

        _collidedDirection = _currentDirection;
        UpdateDirection(Direction.DONT_MOVE);
    }

    private void UpdateDirection(Direction direction)
    {
        if (direction != _collidedDirection)
        {
            _remainingMovementTime = direction == Direction.DONT_MOVE? (int)Random.Range(50f, 200f) : (int)Random.Range(200f, 500f);

            _currentDirection = direction;

            UpdateAnimProps();
            UpdateVelocity();
        }
    }

    private void UpdateAnimProps()
    {
        _animator.SetBool("WalkingRight", _currentDirection == Direction.RIGHT);
        _animator.SetBool("WalkingLeft", _currentDirection == Direction.LEFT);
        _animator.SetBool("WalkingUp", _currentDirection == Direction.TOP);
        _animator.SetBool("WalkingDown", _currentDirection == Direction.BOTTOM);
    }

    private void UpdateVelocity()
    {
        switch (_currentDirection)
        {
            case Direction.BOTTOM:
                _velocity.Set(0, -SPEED);
                break;
            case Direction.TOP:
                _velocity.Set(0, SPEED);
                break;
            case Direction.LEFT:
                _velocity.Set(-SPEED, 0);
                break;
            case Direction.RIGHT:
                _velocity.Set(SPEED, 0);
                break;
            case Direction.DONT_MOVE:
                _velocity.Set(0, 0);
                break;
        }

        _rigidbody.velocity = _velocity;
    }
}
