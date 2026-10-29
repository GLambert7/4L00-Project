using UnityEngine;

public class ChildOrb : MonoBehaviour
{
    private LightEmitter spawner;

    public void Initialize(LightEmitter source)
    {
        spawner = source;
        Debug.Log("Successfully referenced the spawner script!");
    }

}