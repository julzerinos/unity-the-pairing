using UnityEngine;

public class ObstacleParticles : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        transform.parent.gameObject.SetActive(false);
    }
}