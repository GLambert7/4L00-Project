using UnityEngine;

public class CurrentActivator : MonoBehaviour
{
    [SerializeField] private GeneratorPoweredObject current;

    public bool weedsShrunk = false;
    public bool blockBroken = false;

    void Update()
    {
        if (weedsShrunk && blockBroken)
        {
            current.Activate();
            enabled = false;
        }
    }
}