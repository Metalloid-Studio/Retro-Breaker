using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using LootLocker.Requests;

public class MenuHandler : MonoBehaviour
{
    public AudioSource ad;
    public AudioSource errorSound;
    public GameObject infoTab;
    public GameObject leaderBoardHolder;
    public TextMeshProUGUI level;
    public TextMeshProUGUI score;
    public TextMeshProUGUI ErrorText;
    //LeaderBoard
    int stageLeaderBoardID = 4713;
    int counter = 0;
    bool loginIsDone = false;
    public TMP_InputField playerNameInput;
    LootLockerLeaderboardMember[] members;

    public LeaderBoard leaderBoard;

    private void Start()
    {
        level.text = PlayerPrefs.GetInt("level").ToString();
        score.text = PlayerPrefs.GetInt("highScore").ToString();
        StartCoroutine(loginRoutine());
    }

    [System.Obsolete]
    private void Update()
    {
        if (counter == 0 && loginIsDone)
        {
            if (PlayerPrefs.GetInt("highScore") != 0)
                StartCoroutine(leaderBoard.submitScoreRoutine());
            counter = 1;
        }
    }

    [System.Obsolete]
    public void SetPlayerName()
    {
        members = leaderBoard.getMembers();

        bool IsValid = true;
        for (int i = 0; i < members.Length; i++)
        {
            if (playerNameInput.text == members[i].player.name)
            {
                IsValid = false;
                ErrorText.text = "This Name Is Taken";
            }
        }
        for (int i = 0; i < playerNameInput.text.Length; i++)
        {
            if (playerNameInput.text[i] >= '!' && playerNameInput.text[i] <= '+')
            {
                IsValid = false;
                ErrorText.text = "Invalid Characters";
            }
        }
        if (playerNameInput.text.Length > 13)
        {
            IsValid = false;
            ErrorText.text = "Name Is Too Long";
        }


        if (IsValid)
        {
            ad.Play();
            ErrorText.text = "";
            LootLockerSDKManager.SetPlayerName(playerNameInput.text, (responce) =>
            {
                if (responce.success)
                {
                    Debug.Log("Set Name Success");
                    leaderBoard.setPlayerStatus();
                    StartCoroutine(leaderBoard.fetchHighScores());
                }
                else
                {
                    Debug.Log("Name Error");
                }
            });
        }
        else 
        {
            errorSound.Play();
        }
    }
    public void onPlay()
    {
        SceneManager.LoadScene(1);
        ad.Play();
    }
    public void onInfo()
    {
        ad.Play();
        infoTab.SetActive(true);
    }
    public void onInfoClose()
    {
        ad.Play();
        infoTab.SetActive(false);
    }

    public void admin()
    {
        PlayerPrefs.DeleteAll();
    }

    IEnumerator loginRoutine()
    {
        bool done = false;
        LootLockerSDKManager.StartGuestSession((responce) =>
        {
            if (responce.success)
            {
                Debug.Log("Login Success");
                PlayerPrefs.SetString("PlayerID", responce.player_id.ToString());
                done = true;
                loginIsDone = true;
                leaderBoard.setPlayerStatus();
            }
        });
        yield return new WaitWhile(() => done = false);
    }

    [System.Obsolete]
    public void openLeaderBoard()
    {
        ad.Play();
        leaderBoardHolder.SetActive(true);
        if (counter == 1 && leaderBoard.getSubmitionStatus() && leaderBoard.getSettingStatus())
        {          
            StartCoroutine(leaderBoard.fetchHighScores());
            counter = 2;
        }

    }
    public void closeLeaderBoard() 
    {
        ad.Play();
        leaderBoardHolder.SetActive(false);
    }

    public void exitGame(){
        Application.Quit();
    }
}
