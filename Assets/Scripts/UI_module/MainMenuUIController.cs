using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button continueButton;

    
    private void Start()
    {
        playButton.onClick.AddListener(OnPlayButtonPressed);
        continueButton.onClick.AddListener(OnContinueButtonPressed);
        if(ScoreSaver.DataExist())
            continueButton.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        playButton.onClick.RemoveListener(OnPlayButtonPressed);
        playButton.onClick.RemoveListener(OnContinueButtonPressed);
    }
    
    private void OnPlayButtonPressed()
    {
        SceneManager.LoadScene("Scenes/Game",LoadSceneMode.Single);
        ScoreSaver.Reset();
    }
    
    private void OnContinueButtonPressed()
    {
        SceneManager.LoadScene("Scenes/Game",LoadSceneMode.Single);
    }
}
