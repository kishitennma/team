using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoHoverPlayer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject videoPanel;         //動画を表示するパネル（RawImageなど）
    public VideoPlayer videoPlayer;       //VideoPlayer コンポーネント

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (videoPanel != null)
            videoPanel.SetActive(true);           //表示

        if (videoPlayer != null)
        {
            videoPlayer.isLooping = true;
            videoPlayer.Play();                   //ループ再生開始
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (videoPlayer != null)
            videoPlayer.Stop();                   //再生停止

        if (videoPanel != null)
            videoPanel.SetActive(false);          //非表示
    }
}