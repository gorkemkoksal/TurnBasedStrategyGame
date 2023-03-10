using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance { get; private set; }
    private CinemachineImpulseSource _impulseSource;
    private void Awake()
    {
        Instance = this;

        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    public void Shake(float intensity = 1f)
    {
        _impulseSource.GenerateImpulse(intensity);
    }
}
