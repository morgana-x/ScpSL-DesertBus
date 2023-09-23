using Exiled.API.Features;
using System.Linq;

namespace Desert_Bus_SCP_SL
{
    public sealed class Plugin : Plugin<Config>
    {
        public override string Author => "morgana";

        public override string Name => "Desert Bus";

        public override string Prefix => Name;

        public static Plugin Instance;

        private EventHandlers _handlers;

        public Bus bus;

        public override void OnEnabled()
        {
            Instance = this;

            RegisterEvents();

            base.OnEnabled();

            Server.IsHeavilyModded = true;

            Exiled.API.Features.Log.Info( "\n" + Plugin.Instance.Config.serverConfig.Logo);

        }

        public override void OnDisabled()
        {
            UnregisterEvents();

            Instance = null;

            base.OnDisabled();
        }

        private void RegisterEvents()
        {
            _handlers = new EventHandlers();

            Exiled.Events.Handlers.Server.WaitingForPlayers += _handlers.WaitingForPlayers;

            Exiled.Events.Handlers.Server.RoundStarted += _handlers.RoundStarted;

            Exiled.Events.Handlers.Server.RestartingRound += _handlers.RoundRestarting;

            Exiled.Events.Handlers.Server.RespawningTeam += _handlers.RespawningTeam;

            Exiled.Events.Handlers.Player.Verified += _handlers.OnPlayerVerified;

            Exiled.Events.Handlers.Player.Left += _handlers.OnPlayerLeft;

            Exiled.Events.Handlers.Player.SpawningRagdoll += _handlers.OnPlayerSpawningRagdoll;

            Exiled.Events.Handlers.Player.ChangingRole += _handlers.OnPlayerChangingRole;

            Exiled.Events.Handlers.Player.TogglingNoClip += _handlers.OnPlayerNoClip;

            Exiled.Events.Handlers.Player.SearchingPickup += _handlers.OnPlayerSearchingPickup;

            Exiled.Events.Handlers.Player.Spawning += _handlers.OnPlayerSpawning;

            Exiled.Events.Handlers.Player.Spawned += _handlers.OnPlayerSpawned;

        }

        private void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= _handlers.WaitingForPlayers;

            Exiled.Events.Handlers.Server.RoundStarted -= _handlers.RoundStarted;

            Exiled.Events.Handlers.Server.RestartingRound -= _handlers.RoundRestarting;

            Exiled.Events.Handlers.Server.RespawningTeam -= _handlers.RespawningTeam;

            Exiled.Events.Handlers.Player.Verified -= _handlers.OnPlayerVerified;

            Exiled.Events.Handlers.Player.Left -= _handlers.OnPlayerLeft;

            Exiled.Events.Handlers.Player.SpawningRagdoll -= _handlers.OnPlayerSpawningRagdoll;

            Exiled.Events.Handlers.Player.ChangingRole -= _handlers.OnPlayerChangingRole;

            Exiled.Events.Handlers.Player.TogglingNoClip -= _handlers.OnPlayerNoClip;

            Exiled.Events.Handlers.Player.SearchingPickup -= _handlers.OnPlayerSearchingPickup;

            Exiled.Events.Handlers.Player.Spawning -= _handlers.OnPlayerSpawning;

            Exiled.Events.Handlers.Player.Spawned += _handlers.OnPlayerSpawned;

            _handlers.DeInit();
            _handlers = null;
        }
    }
}