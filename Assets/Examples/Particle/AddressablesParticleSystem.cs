using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;

public class AddressablesParticleSystem : MonoBehaviour
{
    [SerializeField] private AssetReference explosionPrefabReference = null;

    public void SpawnExplosion(Vector3 position)
    {
        StartCoroutine(ManageParticleSystem(position));
    }

    private IEnumerator ManageParticleSystem(Vector3 position)
    {
        var asyncOperation = explosionPrefabReference.InstantiateAsync(position, Quaternion.identity, transform);
        yield return asyncOperation;
        var prefab = asyncOperation.Result;
        var particleSystems = prefab.GetComponentsInChildren<ParticleSystem>();
        Assert.IsTrue(particleSystems.Length > 0);

        foreach (var particleSystem in particleSystems)
        {
            Assert.IsFalse(particleSystem.main.loop);
            Assert.IsTrue(Mathf.Approximately(particleSystem.emission.rateOverTime.constant, 0f)); 
            Assert.IsTrue(Mathf.Approximately(particleSystem.emission.rateOverDistance.constant, 0f));
            yield return new WaitUntil(() => particleSystem.isEmitting == false);
        }
        Addressables.ReleaseInstance(asyncOperation);
    }
    IEnumerator Start()
    {
        for (var i = 0; i < 5; i++)
        {
            SpawnExplosion(Vector3.zero);
            yield return new WaitForSeconds(.2f);
        }
    }
}
