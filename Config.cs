using Exiled.API.Interfaces;
using System.ComponentModel;
using System.Collections.Generic;
using Exiled.API.Enums;
using PlayerRoles;

namespace Desert_Bus_SCP_SL
{
    public class BusConfig
    {
        [Description("Should bus randomly swerve into side of road without input")]
        public bool AFKSwerve { get; set; } = true;

        [Description("Amount of time afk from steering wheel until swerve")]
        public float AFKSwerveTime { get; set; } = 10f;

        [Description("Max bus speed")]
        public float maxSpeed = 20.1168f; // meteres per second

        [Description("Acceleration of bus")]
        public float accelerationSpeed = 0.5f;

        [Description("decceleration of bus")]
        public float deccelerationSpeed = 0.15f;

        [Description("Friction multiplier when offroad")]
        public float frictionMultiplier { get; set; } = 10;
    }
    public class PlayerConfig
    {
        [Description("Should ragdolls spawn on death")]
        public bool noRagdolls { get; set; } = true;

        [Description("Should inventory be cleared on spawn")]
        public bool noInventory { get; set; } = true;

        [Description("Roles that the player can spawn as")]
        public List<RoleTypeId> spawnRoles { get; set; } = new List<RoleTypeId>() 
        {
            RoleTypeId.ClassD,
            RoleTypeId.Scientist
        };
    }

    public class RemoteAdminConfig
    {
        [Description("Should staff be able to noclip")]
        public bool NoClipEnabled { get; set; } = false;
        [Description("Should staff be able to force roles")]
        public bool ChangeRolesEnabled { get; set; } = false;

        [Description("Should staff be able to spawn weapons")]
        public bool SpawnWeaponsEnabled { get; set; } = false;

        [Description("Should staff be able to use cassie")]
        public bool CassieEnabled { get; set; } = false;
    }

    public class ServerConfig
    {
        public string Logo { get; set; } = @"██████╗ ███████╗███████╗███████╗██████╗ ████████╗    ██████╗ ██╗   ██╗███████╗
██╔══██╗██╔════╝██╔════╝██╔════╝██╔══██╗╚══██╔══╝    ██╔══██╗██║   ██║██╔════╝
██║  ██║█████╗  ███████╗█████╗  ██████╔╝   ██║       ██████╔╝██║   ██║███████╗
██║  ██║██╔══╝  ╚════██║██╔══╝  ██╔══██╗   ██║       ██╔══██╗██║   ██║╚════██║
██████╔╝███████╗███████║███████╗██║  ██║   ██║       ██████╔╝╚██████╔╝███████║
╚═════╝ ╚══════╝╚══════╝╚══════╝╚═╝  ╚═╝   ╚═╝       ╚═════╝  ╚═════╝ ╚══════╝";
    }
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        public bool Debug { get; set; } = false;

        public BusConfig busConfig { get; set; } = new BusConfig();

        public PlayerConfig playerConfig { get; set; } = new PlayerConfig();

        public RemoteAdminConfig remoteAdminConfig { get; set; } = new RemoteAdminConfig();

        public ServerConfig serverConfig { get; set; } = new ServerConfig();

    }
}