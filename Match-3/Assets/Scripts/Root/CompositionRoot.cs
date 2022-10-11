using Composition;
using Controllers;
using UnityEngine;
using UtilitiesAndHelpers;
using Zenject;

namespace Root
{
     public class CompositionRoot : MonoBehaviour
     {
          private IInitBehaviour _initer;
          private ILoadGameSettings _loadGameSettings;

          [Inject]
          private void Construct(IInitBehaviour initer, ILoadGameSettings loadGameSettings)
          {
               _initer = initer;
               _loadGameSettings = loadGameSettings;
          }

          private void Start()
          {
               _loadGameSettings.LoadGameSettings();
               _initer.Init();
          }
     }
}
