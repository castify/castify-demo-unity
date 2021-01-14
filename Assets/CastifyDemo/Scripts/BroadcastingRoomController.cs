using UnityEngine;
using UnityEngine.UI;
using CastifyUnity;

namespace CastifyDemo
{
  public partial class BroadcastingRoomController : UIController {

    [ScenePath("BroadcastButton")]
    private Button broadcastToggle;

    [ScenePath("BroadcastButton/Text")]
    private Text broadcastToggleLabel;

    [ScenePath("BroadcastStatus")]
    private Image broadcastStatusTip;

    [ScenePath("BroadcastStatus/Text")]
    private Text broadcastStatusLabel;

    [ScenePath("StagePreview")]
    private RawImage stagePreview;

    [ScenePath("/StageCamera")]
    private BroadcastingDriver stage;

    [ScenePath("/StageCamera")]
    private Camera stageCamera;

    [ScenePath("/Stage/Object")]
    private Transform objectTransform;

    void Start() 
    {
      Application.RequestUserAuthorization(UserAuthorization.Microphone);

      stage.videoInput.bounds = new VideoSource.BoundingBox(w: 360, h: 640);

      broadcastToggle.onClick.AddListener(() => {
        switch (stage.broadcaster.state) {
          case Broadcaster.State.Closed: {
            stage.broadcaster.Resume(null);
            break;
          }
          case Broadcaster.State.Opened: {
            stage.broadcaster.Close();
            break;
          }
        }
      });

      stage.broadcaster.OnStateChange += e => {
        switch (e.state) {
          case Broadcaster.State.Closed: {
            broadcastToggleLabel.text = "●";
            broadcastStatusLabel.text = "OFFLINE";
            broadcastStatusTip.color = new Color(0.1f, 0.1f, 0.5f, 0.75f);
            break;
          }
          case Broadcaster.State.WIP: {
            broadcastToggleLabel.text = "○";
            broadcastStatusLabel.text = "WIP";
            broadcastStatusTip.color = new Color(0.66f, 0, 0.2f, 0.66f);  
            break;
          }
          case Broadcaster.State.Opened: {
            broadcastToggleLabel.text = "■";
            broadcastStatusLabel.text = "ON AIR";
            broadcastStatusTip.color = new Color(1, 0, 0, 0.5f);  
            break;
          }
        }
        broadcastToggle.interactable = e.state != Broadcaster.State.WIP;
      };

      // Makes size of the stage render texture fit to the screen size.
      stagePreview.texture 
        = stageCamera.targetTexture 
        = new RenderTexture(
          Screen.width, 
          Screen.height, 16
        );
    }

    void Update()
    {
      var t = (Time.realtimeSinceStartup * 1.5f % 2) - 1;
      objectTransform.localPosition = new Vector3(
        x: objectTransform.localPosition.x, 
        z: objectTransform.localPosition.z, 
        y: (t - 1) * (t + 1) * -1.75f
      );
    }
  }
}
