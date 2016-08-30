using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameObject backgroundToHide;
    public GameObject backgroundToShowGameOver;
    public GameObject backgroundToShowWin;
    public GameObject stickToHide;
    public GameObject gameOverText;
    public GameObject winText;
    public GameObject restartButton;
    public GameObject quitButton;
    public GameObject turboModeText;
    public static GameManager instance;
    public CameraController cameraController;

    private Material fadeMaterial;
    private Color fadeColor = Color.black;
    private float fadeOutTime = 2f;
    private float fadeInTime = 2f;
    private bool win = false;

    void Awake()
    {
        Shader shader = Shader.Find("Plane/No zTest");
        fadeMaterial = new Material(shader);
        StartCoroutine("FadeIn");
    }

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void GameOver()
    {
        if (!win)
        {
            cameraController.KillBackgroundMusic();
            StartCoroutine(Fade(false));
        }
    }

    public void WinGame()
    {
        win = true;
        cameraController.KillBackgroundMusic();
        StartCoroutine(Fade(true));
    }


    private IEnumerator Fade(bool win)
    {
        yield return new WaitForSeconds(1f);
        float t = 0.0f;
        while (t < 1.0f)
        {
            yield return new WaitForEndOfFrame();
            t = Mathf.Clamp01(t + Time.deltaTime / fadeOutTime);
            DrawingUtilities.DrawQuad(fadeMaterial, fadeColor, t);
        }
        backgroundToHide.SetActive(false);
        stickToHide.SetActive(false);
        if (win)
        {
            backgroundToShowWin.SetActive(true);
            winText.SetActive(true);
        }
        else
        {
            backgroundToShowGameOver.SetActive(true);
            gameOverText.SetActive(true);
        }
        restartButton.SetActive(true);
        quitButton.SetActive(true);
        turboModeText.SetActive(false);
        GameObject.Find("Parties").SetActive(false);
        cameraController.SetCameraInitPosition();
        cameraController.KillBall();
        while (t > 0.0f)
        {
            yield return new WaitForEndOfFrame();
            t = Mathf.Clamp01(t - Time.deltaTime / fadeInTime);
            DrawingUtilities.DrawQuad(fadeMaterial, fadeColor, t);
        }
    }


    private IEnumerator FadeIn()
    {
        float t = 1.0f;
        while (t > 0.0f)
        {
            yield return new WaitForEndOfFrame();
            t = Mathf.Clamp01(t - Time.deltaTime / fadeInTime);
            DrawingUtilities.DrawQuad(fadeMaterial, fadeColor, t);
        }
    }

    private IEnumerator FadeOut(bool restart)
    {
        float t = 0.0f;
        while (t < 1.0f)
        {
            yield return new WaitForEndOfFrame();
            t = Mathf.Clamp01(t + Time.deltaTime / fadeOutTime);
            DrawingUtilities.DrawQuad(fadeMaterial, fadeColor, t);
        }
        if (restart)
        {
            SceneManager.LoadScene(0);
        } else
        {
            Application.Quit();
        }
    }

    public static class DrawingUtilities
    {
        public static void DrawQuad(Material aMaterial, Color aColor, float aAlpha)
        {
            aColor.a = aAlpha;
            aMaterial.SetPass(0);
            GL.PushMatrix();
            GL.LoadOrtho();
            GL.Begin(GL.QUADS);
            GL.Color(aColor);
            GL.Vertex3(0, 0, -1);
            GL.Vertex3(0, 1, -1);
            GL.Vertex3(1, 1, -1);
            GL.Vertex3(1, 0, -1);
            GL.End();
            GL.PopMatrix();
        }
    }

    public void RestartGame()
    {
        StartCoroutine(FadeOut(true));
    }

    public void QuitGame()
    {
        StartCoroutine(FadeOut(false));
    }

}
