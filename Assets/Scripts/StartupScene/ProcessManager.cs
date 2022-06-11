﻿using System;
using System.Collections;
using System.IO;
using Tools.AssetBundle;
using Tools.Authentication;
using Tools.Firestore;
using Tools.Firestore.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StartupScene
{
    public class ProcessManager : MonoBehaviour
    {
        private readonly AuthenticationHandler _auth = new();

        private bool _hasPressedStartButton;

        private void Start()
        {
            _auth.Start(_setUserData);
            _setUserData(this, null);
            View.Instance.UidText = _auth.GetUser()?.UserId;
            View.Instance.setOnClickSignInWithAppleCustomButtonAction = () => _auth.SignInWithApple();
            View.Instance.setOnClickSignInWithGoogleCustomButtonAction = () => _auth.SignInWithGoogle();
            View.Instance.setOnClickSignInWithAnonymouslyCustomButtonAction = () => _auth.SignInWithAnonymously();
            View.Instance.setOnClickNicknameRegistrationSaveCustomButtonCustomButtonAction =
                () =>
                {
                    if (View.Instance.DisplayNameInputField.Length is 0 or > 12)
                        View.Instance.DisplayNameErrorText = "12文字以内で入力してください";
                    else
                    {
                        _auth.UpdateDisplayName(View.Instance.DisplayNameInputField);
                        View.Instance.setNicknameRegistrationModalVisible = false;
                        StartCoroutine(_TransitionToSelectScene());
                    }
                };
        }

        private void Update()
        {
            _auth.Update();

            if (_hasPressedStartButton || Input.touchCount <= 0) return;
            OnPressedStartButton();
            _hasPressedStartButton = true;
        }

        private void OnDestroy()
        {
            _auth.OnDestroy(_setUserData);
        }

        public void OnClickSignOutButton()
        {
            _auth.SignOut();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void _setUserData(object sender, EventArgs eventArgs)
        {
            var user = _auth.GetUser();
            View.Instance.UidText = user?.UserId;
            Debug.Log(user?.UserId != null);
            if (user?.UserId != null)
            {
                View.Instance.setAccountLinkageModalVisible = false;
                Debug.Log("user.DisplayName == null");
                Debug.Log(user.DisplayName == null);
                Debug.Log(user.ProviderData);
                if (user.DisplayName == "")
                {
                    _hasPressedStartButton = true;
                    View.Instance.setNicknameRegistrationModalVisible = true;
                    View.Instance.DisplayNameInputField = user.DisplayName;
                } else if (_hasPressedStartButton)
                {
                    StartCoroutine(_TransitionToSelectScene());
                }
            }
        }

        private void OnPressedStartButton()
        {
            View.Instance.StartAudioSource.Play();
            var user = _auth.GetUser();
            if (user?.UserId != null)
                StartCoroutine(_TransitionToSelectScene());
            else
            {
                View.Instance.setAccountLinkageModalVisible = true;
            }
        }

        private IEnumerator _TransitionToSelectScene()
        {
            var db = new FirestoreHandler();

            // Check License
            var ie = db.GetIsSupportedVersionCoroutine(Application.version);
            yield return ie;
            var isValidLicense = ie.Current != null && (bool)ie.Current;

            if (!isValidLicense)
            {
                // TODO: 最新のバージョンを使用するようにポップアップ
                Debug.LogError("最新のバージョンをご使用ください");
                Application.Quit(); // TMP
            }

            // Get Music List
            ie = db.GetMusicListCoroutine();
            yield return StartCoroutine(ie);
            var musicList = (Music[])ie.Current;

            // Cache Setting
            var cachePath = Path.Combine(Application.persistentDataPath, "cache");
            Directory.CreateDirectory(cachePath);
            var cache = Caching.AddCache(cachePath);
            Caching.currentCacheForWriting = cache;

            // Download Asset Bundles
            var assetBundleHandler = new AssetBundleHandler(musicList);
            assetBundleHandler.OnCompletionRateChanged += rate => View.Instance.CompletionRateText = $"DL: {rate}%";
            assetBundleHandler.OnDownloadCompleted += () => SceneManager.LoadScene("SelectScene");
            assetBundleHandler.OnErrorOccured += error =>
            {
                // TODO: エラーを出力する
                SceneManager.LoadScene("StartupScene");
            };
            var downloadEnumerator = assetBundleHandler.DownloadCoroutine();
            yield return StartCoroutine(downloadEnumerator);
        }
    }
}