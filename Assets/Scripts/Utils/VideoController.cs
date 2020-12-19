using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    #region attributes
    [SerializeField]
    private VideoPlayer videoPlayer;
    [SerializeField]
    private RawImage rawImage;

    [SerializeField]
    private Text videoTimeText;
    [SerializeField]
    public Slider videoTimeSlider;

    private int currentHour;
    private int currentMinute;
    private int currentSecond;

    private int clipHour;
    private int clipMinute;
    private int clipSecond;

    public Button playPauseButton;
    public Button stopButton;

    [SerializeField]
    private Sprite playButtonSprite;
    [SerializeField]
    private Sprite pauseButtonSprite;

    #endregion

    #region private methods
    // Start is called before the first frame update
    void Start()
    {
        if (videoPlayer == null)
        {
            videoPlayer = this.GetComponent<VideoPlayer>();
        }
        if (rawImage == null)
        {
            rawImage = this.GetComponent<RawImage>();
        }
        //videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        //videoPlayer.SetTargetAudioSource(0, this.GetComponent<AudioSource>());
        //videoPlayer.IsAudioTrackEnabled(0);
        videoPlayer.playOnAwake = false;

        playPauseButton.onClick.AddListener(PlayOrPause);
        ToggleLoop(true);
    }

    // Update is called once per frame
    void Update()
    {
        ///Render VideoPlayerd video to UGUI RawImage
        rawImage.texture = videoPlayer.texture;
        if(videoPlayer.clip != null)
        {
            ShowVideoTime();
        }
    }
    private void ShowVideoTime()
    {
        // current video playback time
        currentHour = (int)videoPlayer.time / 3600;
        currentMinute = (int)(videoPlayer.time - currentHour * 3600) / 60;
        currentSecond = (int)(videoPlayer.time - currentHour * 3600 - currentMinute * 60);
        // Display the current video playback time on Text
        videoTimeText.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
            currentHour, currentMinute, currentSecond);
        // Assign the time scale of the current video playback to the Slider
        videoTimeSlider.value = (float)(videoPlayer.time / videoPlayer.clip.length);
    }
    private void ShowVideoLength(VideoClip videos)
    {
        videoPlayer.clip = videos;
        videoPlayer.Play();
        videoTimeSlider.gameObject.SetActive(true);
        clipHour = (int)videoPlayer.clip.length / 3600;
        clipMinute = (int)(videoPlayer.clip.length - clipHour * 3600) / 60;
        clipSecond = (int)(videoPlayer.clip.length - clipHour * 3600 - clipMinute * 60);
        videoTimeText.text = string.Format("{0:D2}:{1:D2}:{2:D2} ",
             clipHour, clipMinute, clipSecond);
    }
    private void PlayOrPause()
    {
        // If the video clip is not empty, and the video screen is not played
        if (videoPlayer.clip != null && (ulong)videoPlayer.frame < videoPlayer.frameCount)
        {
            PauseVideo();
        }
        else
        {
            PlayVideo();
        }
    }
    #endregion

    #region public methods
    public void PauseVideo()
    {
        //If the video is playing
        if (videoPlayer.isPlaying)
        {
            playPauseButton.image.sprite = pauseButtonSprite;
            videoPlayer.Pause();
        }
    }

    public void PlayVideo()
    {
        playPauseButton.image.sprite = playButtonSprite;
        videoPlayer.Play();
    }

    public void PlayVideoOnURL(string path)
    {
        if(path != string.Empty)
        {
            videoPlayer.url = path;
            videoPlayer.Play();
        }
        else
        {
            Debug.LogWarning("Fail to log video from URL");
        }
    }
    public void PlayVideoOnClip(VideoClip videoClip)
    {
        if(videoClip != null)
        {
            videoPlayer.clip = videoClip;
            videoPlayer.Play();
        }
        else
        {
            Debug.LogWarning("Fail to load Video");
        }
    }

    public void SetVideoTime(double newTime)
    {
        videoPlayer.time = newTime;
    }

    public double GetVideoClipLength()
    {
        return videoPlayer.clip.length;
    }

    public void StopVideo()
    {
        videoPlayer.Stop();
        playPauseButton.image.sprite = playButtonSprite;
        videoTimeSlider.gameObject.SetActive(false);
        videoTimeText.text = "";
    }

    public void ToggleLoop(bool loop)
    {
        videoPlayer.isLooping = loop;
    }
    #endregion
}
