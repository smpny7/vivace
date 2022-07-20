﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SelectScene
{
    public class View : MonoBehaviour
    {
        public static View Instance;

        [SerializeField] private GameObject artworkContentGameObject;
        [SerializeField] private GameObject artworkTemplateGameObject;
        [SerializeField] private GameObject musics;
        [SerializeField] private HorizontalLayoutGroup artworkContentHorizontalLayoutGroup;
        [SerializeField] private RectTransform artworkBackgroundRectTransform;
        [SerializeField] private RectTransform artworkBackgroundBottomRectTransform;
        [SerializeField] private RectTransform parentCanvasRectTransform;
        [SerializeField] private Scrollbar scrollbar;
        [SerializeField] private TextMeshProUGUI artistText;
        [SerializeField] private TextMeshProUGUI musicTitleText;
        [SerializeField] private ToggleGroup toggleGroup;

        private float _artworkBackgroundHeight;

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        private void Start()
        {
            InitializeArtworkBackground();
            InitializeArtworkSize();
            InitializeArtworkBothEndsPadding();
        }

        private void InitializeArtworkBackground()
        {
            // アートワーク背景の上半分のサイズ指定
            _artworkBackgroundHeight = parentCanvasRectTransform.sizeDelta.y - 700;
            artworkBackgroundRectTransform.sizeDelta = new Vector2(artworkBackgroundRectTransform.sizeDelta.x,
                _artworkBackgroundHeight * 0.7f);
            artworkBackgroundBottomRectTransform.sizeDelta =
                new Vector2(_artworkBackgroundHeight + 300, _artworkBackgroundHeight * 0.3f);

            // アートワーク背景の下半分のサイズ指定
            var bottomPosition = artworkBackgroundBottomRectTransform.anchoredPosition;
            bottomPosition.y -= _artworkBackgroundHeight * 0.7f - (497f - 106.8f);
            artworkBackgroundBottomRectTransform.anchoredPosition = bottomPosition;
        }

        private void InitializeArtworkSize()
        {
            ArtworkHeight = _artworkBackgroundHeight * 0.5f;
        }

        private void InitializeArtworkBothEndsPadding()
        {
            var padding = (int)parentCanvasRectTransform.sizeDelta.x / 2 - (int)ArtworkHeight * 3 / 4;
            artworkContentHorizontalLayoutGroup.padding.left = padding;
            artworkContentHorizontalLayoutGroup.padding.right = padding;
        }

        public GameObject ArtworkContentGameObject => artworkContentGameObject;
        public GameObject ArtworkTemplateGameObject => artworkTemplateGameObject;
        public GameObject Musics => musics;
        public float ArtworkHeight { get; private set; }

        public float Scrollbar
        {
            get => scrollbar.value;
            set => scrollbar.value = value;
        }
        
        public string ArtistText
        {
            set => artistText.text = value;
        }
        
        public string MusicTitleText
        {
            set => musicTitleText.text = value;
        }
    }
}