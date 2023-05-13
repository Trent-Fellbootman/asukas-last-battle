using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class QualitySelector : MonoBehaviour
{
    [SerializeField] private String[] renderPipelineNames;
    [SerializeField] private RenderPipelineAsset[] renderPipelineAssets;

    private void Awake()
    {
        var dropdownMenu = GetComponent<TMP_Dropdown>();
        dropdownMenu.options = renderPipelineNames.Select(pipelineName => new TMP_Dropdown.OptionData(pipelineName))
            .ToList();
        dropdownMenu.onValueChanged.AddListener(_setQuality);
    }

    private void Update()
    {
        if (renderPipelineNames.Length != renderPipelineAssets.Length)
        {
            throw new ArgumentException("The number of render pipeline names and assets must be the same.");
        }
    }

    private void _setQuality(int index)
    {
        GraphicsSettings.defaultRenderPipeline = renderPipelineAssets[index];
        QualitySettings.renderPipeline = renderPipelineAssets[index];
    }
}