using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance { get; private set; }

    [SerializeField] private GameObject _explosionPrefab;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    
    private void PlayEffect(Vector3 pos, GameObject effectPrefab)
    {
        GameObject effect = Instantiate(effectPrefab, pos, Quaternion.identity);

        Destroy(effect, 5.0f);
    }

    public void PlayExplosion(Vector3 pos)
    {
        PlayEffect(pos, _explosionPrefab);
    }
}
