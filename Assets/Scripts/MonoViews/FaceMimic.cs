using System.Collections;
using UnityEngine;

public class FaceMimic : MonoBehaviour
{
    [SerializeField] private GameObject _constantFace;
    [SerializeField] private GameObject _mimicFace;
    [SerializeField] private float _mimicLength;
    private float _mimicSpan;

    private void OnEnable()
    {
        _constantFace.SetActive(true);
        _mimicFace.SetActive(false);
        _mimicSpan = Random.Range(Constants.MinMimicSpan, Constants.MaxMimicSpan);
        StartCoroutine(ShowMimic(true));
    }

    IEnumerator ShowMimic(bool mimicOn)
    {
        float count = 0;
        var length = mimicOn ? _mimicSpan : _mimicLength;
        while (count < length)
        {
            if(Time.timeScale != 0)
                count += Time.deltaTime;
            yield return null;
        }
        _constantFace.SetActive(!mimicOn);
        _mimicFace.SetActive(mimicOn);
        StartCoroutine(ShowMimic(!mimicOn));
    }
}