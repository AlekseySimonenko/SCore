using SCore.Ads;
using UnityEngine;
using Zenject;

public class IAdsManagerInstaller : MonoBehaviour
{
    private void Awake()
    {
        IAdsManager targetComponent = gameObject.GetComponent<IAdsManager>();
        ProjectContext.Instance.Container.Bind<IAdsManager>().FromInstance(targetComponent).AsSingle();
    }

    private void Start()
    {
    }

    private void Update()
    {
    }
}