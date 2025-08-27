using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class LoopVideoClips : MonoBehaviour
{
    public VideoClip[] Clips;//ビデオ群

    public VideoPlayer VP;//出力動画

    private int now_video = 0;//現在再生中のビデオ
    private int max_videos;//最大ビデオ数
    private int flames = 0;
    private int now_flames = 0;
    const int MFLAME = 30;
    void Start()
    {
        max_videos = Clips.Length;//ビデオの数を取得
        VP.clip = Clips[0];//最初に再生するビデオを設定
        VP.Play();
    }

    void Update()
    {
        flames++;
        now_flames = flames / Set_FPS.FPS_Value;
        //次の動画を再生する
        if (now_flames > MFLAME && VP.isPlaying)
        {
            
            //再生する動画の番号を選択
            if (now_video < max_videos - 1)
                now_video++;
            else
                now_video = 0;
            //再生する動画、画像を設定
            VP.clip = Clips[now_video];
            flames = 0;
            now_flames = 0;
            VP.Play();//動画再生
        }
    }
}