using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Objects")]
    [SerializeField] private GameObject Border;
    [SerializeField] private GameObject Grid;
    [SerializeField] private GameObject Board;             
    [SerializeField] private GameObject Ghost;
    [SerializeField] private GameObject GameUI;

    [Header("UI Panels")]
    [SerializeField] private GameObject MainMenuPanel;
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private GameObject InstructionsPanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        Grid.SetActive(false);
        Border.SetActive(false);
        Board.SetActive(false);
        Ghost.SetActive(false);
        InstructionsPanel.SetActive(false);
        GameUI.SetActive(false);

        MainMenuPanel.SetActive(true);
        GameOverPanel.SetActive(false);
    }

    public void ShowInstructions()
    {
        Grid.SetActive(false);
        Border.SetActive(false);
        Board.SetActive(false);
        Ghost.SetActive(false);
        GameUI.SetActive(false);
        MainMenuPanel.SetActive(false);
        GameOverPanel.SetActive(false);

        InstructionsPanel.SetActive(true);
    }


    public void StartGame()
    {
        MainMenuPanel.SetActive(false);
        GameOverPanel.SetActive(false);
        InstructionsPanel.SetActive(false);

        Grid.SetActive(true);
        Border.SetActive(true);
        Board.SetActive(true);
        Ghost.SetActive(true);
        GameUI.SetActive(true);

        ScoreManager.Instance.ResetScore();
        Board.GetComponent<Board>().StartBoard();
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void GameOver()
    {
        Grid.SetActive(false);
        Border.SetActive(false);
        Ghost.SetActive(false);
        InstructionsPanel.SetActive(false);
        GameUI.SetActive(false);

        GameOverPanel.SetActive(true);
        Board.GetComponent<Board>().StopBoard();
        ScoreManager.Instance.ResetScore();
    }

    public void BackToMenu()
    {
        ShowMainMenu();
    }

    public void PlayAgain()
    {
        StartGame();
    }
}