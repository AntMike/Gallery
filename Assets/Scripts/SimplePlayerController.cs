using JoystickControls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gallery.Cube
{
    public class SimplePlayerController : MonoBehaviour
    {

        public float cubeMovingSpeed;
        public Camera mainCamera;

        //cube parent transform 
        private Transform _transform;

        //joystick movement
        private Vector2 movementVector;
        //min and max cube moving points
        private float xSidePoint = 10;
        private float upPoint = 2f;
        private float downPoint = -0.65f;
        //cube location to camera field of view
        private Vector3 screenPoint;

        [Header("Rolling buttons")]
        public Button RollLeftBtn;
        public Button RollRightBtn;


        //rotation sides
        //0 - front
        //1 - right
        //2 - back
        //3 - left
        private List<Quaternion> positions = new List<Quaternion>() { new Quaternion(0, 0, 0, 1), new Quaternion(0f, 0.7f, 0f, 0.7f), new Quaternion(0, 1, 0, 0), new Quaternion(0, 0.7f, 0, -0.7f) };
        private int _currentState = 0;
        private Quaternion _nextRollPoint;

        //renderer of the cube
        private Renderer mRenderer;

        //set transform and mesh renderer to variables
        private void Awake()
        {
            _transform = GetComponent<Transform>();
            mRenderer = GetComponentInChildren<MeshRenderer>();
        }


        //calculate joystick movement 
        private void FixedUpdate()
        {
            //get cube position inside cam field of view
            screenPoint = mainCamera.WorldToViewportPoint(transform.position);
            //return joystick axis
            movementVector = new Vector2(JoystickInputManager.GetAxis("Horizontal"), JoystickInputManager.GetAxis("Vertical"));

            if (movementVector.x >= 0.5f && screenPoint.x < 1)
            {
                Movement(new Vector3(xSidePoint, 0, transform.position.z), Time.deltaTime * (cubeMovingSpeed + Mathf.Abs(transform.position.z)) / 2.5f);
            }
            else if (movementVector.x <= -0.5f && screenPoint.x > 0)
            {
                Movement(new Vector3(-xSidePoint, 0, transform.position.z), Time.deltaTime * (cubeMovingSpeed + Mathf.Abs(transform.position.z)) / 2.5f);
            }
            if (movementVector.y >= 0.5f && screenPoint.y < 1)
            {
                Movement(new Vector3(transform.position.x, 0, upPoint), Time.deltaTime * cubeMovingSpeed);
            }
            else if (movementVector.y <= -0.5f && screenPoint.y > 0 && screenPoint.x < 1 && screenPoint.x > 0)
            {
                Movement(new Vector3(transform.position.x, 0, downPoint), Time.deltaTime * cubeMovingSpeed);
            }
        }

        /// <summary>
        /// Take texture and set it to the cube side
        /// </summary>
        /// <param name="_texture">Texture that'll set on cube</param>
        public void ChangeTexture(Texture _texture)
        {
            //set texture to side
            switch (_currentState)
            {
                case 0:
                    mRenderer.materials[2].mainTexture = _texture;
                    break;
                case 1:
                    mRenderer.materials[1].mainTexture = _texture;
                    break;
                case 2:
                    mRenderer.materials[0].mainTexture = _texture;
                    break;
                case 3:
                    mRenderer.materials[3].mainTexture = _texture;
                    break;
            }
        }

        /// <summary>
        /// Moves cube
        /// </summary>
        /// <param name="_newPoint">Finish cube position</param>
        /// <param name="_speed">Speed of moving</param>
        private void Movement(Vector3 _newPoint, float _speed)
        {
            transform.position = Vector3.MoveTowards(transform.position, _newPoint, _speed);
        }

        /// <summary>
        /// Roll cube
        /// </summary>
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

        /// <summary>
        /// Turn cube to the left side
        /// </summary>
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
        /// <summary>
        /// Turn cube to the right side
        /// </summary>
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
}

