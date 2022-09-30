using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class PrefabLoader : MonoBehaviour
{
    [SerializeField] private AssetReference prefabReference = null;

    private IEnumerator DemoAddressablePrefab()
    {
        var asyncOperationHandle = prefabReference.LoadAssetAsync<GameObject>();

        yield return asyncOperationHandle;

        var prefab = asyncOperationHandle.Result;

        List<GameObject> objs = new List<GameObject>();

        for (var i = 0; i < 100; i++)
        {
            var obj = Instantiate(prefab, this.transform);
            obj.transform.position = new Vector3(Random.Range(-4f, 4f), Random.Range(-4f, 4f));

            objs.Add(obj);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1);

        for (var i = 0; i < objs.Count; i++)
        {
            Destroy(objs[i]);
            yield return new WaitForSeconds(0.1f);
        }
        Addressables.Release(asyncOperationHandle);
    }

    IEnumerator Start()
    {
        yield return DemoAddressablePrefab();
    }
}
