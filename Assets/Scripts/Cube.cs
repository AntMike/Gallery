using UnityEngine;
using UnityEngine.UI;

public class Cube : MonoBehaviour {
    
    private Transform _transform;
    private Renderer mRenderer;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        mRenderer = GetComponentInChildren<MeshRenderer>();
    }



    public void ChangeTexture(Texture _text)
    {
        int _rotation = (int)_transform.rotation.eulerAngles.y;
        switch (_rotation)
        {
            case 0:
                mRenderer.materials[2].mainTexture = _text;
                break;
            case 90:
                mRenderer.materials[1].mainTexture = _text;
                break;
            case 180:
                mRenderer.materials[0].mainTexture = _text;
                break;
            case 270:
                mRenderer.materials[3].mainTexture = _text;
                break;
        }
    }



}
