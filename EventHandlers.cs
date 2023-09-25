using Exiled.Events.EventArgs;
using Exiled.Events.EventArgs.Player;
using UnityEngine.Diagnostics;
using Exiled.API.Features;
using UnityEngine;
using System.Collections.Generic;
using MEC;
using Exiled.Events.EventArgs.Server;
using Exiled.Events.EventArgs.Cassie;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Warhead;
using CustomPlayerEffects;
using FacilitySoundtrack;
using Respawning;

namespace Desert_Bus_SCP_SL
{
    public class EventHandlers
    {
        public CoroutineHandle updateCoroutineHandle;
        public IEnumerator<float> updateCoroutine()
        {
            while (true) 
            {
                Plugin.Instance.bus.Update();
                yield return Timing.WaitForOneFrame;
            }
        }
        public void WaitingForPlayers()
        {
            Init();
            Round.IsLocked = true;
            Round.Start();
        }
        public void RoundStarted()
        {
            Round.IsLocked = true;
            Map.AmbientSoundPlayer.StopAllCoroutines();
            Cassie.Announcer.enabled = false;
            Room.Get(Exiled.API.Enums.RoomType.Surface).Color = Color.white;
        }
        public void Init()
        {
            Plugin.Instance.bus = new Bus();
            Plugin.Instance.bus.SpawnModel( Room.Get( Exiled.API.Enums.RoomType.Surface).Position + (Vector3.up * 25f) + (Vector3.back * 55f));

            updateCoroutineHandle = Timing.RunCoroutine(updateCoroutine());
        }
        public void DeInit()
        {
            Timing.KillCoroutines(updateCoroutineHandle);
            Plugin.Instance.bus = null;
        }
        public void RoundRestarting()
        {
            DeInit();
        }
        public void OnPlayerVerified(VerifiedEventArgs ev)
        {
            ev.Player.RoleManager.ServerSetRole(Plugin.Instance.Config.playerConfig.spawnRoles.RandomItem(), PlayerRoles.RoleChangeReason.RoundStart);
            if (Plugin.Instance.bus != null)
            {
                Plugin.Instance.bus.points.updateCustomInfo(ev.Player.UserId);
            }
        }
        public void OnPlayerLeft(LeftEventArgs ev) 
        {
            /*if (Player.List.Count <= 0)
            {
                Timing.PauseCoroutines(updateCoroutineHandle);
                Log.Debug("Pausing couroutine handle because there are no players!");
            }*/
        }
        public void OnPlayerChangingRole(ChangingRoleEventArgs ev)
        {
            if ((ev.Reason == Exiled.API.Enums.SpawnReason.ForceClass &&  !Plugin.Instance.Config.remoteAdminConfig.ChangeRolesEnabled))
            {
                ev.IsAllowed = false;
                return;
            }

            ev.NewRole = Plugin.Instance.Config.playerConfig.spawnRoles.RandomItem();

            if (Plugin.Instance.Config.playerConfig.noInventory)
            {
                ev.ShouldPreserveInventory = false;
                ev.Ammo.Clear();
                ev.Items.Clear();
            }
            
        }
        public void OnPlayerSpawningRagdoll(SpawningRagdollEventArgs ev) 
        {
            if (Plugin.Instance.Config.playerConfig.noRagdolls) 
            {
                ev.IsAllowed = false;
            }
        }

        public void OnCassieMessage(SendingCassieMessageEventArgs ev)
        {
            ev.IsAllowed = false;
        }
        public void WarheadLeverStatusChange (ChangingLeverStatusEventArgs ev)
        {
            ev.IsAllowed = false;
        }

        public void AnnouncingDecontamination( AnnouncingDecontaminationEventArgs ev)
        {
        }

        public void OnPlayerSpawning(SpawningEventArgs ev)
        {
            ev.Position = Plugin.Instance.bus.SpawnPosition;
        }

        public void Decontaminating(DecontaminatingEventArgs ev)
        {
            ev.IsAllowed = false;
        }
        public void WarheadDetonating(DetonatingEventArgs ev)
        {
            ev.IsAllowed = false;
            Room.Get(Exiled.API.Enums.RoomType.Surface).Color = Color.white;
        }
        public void OnPlayerSpawned(SpawnedEventArgs ev)
        {
            ev.Player.Teleport(Plugin.Instance.bus.SpawnPosition);
            if (Plugin.Instance.bus != null)
            {
                Plugin.Instance.bus.points.updateCustomInfo(ev.Player.UserId);
            }
            ev.Player.EnableEffect(Exiled.API.Enums.EffectType.SoundtrackMute, duration: 0, false);
        }
        public void OnPlayerNoClip(TogglingNoClipEventArgs ev)
        {
            if (!Plugin.Instance.Config.remoteAdminConfig.NoClipEnabled)
            {
                ev.IsAllowed = false;
                ev.IsEnabled = false;
            }
        }

        public void OnPlayerSearchingPickup(SearchingPickupEventArgs ev) // BUS CONTROLS MOMENT!
        {
            if (Plugin.Instance.bus.controlButtons.ProcessInput(ev.Pickup, ev.Player))
            {
                ev.IsAllowed = false;
            }
        }

        public void RespawningTeam( RespawningTeamEventArgs ev)
        {
            ev.IsAllowed = false;
        }
    }
}