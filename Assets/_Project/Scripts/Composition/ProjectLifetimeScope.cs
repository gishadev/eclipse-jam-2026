using gishadev.eclipse.Infrastructure.Input;
using gishadev.tools.Audio;
using gishadev.tools.Infrastructure;
using gishadev.tools.Pooling;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace gishadev.eclipse.Composition
{
    /// <summary>
    /// Root scope: lives for the whole app (spawned from VContainerSettings, DontDestroyOnLoad).
    /// Holds app-lifetime services: event bus, audio, pooled emitters, scene loading, later save/progress.
    /// </summary>
    public class ProjectLifetimeScope : LifetimeScope
    {
        [SerializeField] private AudioMasterSO audioMasterSO;
        [SerializeField] private PoolDataSO poolDataSO;

        protected override void Configure(IContainerBuilder builder)
        {
            // Audio and emitters stay off until their assets are assigned on the prefab.
            // SFXEmitter needs IAudioManager, so emitters also require audio.
            bool hasAudio = audioMasterSO != null;
            new GishadevToolsInstaller(audioMasterSO, poolDataSO)
            {
                RegisterAudio = hasAudio,
                RegisterEmitters = hasAudio && poolDataSO != null,
            }.Install(builder);

            builder.RegisterEntryPoint<InputService>();
        }
    }
}
