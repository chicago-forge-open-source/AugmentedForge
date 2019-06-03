using UnityEngine;

namespace Main
{
    public interface CompassInterface
    {
        bool IsEnabled { get; }
        float TrueHeading { get; }
    }
}