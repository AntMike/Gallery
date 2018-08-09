using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using UnityEngine.UI;

public class LoadFiles : MonoBehaviour {


    public Transform parent;
    public Camera cam;
    public GameObject pref;
    public List<Texture> oflineList;

    private void Start () {
        string HtmlText = GetHtmlFromUri("http://google.com");
        if (HtmlText == "")
        {
            CreateImage(false);
        }
        else
        {
            CreateImage(true);
        }
    }

    private void CreateImage(bool _hasInernetConnection)
    {
        parent.GetComponent<GridLayoutGroup>().enabled = true;


        if (_hasInernetConnection)
        {
            foreach (string _link in LinkBase.Instance.links)
            {

                parent.GetComponent<RectTransform>().sizeDelta = new Vector3(1, ((LinkBase.Instance.links.Count-1)*400)+100);
                var go = Instantiate(pref, parent);
                go.GetComponentInChildren<DragHandeler>().cam = cam;
                go.GetComponentInChildren<DragHandeler>().parent = go.transform;
                go.GetComponentInChildren<DragHandeler>().StartCoroutine(go.GetComponentInChildren<DragHandeler>().SetInternetImage(_link));
            }
        }
        else
        {
            foreach (Texture _texture in oflineList)
            {
                parent.GetComponent<RectTransform>().sizeDelta = new Vector3(1, ((oflineList.Count-1)*400) + 100);
                var go = Instantiate(pref, parent);
                go.GetComponentInChildren<DragHandeler>().cam = cam;
                go.GetComponentInChildren<DragHandeler>().SetOflineImage(_texture);
            }
        }
        
    }

    private IEnumerator StopGroup()
    {
        yield return new WaitForSeconds(0.5f);
        parent.GetComponent<GridLayoutGroup>().enabled = false;
    }
    


 public string GetHtmlFromUri(string resource)
{
    string html = string.Empty;
    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(resource);
    try
    {
        using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
        {
            bool isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;
            if (isSuccess)
            {
                using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
                {
                    char[] cs = new char[80];
                    reader.Read(cs, 0, cs.Length);
                    foreach (char ch in cs)
                    {
                        html += ch;
                    }
                }
            }
        }
    }
    catch
    {
        return "";
    }
    return html;
}
}
