using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragHandeler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 startPosition;
    [HideInInspector]
    public Transform parent;
    [HideInInspector]
    public Camera cam;

    public IEnumerator SetInternetImage(string _url)
    {
        using (WWW www = new WWW(_url))
        {

            yield return www;
            //Debug.Log(text = www.);
            parent.GetComponent<RawImage>().texture = www.texture;
        }
    }

    public void SetOflineImage(Texture _texture)
    {
        parent.GetComponent<RawImage>().texture = _texture;
    }

    #region IBeginDragHandler implementation

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = parent.localPosition;
    }

    #endregion

    #region IDragHandler implementation

    public void OnDrag(PointerEventData eventData)
    {
        parent.position = eventData.position;
        Debug.DrawRay(cam.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, Color.green);
    }

    #endregion

    #region IEndDragHandler implementation

    public void OnEndDrag(PointerEventData eventData)
    {
        parent.localPosition = startPosition;
        CollisionDetection();
    }

    private void CollisionDetection()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Debug.DrawRay(cam.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, Color.green);
        if (Physics.Raycast(ray, out hit, 1000))
        {
            if (hit.collider.tag == "cube")
            {
                hit.collider.gameObject.GetComponent<Cube>().ChangeTexture(parent.GetComponent<RawImage>().texture);
            }
        }
    }

    #endregion



}
