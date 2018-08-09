using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using UnityEngine.UI;
using Gallery.Base;
using Gallery.ImageControl;

namespace Gallery.Game
{
    public class LoadFiles : MonoBehaviour
    {

        //content
        public Transform parent;
        //main camera
        public Camera mainCamera;
        //image prefab
        public GameObject imagePrefab;


        //check the internet connection and start load files
        private void Start()
        {
            //check the internet connection and start load files
            string HtmlText = GetHtmlFromUri("http://google.com");
            if (HtmlText == "")
            {
                //connection failed
                CreateImage(false);
            }
            else
            {
                //success
                CreateImage(true);
            }
        }

        /// <summary>
        /// Creating images due the list and set them to grid
        /// </summary>
        /// <param name="_hasInernetConnection">Is internet connection available</param>
        private void CreateImage(bool _hasInernetConnection)
        {
            parent.GetComponent<GridLayoutGroup>().enabled = true;

            if (_hasInernetConnection)
            {
                parent.GetComponent<RectTransform>().sizeDelta = new Vector3(1, ((ImageBase.Instance.links.Count - 1) * 400) + 100);
                foreach (string _link in ImageBase.Instance.links)
                {
                    GameObject go = InstantiatePrefab();
                    go.GetComponentInChildren<DragHandeler>().StartCoroutine(go.GetComponentInChildren<DragHandeler>().SetInternetImage(_link));
                }
            }
            else
            {
                parent.GetComponent<RectTransform>().sizeDelta = new Vector3(1, ((ImageBase.Instance.oflineList.Count - 1) * 400) + 100);
                foreach (Texture _texture in ImageBase.Instance.oflineList)
                {
                    GameObject go = InstantiatePrefab();
                    go.GetComponentInChildren<DragHandeler>().SetOflineImage(_texture);
                }
            }

        }

        /// <summary>
        /// Create prefab and set local variables
        /// </summary>
        /// <returns>Created image</returns>
        private GameObject InstantiatePrefab()
        {
            GameObject go = Instantiate(imagePrefab, parent);
            go.GetComponentInChildren<DragHandeler>().cam = mainCamera;
            go.GetComponentInChildren<DragHandeler>().parent = go.transform;
            return go;
        }

        /// <summary>
        /// Disable gridlayout
        /// </summary>
        private IEnumerator StopGroup()
        {
            yield return new WaitForSeconds(0.5f);
            parent.GetComponent<GridLayoutGroup>().enabled = false;
        }


        /// <summary>
        /// Chack connection to link
        /// </summary>
        /// <param name="resource">Link</param>
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
}
