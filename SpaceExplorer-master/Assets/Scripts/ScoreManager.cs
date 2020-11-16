using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public enum gameState
    {
        waiting,
        running,
        enterscore,
        leaderboard
    };

   public gameState gs;
    public GameObject scorePanel;
    public Text namesList;
    public GameObject inputBox;

    // Reference to the dreamloLeaderboard prefab in the scene
    dreamloLeaderBoard dl;


    // Start is called before the first frame update
    void Start()
    {
        // get the reference here...
        this.dl = dreamloLeaderBoard.GetSceneDreamloLeaderboard();
        this.gs = gameState.running;
        scorePanel.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (gs == gameState.enterscore)
        {
            scorePanel.SetActive(true);
        }
        else if (gs == gameState.leaderboard)
        {
            List<dreamloLeaderBoard.Score> scoreList = dl.ToListHighToLow();

            if (scoreList == null||scoreList.Count==0)
            {
                namesList.text="(loading...)";
            }
            else
            {
                Debug.Log("scorelist not null");
                int maxToDisplay = 5;
                int count = 0;
                namesList.text = "";
                Debug.Log(scoreList.Count);
                foreach (dreamloLeaderBoard.Score currentScore in scoreList)
                {
                    Debug.Log("scoreList iterate");
                    count++;
                    namesList.text=namesList.text+"\n"+currentScore.playerName+"  "+currentScore.score.ToString();
                    if (count >= maxToDisplay)
                    {
                        Debug.Log("scorelist too big");
                        gs = gameState.waiting;
                        break;
                    }
                }
               gs = gameState.waiting;
            }
        }
    }

    public void EnterScore(string name)
    {
        dl.AddScore(name, (int)Time.timeSinceLevelLoad);
        this.gs = gameState.leaderboard;
        inputBox.SetActive(false);
    }
}
