using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    [SerializeField] GameObject box;

    public void OnBoxSpawnerBtn()
    {
        Instantiate(box, transform.position, Quaternion.identity);
    }
}
