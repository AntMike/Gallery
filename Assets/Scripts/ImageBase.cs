using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gallery.Base
{
    public class ImageBase : MonoBehaviour
    {

        public static ImageBase Instance;
        /// <summary>
        /// Base of links to images
        /// </summary>
        public List<string> links;
        /// <summary>
        /// Local base of images
        /// </summary>
        public List<Texture> oflineList;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }
    }
}
