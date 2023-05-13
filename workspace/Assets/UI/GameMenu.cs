using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private string[] videoQualityPresetNames;
    [SerializeField] private RenderPipelineAsset[] renderPipelineAssets;
    [SerializeField] private int defaultVideoQualityPresetIndex = 0;
    
    [SerializeField] private string[] difficultyLevelNames;
    [SerializeField] private float[] playerHealthLevels;
    [SerializeField] private int defaultDifficultyLevelIndex = 0;

    [SerializeField] private bool defaultFPSVisibility = false;
    
    [SerializeField] private GameObject healthBarObject;
    [SerializeField] private GameObject player;
    
    private DropdownField videoQualitySelector;
    private DropdownField difficultyLevelSelector;
    private Toggle fpsToggle;
    
    private Button restartButton;
    private Button mainMenuButton;
    private Button quitButton;
    
    private HealthBar healthBar;
    private MouseLookScript mouseLookScript;

    private int? currentVideoQualityPresetIndex = null;
    private int? currentDifficultyLevelIndex = null;
    private bool? currentFPSVisibility = null;

    private void OnEnable()
    {
        healthBar = healthBarObject.GetComponent<HealthBar>();
        mouseLookScript = player.GetComponent<MouseLookScript>();
        
        var uiDocument = GetComponent<UIDocument>();
        
        videoQualitySelector = uiDocument.rootVisualElement.Q<DropdownField>("VideoQualitySelector");
        videoQualitySelector.choices = videoQualityPresetNames.ToList();
        videoQualitySelector.RegisterValueChangedCallback(evt => _selectVideoQuality(evt.newValue));
        if (!currentVideoQualityPresetIndex.HasValue)
        {
            currentVideoQualityPresetIndex = defaultVideoQualityPresetIndex;
            videoQualitySelector.value = videoQualityPresetNames[defaultVideoQualityPresetIndex];
        }
        else
        {
            videoQualitySelector.SetValueWithoutNotify(videoQualityPresetNames[currentVideoQualityPresetIndex.Value]);
        }

        difficultyLevelSelector = uiDocument.rootVisualElement.Q<DropdownField>("DifficultyLevelSelector");
        difficultyLevelSelector.RegisterValueChangedCallback(evt => _selectDifficulty(evt.newValue));
        difficultyLevelSelector.choices = difficultyLevelNames.ToList();
        if (!currentDifficultyLevelIndex.HasValue)
        {
            currentDifficultyLevelIndex = defaultDifficultyLevelIndex;
            difficultyLevelSelector.value = difficultyLevelNames[defaultDifficultyLevelIndex];
        }
        else
        {
            difficultyLevelSelector.SetValueWithoutNotify(difficultyLevelNames[currentDifficultyLevelIndex.Value]);
        }
        
        fpsToggle = uiDocument.rootVisualElement.Q<Toggle>("FPSToggle");
        fpsToggle.RegisterValueChangedCallback(evt => _toggleFPSVisibility(evt.newValue));
        if (!currentFPSVisibility.HasValue)
        {
            currentFPSVisibility = defaultFPSVisibility;
            fpsToggle.value = defaultFPSVisibility;
        }
        else
        {
            fpsToggle.SetValueWithoutNotify(currentFPSVisibility.Value);
        }

        restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
        mainMenuButton = uiDocument.rootVisualElement.Q<Button>("MainMenuButton");
        quitButton = uiDocument.rootVisualElement.Q<Button>("QuitButton");

        restartButton.clicked += _restartGame;
        mainMenuButton.clicked += _quitToMainMenu;
        quitButton.clicked += _quitGame;
    }

    private void _selectVideoQuality(string choice)
    {
        int index = videoQualityPresetNames.ToList().IndexOf(choice);
        currentVideoQualityPresetIndex = index;
        
        QualitySettings.renderPipeline = renderPipelineAssets[index];
        
        Debug.Log("Video quality: " + videoQualityPresetNames[index]);
    }

    private void _selectDifficulty(string choice)
    {
        int index = difficultyLevelNames.ToList().IndexOf(choice);
        currentDifficultyLevelIndex = index;
        
        healthBar.health = healthBar.health / healthBar.initialHealth * playerHealthLevels[index];
        healthBar.initialHealth = playerHealthLevels[index];
        
        Debug.Log("Player health: " + healthBar.health + "/" + healthBar.initialHealth);
    }

    private void _restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void _quitToMainMenu()
    {
        // TODO
    }
    
    private void _quitGame()
    {
        Application.Quit();
    }

    private void _toggleFPSVisibility(bool visible)
    {
        currentFPSVisibility = visible;
        mouseLookScript.showFps = visible;
    }
}
