using JoystickControls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SimplePlayerController : MonoBehaviour {
    
    public float speed;
    public Camera cam;
    private Vector2 movementVector;
    private float xSidePoint = 10;
    private float upPoint = 2f;
    private float downPoint = -0.65f;
    private Vector3 screenPoint;

    public Button RollLeftBtn;
    public Button RollRightBtn;
    private Transform _transform;



    private List<Quaternion> positions = new List<Quaternion>() { new Quaternion(0, 0, 0, 1), new Quaternion(0f, 0.7f, 0f, 0.7f), new Quaternion(0, 1, 0, 0), new Quaternion(0, 0.7f, 0, -0.7f) };
    private int _currentState = 0;
    private Quaternion _nextRollPoint;
    private Vector3 _nextRoll;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }



    private void FixedUpdate()
	{

        screenPoint = cam.WorldToViewportPoint(transform.position);
        movementVector = new Vector2(JoystickInputManager.GetAxis("Horizontal"), JoystickInputManager.GetAxis("Vertical"));

        if (movementVector.x >= 0.5f && screenPoint.x < 1)
        {
            Movement(new Vector3(xSidePoint, 0, transform.position.z), Time.deltaTime * speed / 2);
        }
        else if (movementVector.x <= -0.5f && screenPoint.x > 0)
        {
            Movement(new Vector3(-xSidePoint, 0, transform.position.z), Time.deltaTime * speed / 2);
        }
        if (movementVector.y >= 0.5f && screenPoint.y < 1)
        {
            Movement(new Vector3(transform.position.x, 0, upPoint), Time.deltaTime * speed);
        }
        else if (movementVector.y <= -0.5f && screenPoint.y > 0 && screenPoint.x < 1 && screenPoint.x > 0)
        {
            Movement(new Vector3(transform.position.x, 0, downPoint), Time.deltaTime * speed);
        }
    }

    private void Movement(Vector3 _newPoint, float _speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, _newPoint, _speed);
    }

    private IEnumerator Roll()
    {
        float _localTime = 0.35f;
        while (_localTime > 0)
        {
            _localTime -= Time.deltaTime;
            _transform.rotation = Quaternion.Lerp(_transform.rotation, _nextRollPoint, 0.15f);
            yield return new WaitForFixedUpdate();
        }
        _transform.rotation = _nextRollPoint;
        RollLeftBtn.interactable = true;
        RollRightBtn.interactable = true;
    }

    public void RollLeft()
    {
        RollLeftBtn.interactable = false;
        RollRightBtn.interactable = false;

        _currentState--;
        if (_currentState < 0)
            _currentState = 3;

        _nextRollPoint = positions[_currentState];

        StartCoroutine(Roll());
    }

    public void RollRight()
    {
        RollLeftBtn.interactable = false;
        RollRightBtn.interactable = false;

        _currentState++;
        if (_currentState > 3)
            _currentState = 0;

        _nextRollPoint = positions[_currentState];

        StartCoroutine(Roll());
    }
}

