using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AIOFramework.Runtime
{
    public partial class Entrance : MonoBehaviour
    {
        private void Start()
        {
            InitBuiltinComponents();
            DontDestroyOnLoad(this);
        }
    }
}

