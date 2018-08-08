using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cube : MonoBehaviour {

    public Slider positionSlider;

    public Transform _transform;

    public Button RollLeftBtn;
    public Button RollRightBtn;

    private float _nextRollPoint = 0;
    private Vector3 _nextRoll;
    private CubeState _state = CubeState.Front;

    private bool _isRoll = false;
    private bool _isLeftRoll = false;


    public void ChangeTexture(Texture _text)
    {
        int _rotation = (int)_transform.rotation.eulerAngles.y;
        switch (_rotation)
        {
            case 0:
                gameObject.GetComponent<MeshRenderer>().materials[2].mainTexture = _text;
                break;
            case 90:
                gameObject.GetComponent<MeshRenderer>().materials[1].mainTexture = _text;
                break;
            case 180:
                gameObject.GetComponent<MeshRenderer>().materials[0].mainTexture = _text;
                break;
            case 270:
                gameObject.GetComponent<MeshRenderer>().materials[3].mainTexture = _text;
                break;
        }
    }

    private void Update()
    {
        //if(_isRoll)
        //{
        //    if(_isLeftRoll)
        //    {
        //        if(_transform.eulerAngles.y == 0)
        //        {
        //            _transform.eulerAngles = new Vector3(0, 360, 0);
        //            _state = CubeState.Front360;
        //            _isRoll = false;
        //        }
        //    } else
        //    {
        //        if (_transform.eulerAngles.y == 360)
        //        {
        //            _transform.eulerAngles = new Vector3(0, 0, 0);
        //            _isRoll = false;
        //        }
        //    }
        //}
        //if (_transform.eulerAngles.y != _nextRollPoint )
        //{
        //    if (_isRoll)
        //        _isRoll = false;
        //    Debug.Log(_nextRollPoint);
        //    if (Mathf.Abs(_transform.eulerAngles.y - _nextRollPoint) > 0.2f )
        //    {
        //        _transform.eulerAngles = Vector3.Lerp(_transform.eulerAngles, _nextRoll,5 );
        //    }
        //    else
        //    {
        //        _transform.eulerAngles = _nextRoll;
        //    }
        //} else if(RollRightBtn.interactable == false || RollLeftBtn.interactable == false)
        //{
        //    RollLeftBtn.interactable = true;
        //    RollRightBtn.interactable = true;
        //}
    }


    IEnumerator Roll()
    {
        while (_transform.eulerAngles.y != _nextRollPoint)
        {

            if (_transform.eulerAngles.y != _nextRollPoint)
            {
                if (_isRoll)
                    _isRoll = false;
                Debug.Log(_nextRollPoint);
                if (Mathf.Abs(_transform.eulerAngles.y - _nextRollPoint) > 2f)
                {
                    _transform.eulerAngles = Vector3.Lerp(_transform.eulerAngles, _nextRoll, Time.deltaTime *5);
                }
                else
                {
                    _transform.eulerAngles = _nextRoll;
                }
            }
            yield return new WaitForFixedUpdate();
        }
        RollLeftBtn.interactable = true;
        RollRightBtn.interactable = true;
    }

    public void RollLeft()
    {
        RollLeftBtn.interactable = false;
        RollRightBtn.interactable = false;

        _isRoll = true;
        _isLeftRoll = true;

        if (_state == CubeState.Front || _state == CubeState.Front360)
        {
            _state = CubeState.Left;
        }
        else if (_state == CubeState.Left)
        {
            _state = CubeState.Back;
        }
        else if (_state == CubeState.Back)
        {
            _state = CubeState.Right;
        }
        else if (_state == CubeState.Right)
        {
            _state = CubeState.Front;
        }

        _nextRollPoint = (float)_state;
        _nextRoll = new Vector3(0, _nextRollPoint, 0);
        StartCoroutine(Roll());
    }

    public void RollRight()
    {
        RollLeftBtn.interactable = false;
        RollRightBtn.interactable = false;

        _isRoll = true;
        _isLeftRoll = false;

        if (_state == CubeState.Front || _state == CubeState.Front360)
        {
            _state = CubeState.Right;
        }
        else if (_state == CubeState.Right)
        {
            _state = CubeState.Back;
        }
        else if (_state == CubeState.Back)
        {
            _state = CubeState.Left;
        }
        else if (_state == CubeState.Left)
        {
            _state = CubeState.Front;
        }

        _nextRollPoint = (float)_state;

        _nextRoll = new Vector3(0, _nextRollPoint, 0);
        StartCoroutine(Roll());
    }

    void Rolling(int _side)
    {

    }

    public void Moving()
    {
        _transform.position = new Vector3(0, 0, positionSlider.value);
    }

    public enum CubeState
    {
        Front = 0,
        Right = 90,
        Back = 180,
        Left = 270,
        Front360 = 360
    }


}
