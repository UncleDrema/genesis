using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Genesis
{
    public class GameUI : MonoBehaviour
    {
        [Header("Temporary Menu UI Reference")]
        [SerializeField] MenuUI MainMenuUI;

        [Header("Table")]
        [SerializeField] GameObject TableRowPrefab;
        [SerializeField] Transform TableContentTransform;

        [Header("Buttons")]
        [SerializeField] Button SettingsButton;
        [SerializeField] Button QuitButton;
        [SerializeField] Button GraphSwitchLeftButton;
        [SerializeField] Button GraphSwitchRightButton;

        [Header("Settings")]
        [SerializeField] GameObject SettingsPanel;
        [SerializeField] Button SettingsApplyButton;
        [SerializeField] Slider SettingsSlider1;
        [SerializeField] Slider SettingsSlider2;    

        private Animator SettingsAnimator;

        void Start()
        {
            SettingsAnimator = SettingsPanel.GetComponent<Animator>();

            SettingsButton.onClick.AddListener(OnSettingsClick);
            QuitButton.onClick.AddListener(OnQuitClick);
            GraphSwitchLeftButton.onClick.AddListener(OnGraphSwitchLeftClick);
            GraphSwitchRightButton.onClick.AddListener(OnGraphSwitchRightClick);

            SettingsApplyButton.onClick.AddListener(OnSettingsApply);

            LoadParametersIntoTable(new Dictionary<string, string> { { "Параметр 1", "1" }, { "Параметр 2", "2"}, { "Параметр 3", "3" } });
        }

        void OnDestroy()
        {
            SettingsButton.onClick.RemoveListener(OnSettingsClick);
            QuitButton.onClick.RemoveListener(OnQuitClick);
            GraphSwitchLeftButton.onClick.RemoveListener(OnGraphSwitchLeftClick);
            GraphSwitchRightButton.onClick.RemoveListener(OnGraphSwitchRightClick);

            SettingsApplyButton.onClick.RemoveListener(OnSettingsApply);
        }

        private void LoadParametersIntoTable(Dictionary<string, string> parameters)
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

        private void OnSettingsClick()
        {
            SettingsAnimator.SetBool("Opened", true);
        }

        private void OnSettingsApply()
        {
            SettingsAnimator.SetBool("Opened", false);

            // TODO: Применить настройки
            Debug.Log($"Settings values: {SettingsSlider1.value}; {SettingsSlider2.value}");
        }

        private void OnQuitClick()
        {
            // TODO: Заменить на загрузку меню
            MainMenuUI.Show();
        }

        private void OnGraphSwitchLeftClick()
        {

        }

        private void OnGraphSwitchRightClick()
        {

        }
    }
}
