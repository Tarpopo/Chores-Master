using UnityEngine;
public class WinParticles : MonoBehaviour
{
    private ParticleSystem _winParticles;
    private void Start()
    {
        _winParticles = GetComponent<ParticleSystem>();
        FindObjectOfType<LevelManager>().OnLevelLoaded += () => SetParticles(false);
    }
    public void SetParticles(bool isActive)
    {
        _winParticles.gameObject.SetActive(isActive);
        if (isActive)_winParticles.Play();
        else _winParticles.Stop();
    }
}
