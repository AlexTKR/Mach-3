using System;
using UnityEngine;
using Zenject;

namespace Composition.Root
{
    public class MapSceneRoot : MonoBehaviour
    {
        private IInitBehaviour _initer;
        
        [Inject]
        private void Construct()
        {
            
        }

        private void Start()
        {
            
        }
    }
}
