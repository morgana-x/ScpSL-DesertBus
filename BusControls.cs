using Exiled.API.Features.Pickups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Features;
namespace Desert_Bus_SCP_SL
{
    public class BusControls
    {

        public Bus bus;
        public Pickup Button_SteerLeft;
        public Pickup Button_SteerRight;
        public Pickup Button_Acceleration;
        public Pickup Button_Door;

        public bool ProcessInput(Pickup item, Player pl)
        {
            if (item == null)
            {
                return false;
            }
            else if (item == Button_SteerLeft)
            {
                //pl.ShowHint("steered left");
                bus.SteerLeft();
                //SteerLeft(pl);
                return true;
            }
            else if (item == Button_SteerRight)
            {
                //pl.ShowHint("steered right");
                bus.SteerRight();
                //SteerRight(pl);
                return true;
            }
            else if (item == Button_Door)
            {
                //pl.ShowHint("toggled door");
                bus.ToggleDoor();
                //ToggleDoor(pl);
                return true;
            }
            else if (item == Button_Acceleration)
            {
                //pl.ShowHint("accelerated.\nSpeed: (" + bus.speed.ToString() + "/" + Plugin.Instance.Config.busConfig.maxSpeed.ToString() + ")");
                bus.Accelerate();
                //Accelerate(pl);
                return true;
            }
            return false;
        }
        public BusControls(Bus b)
        {
            bus = b;
        }
    }

    }
