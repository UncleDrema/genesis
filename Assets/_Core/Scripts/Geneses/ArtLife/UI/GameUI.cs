using System;
using System.Collections.Generic;
using Genesis.GameWorld;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Geneses.ArtLife.UI
{
    public class GameUI : MonoBehaviour
    {
        [Header("Temporary Menu UI Reference")]
        [SerializeField] MenuUI MainMenuUI;

        [Header("Panels")]
        [SerializeField] List<GameObject> Panels;

        [Header("CellPanel")]
        [SerializeField] GameObject TableRowPrefab;
        [SerializeField] Transform TableContentTransform;
        [SerializeField] TMP_Text GenomeText;

        [Header("Buttons")]
        [SerializeField] Button SettingsButton;
        [SerializeField] Button QuitButton;
        [SerializeField] Button PanelSwitchLeftButton;
        [SerializeField] Button PanelSwitchRightButton;

        [Header("Settings")]
        [SerializeField] GameObject SettingsPanel;
        [SerializeField] Button SettingsApplyButton;
        [SerializeField] List<SliderView> Sliders;

        private Animator _settingsAnimator;
        private int _activePanel;
        private ArtLifeConfig _artLifeConfig;
        private GameWorldConfigAsset _gameWorldConfig;
        private Dictionary<SliderView, (Action<GameUI, SliderView> SetSliderValue, Action<SliderView, GameUI> GetSliderValue)> _slidersActions;

        [Inject]
        private void Construct(ArtLifeConfig config, GameWorldConfigAsset gameWorldConfig)
        {
            _artLifeConfig = config;
            _gameWorldConfig = gameWorldConfig;
        }

        void Start()
        {
            _activePanel = 0;
            EnablePanel(_activePanel);
            _settingsAnimator = SettingsPanel.GetComponent<Animator>();

            SettingsButton.onClick.AddListener(OnSettingsClick);
            QuitButton.onClick.AddListener(OnQuitClick);
            PanelSwitchLeftButton.onClick.AddListener(OnSwitchLeftPanel);
            PanelSwitchRightButton.onClick.AddListener(OnSwitchRightPanel);

            SettingsApplyButton.onClick.AddListener(OnSettingsApply);

            Sliders[0].SetValueMinMaxAndTitle(0f, 0.1f, "Шанс мутации (жизнь)");
            Sliders[1].SetValueMinMaxAndTitle(0f, 1f, "Шанс мутации (деление)");
            Sliders[2].SetValueMinMaxAndTitle(1f, 10f, "Энергия из органики");
            Sliders[3].SetValueMinMaxAndTitle(1f, 10f, "Энергия на тик");
            Sliders[4].SetValueMinMaxAndTitle(250, 2500, "Максимальная энергия");
            Sliders[5].SetValueMinMaxAndTitle(250, 2500, "Максимум минералов");
            Sliders[6].SetValueMinMaxAndTitle(0, 100, "Радиус зоны радиации");
            Sliders[7].SetValueMinMaxAndTitle(1, 1000, "Желаемая частота кадров");
            Sliders[8].SetValueMinMaxAndTitle(1, 60, "Рисовать каждый N кадр");
            _slidersActions =
                new()
                {
                    [Sliders[0]] = (
                        (ui, slider) => slider.Slider.value = ui._artLifeConfig.MutationChance,
                        (slider, ui) => ui._artLifeConfig.MutationChance = slider.Slider.value
                    ),
                    [Sliders[1]] = (
                        (ui, slider) => slider.Slider.value = ui._artLifeConfig.DuplicateMutationChance,
                        (slider, ui) => ui._artLifeConfig.DuplicateMutationChance = slider.Slider.value
                    ),
                    [Sliders[2]] = (
                        (ui, slider) => slider.Slider.value = ui._artLifeConfig.EnergyFromOrganic,
                        (slider, ui) => ui._artLifeConfig.EnergyFromOrganic = (int)slider.Slider.value
                    ),
                    [Sliders[3]] = (
                        (ui, slider) => slider.Slider.value = ui._artLifeConfig.EnergySpendPerTick,
                        (slider, ui) => ui._artLifeConfig.EnergySpendPerTick = (int)slider.Slider.value
                    ),
                    [Sliders[4]] = (
                        (ui, slider) => slider.Slider.value = ui._artLifeConfig.MaxEnergy,
                        (slider, ui) => ui._artLifeConfig.MaxEnergy = (int)slider.Slider.value
                    ),
                    [Sliders[5]] = (
                        (ui, slider) => slider.Slider.value = ui._artLifeConfig.MaxAccumulatedMinerals,
                        (slider, ui) => ui._artLifeConfig.MaxAccumulatedMinerals = (int)slider.Slider.value
                    ),
                    [Sliders[6]] = (
                        (ui, slider) => slider.Slider.value = ui._artLifeConfig.RadiationRadius,
                        (slider, ui) => ui._artLifeConfig.RadiationRadius = slider.Slider.value
                    ),
                    [Sliders[7]] = (
                        (ui, slider) => slider.Slider.value = ui._gameWorldConfig.DesiredFramerate,
                        (slider, ui) => ui._gameWorldConfig.DesiredFramerate = (int)slider.Slider.value
                    ),
                    [Sliders[8]] = (
                        (ui, slider) => slider.Slider.value = ui._gameWorldConfig.DrawEveryNthFrame,
                        (slider, ui) => ui._gameWorldConfig.DrawEveryNthFrame = (int)slider.Slider.value
                    )
                };
        }

        void OnDestroy()
        {
            SettingsButton.onClick.RemoveListener(OnSettingsClick);
            QuitButton.onClick.RemoveListener(OnQuitClick);
            PanelSwitchLeftButton.onClick.RemoveListener(OnSwitchLeftPanel);
            PanelSwitchRightButton.onClick.RemoveListener(OnSwitchRightPanel);

            SettingsApplyButton.onClick.RemoveListener(OnSettingsApply);
        }
        
        private void EnablePanel(int no)
        {
            for (int i = 0; i < Panels.Count; i++)
            {
                Panels[i].SetActive(i == no);
            }
        }

        public void LoadParametersIntoTable(Dictionary<string, string> parameters)
        {
            int currentRows = TableContentTransform.childCount - 1;
            int newRows = parameters.Count;
            int rowsDiff = newRows - currentRows;

            if (rowsDiff < 0)
            {
                for (int i = currentRows; i > newRows; i--)
                {
                    GameObject.Destroy(TableContentTransform.GetChild(i).gameObject);
                }
            }
            else if (rowsDiff > 0)
            {
                for (int i = 0; i < rowsDiff; i++)
                {
                    GameObject.Instantiate(TableRowPrefab, TableContentTransform);
                }
            }

            int row = 1;
            foreach (var (param, value) in parameters)
            {
                TableContentTransform.GetChild(row).GetComponent<TableRow>().SetContent(param, value);
                row++;
            }
        }
        
        public void SetGenomeText(string genome)
        {
            GenomeText.text = genome;
        }

        private void OnSettingsClick()
        {
            _settingsAnimator.SetBool("Opened", true);
            foreach (var (slider, actions) in _slidersActions)
            {
                actions.SetSliderValue(this, slider);
            }
        }

        private void OnSettingsApply()
        {
            _settingsAnimator.SetBool("Opened", false);
            foreach (var (slider, actions) in _slidersActions)
            {
                actions.GetSliderValue(slider, this);
            }
        }

        private void OnQuitClick()
        {
            MainMenuUI.Show();
        }

        private void OnSwitchLeftPanel()
        {
            _activePanel--;
            if (_activePanel < 0)
                _activePanel = Panels.Count - 1;

            EnablePanel(_activePanel);
        }

        private void OnSwitchRightPanel()
        {
            _activePanel++;
            if (_activePanel >= Panels.Count)
                _activePanel = 0;

            EnablePanel(_activePanel);
        }
    }
}
