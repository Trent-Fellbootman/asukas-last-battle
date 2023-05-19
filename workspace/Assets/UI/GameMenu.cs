using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private string[] videoQualityPresetLabels;
    [SerializeField] private string[] qualityLevelPresetNames;
    [SerializeField] private int defaultVideoQualityPresetIndex = 0;

    [SerializeField] private string[] difficultyLevelNames;
    [SerializeField] private float[] playerHealthLevels;
    [SerializeField] private int defaultDifficultyLevelIndex = 0;

    [SerializeField] private bool defaultFPSVisibility = false;
    [SerializeField] private bool defaultHudVisibility = true;
    // [SerializeField] private bool defaultPathTracingEnabled = false;
    [SerializeField] private bool defaultFullScreenEnabled = true;

    [SerializeField] private GameObject healthBarObject;
    [SerializeField] private GameObject player;

    [SerializeField] private GameObject[] hudGameObjects;
    [SerializeField] private string mainMenuSceneName;

    // [SerializeField] private GameObject globalVolumeObject;
    // [SerializeField] private VolumeProfile normalVolumeProfile;
    // [SerializeField] private VolumeProfile pathTracingProfile;

    // private Volume globalVolume;

    private DropdownField videoQualitySelector;
    private DropdownField difficultyLevelSelector;
    private Toggle fpsToggle;
    private Toggle hudToggle;
    // private Toggle pathTracingToggle;
    private Toggle fullScreenToggle;

    private Button restartButton;
    private Button mainMenuButton;
    private Button quitButton;

    private HealthBar healthBar;
    private MouseLookScript mouseLookScript;
    private GunInventory gunInventory;

    private int? currentVideoQualityPresetIndex = null;
    private int? currentDifficultyLevelIndex = null;
    private bool? currentFPSVisibility = null;
    private bool? currentHudVisibility = null;
    // private bool? pathTracingEnabled = null;
    private bool? fullScreenEnabled = null;
    private Vector3[] lastHudGameObjectLocalScales;

    private void Awake()
    {
        lastHudGameObjectLocalScales = new Vector3[hudGameObjects.Length];
        for (int i = 0; i < hudGameObjects.Length; i++)
        {
            lastHudGameObjectLocalScales[i] = hudGameObjects[i].transform.localScale;
        }

        // globalVolume = globalVolumeObject.GetComponent<Volume>();
    }

    private void OnEnable()
    {
        healthBar = healthBarObject.GetComponent<HealthBar>();
        mouseLookScript = player.GetComponent<MouseLookScript>();
        gunInventory = player.GetComponent<GunInventory>();

        var uiDocument = GetComponent<UIDocument>();

        videoQualitySelector = uiDocument.rootVisualElement.Q<DropdownField>("VideoQualitySelector");
        videoQualitySelector.choices = videoQualityPresetLabels.ToList();
        videoQualitySelector.RegisterValueChangedCallback(evt => _selectVideoQuality(evt.newValue));
        if (!currentVideoQualityPresetIndex.HasValue)
        {
            currentVideoQualityPresetIndex = defaultVideoQualityPresetIndex;
            videoQualitySelector.value = videoQualityPresetLabels[defaultVideoQualityPresetIndex];
        }
        else
        {
            videoQualitySelector.SetValueWithoutNotify(videoQualityPresetLabels[currentVideoQualityPresetIndex.Value]);
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
            fpsToggle.value = currentFPSVisibility.Value;
            _toggleFPSVisibility(fpsToggle.value);
        }
        else
        {
            fpsToggle.SetValueWithoutNotify(currentFPSVisibility.Value);
        }

        hudToggle = uiDocument.rootVisualElement.Q<Toggle>("HUDToggle");
        hudToggle.RegisterValueChangedCallback(evt => _toggleHUDVisibility(evt.newValue));
        if (!currentHudVisibility.HasValue)
        {
            currentHudVisibility = defaultHudVisibility;
            hudToggle.value = currentHudVisibility.Value;
            _toggleHUDVisibility(hudToggle.value);
        }
        else
        {
            hudToggle.SetValueWithoutNotify(currentHudVisibility.Value);
        }

        fullScreenToggle = uiDocument.rootVisualElement.Q<Toggle>("FullScreenToggle");
        fullScreenToggle.RegisterValueChangedCallback(evt => _toggleFullScreen(evt.newValue));
        if (!fullScreenEnabled.HasValue)
        {
            fullScreenEnabled = defaultFullScreenEnabled;
            fullScreenToggle.value = fullScreenEnabled.Value;
            _toggleFullScreen(fullScreenToggle.value);
        }
        else
        {
            fullScreenToggle.SetValueWithoutNotify(fullScreenEnabled.Value);
        }

        // pathTracingToggle = uiDocument.rootVisualElement.Q<Toggle>("PathTracingToggle");
        // pathTracingToggle.RegisterValueChangedCallback(evt => _togglePathTracing(evt.newValue));
        // if (!pathTracingEnabled.HasValue)
        // {
        //     pathTracingEnabled = defaultPathTracingEnabled;
        // }
        // else
        // {
        //     pathTracingToggle.SetValueWithoutNotify(pathTracingEnabled.Value);
        // }

        restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
        mainMenuButton = uiDocument.rootVisualElement.Q<Button>("MainMenuButton");
        quitButton = uiDocument.rootVisualElement.Q<Button>("QuitButton");

        restartButton.clicked += _restartGame;
        mainMenuButton.clicked += _quitToMainMenu;
        quitButton.clicked += _quitGame;

        // custom scroll behavior
        var scrollView = uiDocument.rootVisualElement.Q<ScrollView>("ScrollView");

        verticalScroller = scrollView.Q("unity-content-and-vertical-scroll-container").Q<Scroller>();

        scrollView.RegisterCallback<WheelEvent>(@event =>
        {
            scrollVelocity += @event.delta.y * 100;
            
            // Stop the event here so the builtin scroll functionality of the list doesn't activate
            @event.StopPropagation();
        });
    }

    private void _selectVideoQuality(string choice)
    {
        int index = videoQualityPresetLabels.ToList().IndexOf(choice);
        currentVideoQualityPresetIndex = index;

        string qualityPresetName = qualityLevelPresetNames[index];

        QualitySettings.SetQualityLevel(QualitySettings.names.ToList().IndexOf(qualityPresetName), true);

        Debug.Log("Selecting quality preset: " + qualityPresetName);
    }

    private void _toggleFullScreen(bool newEnabled)
    {
        Screen.fullScreen = newEnabled;
        
        if (newEnabled)
        {
            Debug.Log("Fullscreen enabled");
        }
        else
        {
            Debug.Log("Fullscreen disabled");
        }
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
        SceneManager.LoadScene(mainMenuSceneName);
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

    private void _toggleHUDVisibility(bool visible)
    {
        currentHudVisibility = visible;
        if (visible)
        {
            _showHud();
        }
        else
        {
            _hideHud();
        }
    }
    
    // private void _togglePathTracing(bool newEnabled)
    // {
    //     pathTracingEnabled = newEnabled;
    //     globalVolume.profile = newEnabled ? pathTracingProfile : normalVolumeProfile;
    // }

    private void _hideHud()
    {
        gunInventory.ToggleHudVisibility(false);

        for (int i = 0; i < hudGameObjects.Length; i++)
        {
            lastHudGameObjectLocalScales[i] = hudGameObjects[i].transform.localScale;
        }

        foreach (var obj in hudGameObjects)
        {
            obj.transform.localScale = Vector3.zero;
        }
    }

    private void _showHud()
    {
        gunInventory.ToggleHudVisibility(true);

        for (int i = 0; i < hudGameObjects.Length; i++)
        {
            hudGameObjects[i].transform.localScale = lastHudGameObjectLocalScales[i];
        }
    }

    private float scrollVelocity = 0;
    private readonly float scrollDamping = 0.1f;
    private Scroller verticalScroller;

    private void Update()
    {
        if (verticalScroller != null)
        {
            verticalScroller.value += scrollVelocity;
            scrollVelocity -= scrollVelocity * scrollDamping;
        }
    }
}