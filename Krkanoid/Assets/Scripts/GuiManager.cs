using UnityEngine;
using UnityEngine.UI;

public class GuiManager : MonoBehaviour {
    public static GuiManager instance;
    public Text scoreText;

    private int score;
    private const int winScore = 1460;
    private string[] partiesTags = { "HDZScore", "SDPScore", "HNSScore", "LabScore", "HDSSBScore", "IDSScore", "ORaHScore", "SDSSScore", "KNZScore", "HSSScore", "HSPScore", "HGSScore" };
    private int[] partiesCount = {43, 55, 9, 3, 15, 2, 1, 3, 4, 1, 1, 4};
    private float[] partiesScale = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

    private int GetIndex(string tag)
    {
        for (int i=0; i<partiesTags.Length; i++)
        {
            if (partiesTags[i] == tag)
            {
                return i;
            }
        }
        return -1;
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

    public void AddScore(int value)
    {
        score += value;
        DrawScore();
        if (score == winScore)
        {
            GameManager.instance.WinGame();
        }
    }

    public void Damage(string tag)
    {
        int index = GetIndex(tag);
        float currentScale = partiesScale[index];
        float scaleFactor = 1f / partiesCount[index];
        currentScale -= scaleFactor;
        partiesScale[index] = currentScale; 
        
        GameObject.Find(tag).GetComponent<RectTransform>().localScale = new Vector3(currentScale, 1f, 1f);
    }

    private void DrawScore()
    {
        scoreText.text = "Score: " + score;
    }
}
