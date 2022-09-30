using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MemoryManagement : MonoBehaviour
{
    [SerializeField] private AssetReference prefabReference = null;

    private IEnumerator DemoMemoryManagement()
    {
        var gameObjects = new List<GameObject>();

        for (var i = 0; i < 100; i++)
        {
            var asyncOperationHandle = prefabReference.InstantiateAsync(new Vector3(Random.Range(-4f, 4f), Random.Range(-4f, 4f)), Quaternion.identity);
            yield return asyncOperationHandle;
            var gameObjectInstance = asyncOperationHandle.Result;
            gameObjects.Add(gameObjectInstance);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1);

        for (var i = 0; i < gameObjects.Count; i++)
        {
            var gameObjectInstance = gameObjects[i];
            Addressables.ReleaseInstance(gameObjectInstance);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator Start()
    {
        yield return DemoMemoryManagement();
    }
}
