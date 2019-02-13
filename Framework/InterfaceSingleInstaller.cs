using System;
using UnityEngine;
using Zenject;

public class InterfaceSingleInstaller : MonoBehaviour
{
    public Component BindedComponent;

    private void Awake()
    {
        Type BindedInterface = BindedComponent.GetType().GetInterfaces()[0];
        ProjectContext.Instance.Container.Bind(BindedInterface).FromInstance(BindedComponent).AsSingle();
    }

    private void Start()
    {
    }

    private void Update()
    {
    }
}