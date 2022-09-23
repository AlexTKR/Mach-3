using Composition;
using UnityEngine;
using Zenject;

namespace Root
{
     public class CompositionRoot : MonoBehaviour
     {
          private Initer _initer;

          [Inject]
          private void Construct(Initer initer)
          {
               _initer = initer;
          }

          private void Start()
          {
              _initer.Init();
          }
     }
}
