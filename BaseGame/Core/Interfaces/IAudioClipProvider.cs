using UnityEngine;

namespace BaseGame.Core.Interfaces
{
    public interface IAudioClipProvider
    {
        AudioClip GetClip(string clipPath);
    }
}