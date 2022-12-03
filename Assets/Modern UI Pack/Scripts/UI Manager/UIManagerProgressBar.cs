﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Michsky.UI.ModernUIPack
{
    [ExecuteInEditMode]
    public class UIManagerProgressBar : MonoBehaviour
    {
        [Header("SETTINGS")]
        public UIManager UIManagerAsset;

        [Header("RESOURCES")]
        public Image bar;
        public Image background;
        public TextMeshProUGUI label;

        bool dynamicUpdateEnabled;

        void OnEnable()
        {
            if (UIManagerAsset == null)
            {
                try
                {
                    UIManagerAsset = Resources.Load<UIManager>("MUIP Manager");
                }

                catch
                {
                    Debug.LogWarning("No UI Manager found. Assign it manually, otherwise you'll get errors about it.", this);
                }
            }
        }

        void Awake()
        {
            if (dynamicUpdateEnabled == false)
            {
                this.enabled = true;
                UpdateProgressBar();
            }
        }

        void LateUpdate()
        {
            if (Application.isEditor == true && UIManagerAsset != null)
            {
                if (UIManagerAsset.enableDynamicUpdate == true)
                {
                    dynamicUpdateEnabled = true;
                    UpdateProgressBar();
                }

                else
                    dynamicUpdateEnabled = false;
            }
        }

        void UpdateProgressBar()
        {
            try
            {
               // bar.color = UIManagerAsset.progressBarColor;
              //  background.color = UIManagerAsset.progressBarBackgroundColor;
                label.color = UIManagerAsset.progressBarLabelColor;
                label.font = UIManagerAsset.progressBarLabelFont;
                label.fontSize = UIManagerAsset.progressBarLabelFontSize;
            }

            catch { }
        }
    }
}