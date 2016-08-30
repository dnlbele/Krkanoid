using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public GameObject ball;
    public GameObject stick;

    private AudioSource audioSource;
    private float y_offset;
    private float init_stick_y_offset;
    private float init_stick_y_position;
    private float stick_y_offset;
    private float init_y;
    private float max_y = 1.5f;
    private float stick_init_y;
    private Vector3 stageDimensions;
    private float audioVolume = 0f;
    private float audioStepUp = 0.001f;
    private float audioStepDown = 0.005f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        audioSource.volume = audioVolume;
        StartCoroutine("FadeInMusic");
    }

    IEnumerator FadeInMusic()
    {
        while (audioVolume <= 1f)
        {
            audioVolume += audioStepUp;
            audioSource.volume = audioVolume;
            yield return new WaitForSeconds(audioStepUp);
        }
    }

    public void KillBackgroundMusic()
    {
        StartCoroutine("FadeOutMusic");
    }

    IEnumerator FadeOutMusic()
    {
        while (audioVolume > 0f)
        {
            audioVolume -= audioStepDown;
            audioSource.volume = audioVolume;
            yield return new WaitForSeconds(audioStepDown);
        }
        audioSource.Stop();
    }

    void Start()
    {
        stageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        init_stick_y_offset = stageDimensions.y - stick.transform.position.y;
        init_stick_y_position = stick.transform.position.y;
        init_y = Camera.main.transform.position.y;
        y_offset = Camera.main.transform.position.y - ball.transform.position.y;

    }

    void LateUpdate()
    {
        if (stick == null)
        {
            return;
        }
        if (ball != null)
        {
            float new_y = ball.transform.position.y + y_offset;
            new_y = Mathf.Clamp(new_y, init_y, max_y);
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, new_y, Camera.main.transform.position.z);
            stageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            stick_y_offset = stageDimensions.y - init_stick_y_offset;

            if (init_stick_y_position != stick_y_offset)
            {
                stick.GetComponent<PolygonCollider2D>().isTrigger = true;
            }
            else
            {
                stick.GetComponent<PolygonCollider2D>().isTrigger = false;
            }
            if (Mathf.Abs(init_stick_y_position - stick_y_offset) < 2)
            {
                stick.GetComponent<Animator>().SetBool("fading", false);
            }
            else
            {
                stick.GetComponent<Animator>().SetBool("fading", true);
            }

            stick.transform.position = new Vector3(stick.transform.position.x, stick_y_offset, stick.transform.position.z);
        }
    }

    public void KillBall()
    {
        Destroy(ball);
    }

    public void SetCameraInitPosition()
    {
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, init_y, Camera.main.transform.position.z);
    }
}
