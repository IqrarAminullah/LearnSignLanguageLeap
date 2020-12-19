using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// Inherit drag and drop interface
/// </summary>
public class SliderEvent : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField]
    public VideoController videoController; // script for video playback

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Add a start drag event to the Slider
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        videoController.PauseVideo();
        SetVideoTimeValueChange();
    }

    /// <summary>
    /// The current Slider scale value is converted to the current video playback time
    /// </summary>
    private void SetVideoTimeValueChange()
    {
        videoController.SetVideoTime(videoController.videoTimeSlider.value * videoController.GetVideoClipLength());
    }

    /// <summary>
    /// Add an end drag event to the Slider
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        videoController.PlayVideo();
    }
}
