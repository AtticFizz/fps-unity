using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    class Timer
    {
        private bool running;
        private float startTime;

        public Timer()
        {
            running = false;
            startTime = 0;
        }

        public void Start()
        {
            running = true;
            startTime = Time.time;
        }

        public void Stop()
        {
            running = false;
            startTime = 0;
        }

        public float Runtime()
        {
            return Time.time - startTime;
        }

        public bool isRunning()
        {
            return running;
        }
    }
}
