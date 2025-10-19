using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine.UI;
using System.Threading;
using System;

public class SpriteFade : MonoBehaviour
{
    [SerializeField, Header("UIか")]
    private bool _isUI = false;

    [SerializeField, Header("SpriteRenderer"), HideIf(nameof(_isUI))]
    private SpriteRenderer _renderer;

    [SerializeField, Header("Image"), ShowIf(nameof(_isUI))]
    private Image _image;

    public async UniTask IOnSpriteFadeIn(float fadeTime, CancellationToken token)
    {
        float timeFrameSec = 0f;
        Color startColor = Color.white;
        Color endColor = Color.white;
        if (_isUI)
        {
            if (_image == null) return;
            startColor = new Color(_image.color.r, _image.color.g, _image.color.b, 0f);
            endColor = new Color(_image.color.r, _image.color.g, _image.color.b, 1f);
            _image.color = startColor;

            while (timeFrameSec < fadeTime)
            {
                _image.color = Color.Lerp(startColor, endColor, timeFrameSec / fadeTime);
                timeFrameSec += Time.deltaTime;
                try
                {
                    await UniTask.Yield(token);
                }
                catch (OperationCanceledException)
                {
                    _image.color = endColor;
                    return;
                }
            }
            _image.color = endColor;
        }
        else
        {
            if (_renderer == null) return;
            startColor = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, 0f);
            endColor = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, 1f);
            _renderer.color = startColor;

            while (timeFrameSec < fadeTime)
            {
                _renderer.color = Color.Lerp(startColor, endColor, timeFrameSec / fadeTime);
                timeFrameSec += Time.deltaTime;
                try
                {
                    await UniTask.Yield(token);
                }
                catch (OperationCanceledException)
                {
                    _renderer.color = endColor;
                    return;
                }
            }
            _renderer.color = endColor;
        }
    }

    public async UniTask IOnSpriteFadeOut(float fadeTime, CancellationToken token)
    {
        float timeFrameSec = 0;
        Color startColor = Color.white;
        Color endColor = Color.white;
        if (_isUI)
        {
            if (_image == null) return;
            startColor = new Color(_image.color.r, _image.color.g, _image.color.b, 1f);
            endColor = new Color(_image.color.r, _image.color.g, _image.color.b, 0f);
            _image.color = startColor;

            while (timeFrameSec < fadeTime)
            {
                _image.color = Color.Lerp(startColor, endColor, timeFrameSec / fadeTime);
                timeFrameSec += Time.deltaTime;
                try
                {
                    await UniTask.Yield(token);
                }
                catch (OperationCanceledException)
                {
                    _image.color = endColor;
                    return;
                }
            }
            _image.color = endColor;
        }
        else
        {
            if (_renderer == null) return;
            startColor = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, 1f);
            endColor = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, 0f);
            _renderer.color = startColor;

            while (timeFrameSec < fadeTime)
            {
                _renderer.color = Color.Lerp(startColor, endColor, timeFrameSec / fadeTime);
                timeFrameSec += Time.deltaTime;
                try
                {
                    await UniTask.Yield(token);
                }
                catch (OperationCanceledException)
                {
                    _renderer.color = endColor;
                    return;
                }
            }
            _renderer.color = endColor;
        }
    }

    public async void OnSpriteFadeIn(float fadeTime, CancellationToken token)
    {
        float timeFrameSec = 0f;
        Color startColor = Color.white;
        Color endColor = Color.white;
        if (_isUI)
        {
            if (_image == null) return;
            startColor = new Color(_image.color.r, _image.color.g, _image.color.b, 0f);
            endColor = new Color(_image.color.r, _image.color.g, _image.color.b, 1f);
            _image.color = startColor;

            while (timeFrameSec < fadeTime)
            {
                _image.color = Color.Lerp(startColor, endColor, timeFrameSec / fadeTime);
                timeFrameSec += Time.deltaTime;
                try
                {
                    await UniTask.Yield(token);
                }
                catch (OperationCanceledException)
                {
                    _image.color = endColor;
                    return;
                }
            }
            _image.color = endColor;
        }
        else
        {
            if (_renderer == null) return;
            startColor = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, 0f);
            endColor = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, 1f);
            _renderer.color = startColor;

            while (timeFrameSec < fadeTime)
            {
                _renderer.color = Color.Lerp(startColor, endColor, timeFrameSec / fadeTime);
                timeFrameSec += Time.deltaTime;
                try
                {
                    await UniTask.Yield(token);
                }
                catch (OperationCanceledException)
                {
                    _renderer.color = endColor;
                    return;
                }
            }
            _renderer.color = endColor;
        }
    }

    public async void OnSpriteFadeOut(float fadeTime, CancellationToken token)
    {
        float timeFrameSec = 0f;
        Color startColor = Color.white;
        Color endColor = Color.white;
        if (_isUI)
        {
            if (_image == null) return;
            startColor = new Color(_image.color.r, _image.color.g, _image.color.b, 1f);
            endColor = new Color(_image.color.r, _image.color.g, _image.color.b, 0f);
            _image.color = startColor;

            while (timeFrameSec < fadeTime)
            {
                _image.color = Color.Lerp(startColor, endColor, timeFrameSec / fadeTime);
                timeFrameSec += Time.deltaTime;
                try
                {
                    await UniTask.Yield(token);
                }
                catch (OperationCanceledException)
                {
                    _image.color = endColor;
                    return;
                }
            }
            _image.color = endColor;
        }
        else
        {
            if (_renderer == null) return;
            startColor = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, 1f);
            endColor = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, 0f);
            _renderer.color = startColor;

            while (timeFrameSec < fadeTime)
            {
                _renderer.color = Color.Lerp(startColor, endColor, timeFrameSec / fadeTime);
                timeFrameSec += Time.deltaTime;
                try
                {
                    await UniTask.Yield(token);
                }
                catch (OperationCanceledException)
                {
                    _renderer.color = endColor;
                    return;
                }
            }
            _renderer.color = endColor;
        }
    }
}
