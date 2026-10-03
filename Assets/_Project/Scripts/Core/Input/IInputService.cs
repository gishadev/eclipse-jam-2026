using System;
using UnityEngine;

namespace EclipseJam.Core.Input
{
    /// <summary>
    /// Game-facing input. Gameplay/UI depend on this, never on <c>EclipseInput</c> or devices directly,
    /// so tests can pass a fake.
    /// </summary>
    public interface IInputService
    {
        /// <summary>Orbital movement direction, polled every frame. Zero while game input is disabled.</summary>
        Vector2 OrbitalMovement { get; }
        float OrbitalZooming { get; }

        bool IsGameInputEnabled { get; }

        event Action PausePressed;

        /// <summary>Turns the Game map on/off (e.g. while paused). General actions like Pause stay active.</summary>
        void SetGameInputEnabled(bool enabled);
    }
}
