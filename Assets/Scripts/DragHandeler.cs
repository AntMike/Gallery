using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Gallery.Cube;

namespace Gallery.ImageControl
{
    public class DragHandeler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        //start drag position
        private Vector3 startPosition;
        //image parent
        [HideInInspector]
        public Transform parent;
        //mainCamera
        [HideInInspector]
        public Camera cam;


        /// <summary>
        /// Set to UI imge loaded from internet
        /// </summary>
        /// <param name="_url">Image url</param>
        public IEnumerator SetInternetImage(string _url)
        {
            using (WWW www = new WWW(_url))
            {

                yield return www;
                //Debug.Log(text = www.);
                parent.GetComponent<RawImage>().texture = www.texture;
            }
        }

        /// <summary>
        /// Set to UI image loaded from local storage
        /// </summary>
        /// <param name="_texture">Image texture</param>
        public void SetOflineImage(Texture _texture)
        {
            parent.GetComponent<RawImage>().texture = _texture;
        }

        #region IBeginDragHandler implementation
        //start Image drag
        public void OnBeginDrag(PointerEventData eventData)
        {
            startPosition = parent.localPosition;
        }

        #endregion

        #region IDragHandler implementation
        //while draging
        public void OnDrag(PointerEventData eventData)
        {
            parent.position = eventData.position;
            Debug.DrawRay(cam.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, Color.green);
        }

        #endregion

        #region IEndDragHandler implementation
        //when drag is ends
        public void OnEndDrag(PointerEventData eventData)
        {
            parent.localPosition = startPosition;
            CollisionDetection();
        }

        /// <summary>
        /// detect if image was draget to the cube
        /// </summary>
        private void CollisionDetection()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Debug.DrawRay(cam.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, Color.green);
            if (Physics.Raycast(ray, out hit, 1000))
            {
                if (hit.collider.tag == "cube")
                {
                    hit.collider.gameObject.GetComponentInParent<SimplePlayerController>().ChangeTexture(parent.GetComponent<RawImage>().texture);
                }
            }
        }

        #endregion
    }
}
