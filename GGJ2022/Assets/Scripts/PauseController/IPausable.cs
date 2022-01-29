using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPausable {
    public void AttachPausable();
    public void DetachPausable();

    public void Pause();
    public void Slow(float percentage);
    public void Reset();
}
