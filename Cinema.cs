using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class WebGLVideoFix1 : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string videoFileName = "intro.webm"; // ?? cambia si tu archivo tiene otro nombre

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
