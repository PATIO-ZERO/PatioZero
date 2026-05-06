using UnityEngine;
using UnityEngine.Video;
using System.IO;

public class WebGLVideoFix : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string videoFileName = "tranvia.webm"; // ?? cambia si tu archivo tiene otro nombre

    void Start()
    {
#if UNITY_WEBGL
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = Path.Combine(Application.streamingAssetsPath, videoFileName);
#else
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = Path.Combine(Application.streamingAssetsPath, videoFileName);
#endif
        videoPlayer.Play();
    }
}
