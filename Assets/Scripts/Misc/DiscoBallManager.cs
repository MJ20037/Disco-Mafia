using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DiscoBallManager : MonoBehaviour
{
    [SerializeField] private float _discoBallPartyTime = 2f;
    [SerializeField] private Light2D _globalLight;
    [SerializeField] private float _dimLightIntensity = 0.5f;

    public static Action OnDiscoBallHitEvent;

    private ColourSpotLight[] _allSpotLights;
    private Coroutine _discoCoroutine;
    private float _defaultLightIntensity;

    private void Awake()
    {
        _defaultLightIntensity = _globalLight.intensity;
    }

    private void Start()
    {
        _allSpotLights = FindObjectsByType<ColourSpotLight>(FindObjectsSortMode.None);
    }

    private void OnEnable()
    {
        OnDiscoBallHitEvent += DimLights;
    }

    private void OnDisable()
    {
        OnDiscoBallHitEvent -= DimLights;
    }

    public void DicoBallParty()
    {
        if(_discoCoroutine != null){ return; }
        OnDiscoBallHitEvent?.Invoke();
    }

    private void DimLights()
    {
        foreach (ColourSpotLight sl in _allSpotLights)
        {
            StartCoroutine(sl.ChangeSpotLightRotSpeed(_discoBallPartyTime));
        }

        _discoCoroutine = StartCoroutine(GlobalLightResetRoutine());
    }

    private IEnumerator GlobalLightResetRoutine()
    {
        _globalLight.intensity = _dimLightIntensity;
        yield return new WaitForSeconds(_discoBallPartyTime);
        _globalLight.intensity = _defaultLightIntensity;
        _discoCoroutine = null;
    }
}
