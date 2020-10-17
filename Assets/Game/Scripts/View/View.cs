using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.View
{
    public abstract class View : MonoBehaviour
    {
        public virtual void Open()
        {
            gameObject.SetActive(true);
        }

        public virtual void Open(ViewParameters viewParameters)
        {
            Open();
        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
        }
    }

    public abstract class ViewParameters {}
}