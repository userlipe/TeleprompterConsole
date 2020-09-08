﻿using System;
using System.Collections.Generic;
using System.Text;
using static System.Math;

namespace TeleprompterConsole
{
    internal class TelePrompterConfig
    {
        public int DelayInMilliseconds { get; private set; } = 200;

        public void UpdateDelay(int increment) //É preciso um valor negativo para aumentar a velocidade
        {
            var newDelay = Min(DelayInMilliseconds + increment, 1000);
            newDelay = Max(newDelay, 20);
            DelayInMilliseconds = newDelay;
        }

        public bool Done { get; private set; }
        public void SetDone()
        {
            Done = true;
        }
    }
}
