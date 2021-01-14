using UnityEngine;
using CastifyUnity;

namespace CastifyDemo
{
  [RequireComponent(typeof(UnityEngine.AudioSource))]
  [RequireComponent(typeof(UnityEngine.Camera))]
  public class BroadcastingDriver : MonoBehaviour {

    private static string API_TOKEN = /* ... */;

    public Broadcaster broadcaster { get; private set; }

    public VirtualAudio audioInput { get; private set; }

    public VirtualVideo videoInput { get; private set; }

    private Camera camera_;

    private CastifyApp app;

    void Awake() 
    {
      app = new CastifyApp(API_TOKEN);
      audioInput = app.NewVirtualAudio();
      videoInput = app.NewVirtualVideo();
      broadcaster = app.NewBroadcaster();

      // audio
      broadcaster.audioSource = new CastifyUnity.AudioSource.Virtual (audioInput);
      broadcaster.audioEncoderSetting = new AudioEncoderSetting() {};

      // video
      broadcaster.videoSource = new CastifyUnity.VideoSource.Virtual (videoInput);
      broadcaster.videoEncoderSetting = new VideoEncoderSetting() {
        bps = 1_000_000, // 1Mbps
        fps = 30
      };
    }

    void Start() 
    {
      camera_ = GetComponent<UnityEngine.Camera>();

      if (Microphone.devices.Length > 0) {
        Castify.SetupAudioRouting();
        var audioSource = GetComponent<UnityEngine.AudioSource>();
        audioSource.clip = Microphone.Start(Microphone.devices[0], true, 1, AudioSettings.GetConfiguration().sampleRate);
        audioSource.loop = true;
        while (Microphone.GetPosition(null) < 0) {}
        audioSource.Play();
      }
    }

    void OnDestroy() 
    {
      app?.Dispose();
      broadcaster?.Dispose();
      audioInput?.Dispose();
      videoInput?.Dispose();
    }

    void OnPostRender() 
    {
      if (broadcaster?.state == Broadcaster.State.Opened) {
        videoInput.Render(camera_.targetTexture);
      }
    }

    void OnAudioFilterRead(float[] data, int channels) 
    {
      if (broadcaster?.state == Broadcaster.State.Opened) {
        audioInput.Render(data, channels);
      }
    }
  }
}
