using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MountAndBlade
{
    public interface ITriggerCheckable
    {
        bool IsChecked { get; set; }
        void SetCheckStatus(bool isChecked);
    }
}
