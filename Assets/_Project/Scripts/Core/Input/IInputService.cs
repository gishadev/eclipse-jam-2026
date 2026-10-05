using System;
using UnityEngine;

namespace gishadev.eclipse.Core.Input
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

        /// <summary>Mouse delta this frame, in pixels.</summary>
        Vector2 OrbitalLook { get; }

        /// <summary>True while the orbit drag button (RMB) is held.</summary>
        bool IsOrbitalLookHeld { get; }

        /// <summary>Scroll wheel this frame; ±1 per notch (Input System normalizes it by default). Positive = scroll up.</summary>
        float OrbitalScroll { get; }

        /// <summary>Pointer position in screen pixels.</summary>
        Vector2 PointerPosition { get; }

        /// <summary>True when the pointer is over a UI element that blocks raycasts (buttons, popups, ...).</summary>
        bool IsPointerOverUI();

        bool IsGameInputEnabled { get; }

        event Action PausePressed;

        /// <summary>Any keyboard key or mouse button (General map, so it works while Game input is off).</summary>
        event Action StartPressed;

        /// <summary>Salvage click (LMB). Read <see cref="PointerPosition"/> for where.</summary>
        event Action SalvagePressed;

        /// <summary>Turns the Game map on/off (e.g. while paused). General actions like Pause stay active.</summary>
        void SetGameInputEnabled(bool enabled);
    }
}
