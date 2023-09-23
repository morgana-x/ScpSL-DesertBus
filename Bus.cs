using PluginAPI.Core.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using InventorySystem.Items;
using Exiled.API.Features.Pickups;
using UnityEngine;
using Exiled.API.Features.Toys;
using InventorySystem.Items.Pickups;
using Exiled.API.Interfaces;

namespace Desert_Bus_SCP_SL
{

    
   
    public class Bus
    {
        public BusControls controlButtons;// = new BusControls();

        public float speed = 0f;
        public float maxSpeed = Plugin.Instance.Config.busConfig.maxSpeed;
        public Vector3 Position = Vector3.zero;
        public Vector3 SpawnPosition = Vector3.zero;
    
        public float Distance;

        public Road road;

        public Vector3 VirtualPosition = Vector3.zero;
        public Vector3 VirtualEulerAngles = Vector3.zero;

        public List<Primitive> DoorObject;

        public bool DoorToggle = true;
        public Pickup spawnPickup(Vector3 pos)
        {
            Pickup p = Pickup.Create(ItemType.Medkit);
            p.Position = pos;
            p.GameObject.GetComponent<Rigidbody>().isKinematic = true;
            p.PickupTime = 1000f;
            p.Spawn();
            return p;
        }

        public void SpawnButtons(Vector3 position)
        {
            Vector3 upO = Vector3.up * 1f;
            Vector3 downO = Vector3.up * 0.5f;
            Vector3 RightO = Vector3.right * 1f;
            Vector3 fwd = Vector3.fwd;
            controlButtons.Button_SteerRight = spawnPickup( position + (upO*1.5f) + (RightO *-1.7f));
            controlButtons.Button_SteerLeft = spawnPickup( position + (upO * 1.5f) + (RightO * -2.2f));
            controlButtons.Button_Door = spawnPickup( position + (upO *0.7f) + (RightO * -1f) + (fwd*0.25f));
            controlButtons.Button_Acceleration = spawnPickup( position + (upO * 0.55f) + (RightO * -2));

            SpawnSeat(position + (upO * 0.3f) + (Vector3.back * 1.5f)  + (RightO * -1.9f));

            Primitive SteeringWheel = Primitive.Create(position+ (upO * 1.5f) + (RightO * -1.3f) + (RightO * -0.5f) + (RightO * -0.125f));
            SteeringWheel.Scale = new Vector3(0.5f, 0.5f, 0.1f);
            SteeringWheel.Type = PrimitiveType.Cube;
            SteeringWheel.Color = Color.black;
        }
        Primitive SpeedDialHand;
        public void SpawnSpeedDial(Vector3 position)
        {
            Primitive SpeedDialBG = Primitive.Create(position);
            SpeedDialBG.Type = PrimitiveType.Cylinder;
            SpeedDialBG.Scale = new Vector3(0.5f, 0.1f, 0.1f);
            SpeedDialBG.Color = Color.black;

            SpeedDialHand = Primitive.Create(position);
            SpeedDialHand.Type = PrimitiveType.Cube;
            SpeedDialHand.Color = Color.red;
            SpeedDialHand.Scale = new Vector3(0.5f, 0.1f, 0.1f);
        }
        public static UnityEngine.Color GetColorFromString(string colorText)
        {
            UnityEngine.Color color = new UnityEngine.Color(-1f, -1f, -1f);
            string[] charTab = colorText.Split(':');

            if (charTab.Length >= 4)
            {
                if (float.TryParse(charTab[0], out float red))
                    color.r = red / 255f;

                if (float.TryParse(charTab[1], out float green))
                    color.g = green / 255f;

                if (float.TryParse(charTab[2], out float blue))
                    color.b = blue / 255f;

                if (float.TryParse(charTab[3], out float alpha))
                    color.a = alpha;

                return color != new UnityEngine.Color(-1f, -1f, -1f) ? color : UnityEngine.Color.magenta * 3f;
            }

            if (colorText[0] != '#' && colorText.Length == 8)
                colorText = '#' + colorText;

            return ColorUtility.TryParseHtmlString(colorText, out color) ? color : UnityEngine.Color.magenta * 3f;
        }

        public void SpawnSeat(Vector3 position)
        {
            Primitive SeatBack = Primitive.Create(position + (Vector3.up * 0.725f), scale: new Vector3(1, 1.5f, 0.2f));
            SeatBack.Type = PrimitiveType.Cube;
            SeatBack.Color = Color.red;

            Primitive SeatBottom = Primitive.Create(position + (Vector3.up * 0.1f) + (Vector3.forward * 0.25f), scale: new Vector3(1, 0.2f, 0.5f));
            SeatBottom.Type = PrimitiveType.Cube;
            SeatBottom.Color = Color.red;
        }

        public void SpawnModel(Vector3 position)
        {

            Exiled.API.Features.Toys.Light sunLight = Exiled.API.Features.Toys.Light.Create();
            sunLight.Position = position + (Vector3.up * 10f); //+ (Vector3.back * 50) + (Vector3.forward * 10 * x) ;
            sunLight.Intensity = 1000f;
            sunLight.Color = Color.white;
            sunLight.Range = 10000f;
            sunLight.Spawn();
            Position = position;
            VirtualPosition = position;
            
            SpawnPosition = position + Vector3.up * 1.5f;
          
            road.spawnModel(position + Vector3.down * 1.25f);

            Vector3 floorScale = (Vector3.forward * 10) + (Vector3.right * 5) + (Vector3.up * 0.5f);

            Primitive SteeringWheelBack = Primitive.Create(position + (Vector3.left * 2) + (Vector3.forward * 4.85f) + (Vector3.up * 0.6f), scale: new Vector3(1, 1.2f, 0.225f));
            SteeringWheelBack.Type = PrimitiveType.Cube;
            SteeringWheelBack.Color = Color.red;

            Primitive Console = Primitive.Create(position + (Vector3.up * 0.5f) + (Vector3.forward * 4.85f), scale: new Vector3(5, 0.45f, 0.25f));
            Console.Type = PrimitiveType.Cube;
            Console.Color = Color.red;

            SpawnButtons(position + (Vector3.forward * 4.5f));
            Primitive floorTemp = Primitive.Create(position, scale: floorScale) ;
            floorTemp.Type = PrimitiveType.Cube;
            floorTemp.Color = Color.red;

            Primitive roofTemp = Primitive.Create(position + Vector3.up * 3f, scale: floorScale);
            roofTemp.Type = PrimitiveType.Cube;
            roofTemp.Color = Color.red;

            for (int x =0 ; x < 6; x++)
            {
                SpawnSeat(position + (Vector3.up * 0.3f) + (Vector3.right * 1.5f) + (Vector3.forward * 1.5f) + (Vector3.back * (x * 1.25f)));
                SpawnSeat(position + (Vector3.up * 0.3f) + (Vector3.left  * 1.5f) + (Vector3.forward * 1.5f) + (Vector3.back * (x * 1.25f)));
            }

            Exiled.API.Features.Toys.Light interiorLight = Exiled.API.Features.Toys.Light.Create();
            interiorLight.Position = position + (Vector3.up * 2.8f); //+ (Vector3.back * 50) + (Vector3.forward * 10 * x) ;
            interiorLight.Intensity = 10;
            interiorLight.Color = Color.white;
            interiorLight.Range = 5;
            interiorLight.Spawn();

            Exiled.API.Features.Toys.Light interiorLight2 = Exiled.API.Features.Toys.Light.Create();
            interiorLight2.Position = position + ( Vector3.forward * 4f)+  (Vector3.up * 2.8f); //+ (Vector3.back * 50) + (Vector3.forward * 10 * x) ;
            interiorLight2.Intensity = 10;
            interiorLight2.Color = Color.white;
            interiorLight2.Range = 5;
            interiorLight2.Spawn();

            Exiled.API.Features.Toys.Light interiorLight3 = Exiled.API.Features.Toys.Light.Create();
            interiorLight3.Position = position + (Vector3.forward * -4f) + (Vector3.up * 2.8f); //+ (Vector3.back * 50) + (Vector3.forward * 10 * x) ;
            interiorLight3.Intensity = 10;
            interiorLight3.Color = Color.white;
            interiorLight3.Range = 5;
            interiorLight3.Spawn();

            Primitive sideFrontTemp = Primitive.Create(position + (Vector3.forward * 5) + (Vector3.up * 0.5f), scale:new Vector3(5, 1f, 0.1f));
            sideFrontTemp.Type = PrimitiveType.Cube;
            sideFrontTemp.Color = Color.red;

            Primitive sideBackTemp = Primitive.Create(position + (Vector3.forward * -5) + (Vector3.up * 1.5f), scale: new Vector3(5, 3f, 0.1f));
            sideBackTemp.Type = PrimitiveType.Cube;
            sideBackTemp.Color = Color.red;

            Primitive sideFrontTempUp = Primitive.Create(position + (Vector3.forward * 5) + (Vector3.up * 3f), scale: new Vector3(5, 1f, 0.1f)); ;
            sideFrontTempUp.Type = PrimitiveType.Cube;
            sideFrontTempUp.Color = Color.red;

            /*Primitive sideBackTempUp = Primitive.Create(position + (Vector3.forward * -5) + (Vector3.up * 3f), scale: new Vector3(5, 1f, 0.1f));
            sideBackTempUp.Type = PrimitiveType.Cube;
            sideBackTempUp.Color = Color.red;*/

            Primitive sideFrontTempGlass = Primitive.Create(position + (Vector3.forward * 5) + (Vector3.up * 2f), scale: new Vector3(5, 2f, 0.1f)); ;
            sideFrontTempGlass.Type = PrimitiveType.Cube;
            sideFrontTempGlass.Color = GetColorFromString("0.5:0.5:1:0.2");

            /*Primitive sideBackTempGlass = Primitive.Create(position + (Vector3.forward * -5) + (Vector3.up * 2f), scale: new Vector3(5, 2f, 0.1f));
            sideBackTempGlass.Type = PrimitiveType.Cube;
            sideBackTempGlass.Color = GetColorFromString("0.5:0.5:1:0.2");*/

            Primitive sideFrontLeftTemp = Primitive.Create(position + (Vector3.left * 2.5f) + (Vector3.up * 0.6f), scale: new Vector3(0.1f, 1.2f, 10));
            sideFrontLeftTemp.Type = PrimitiveType.Cube;
            sideFrontLeftTemp.Color = Color.red;

            Primitive sideFrontLeftTempUp = Primitive.Create(position + (Vector3.left * 2.5f) + (Vector3.up * 3f), scale: new Vector3(0.1f, 1f, 10)); ;
            sideFrontLeftTempUp.Type = PrimitiveType.Cube;
            sideFrontLeftTempUp.Color = Color.red;

            Primitive sideFrontLeftTempGlass = Primitive.Create(position + (Vector3.left * 2.5f) + (Vector3.up * 2f), scale: new Vector3(0.1f, 2f, 10)); ;
            sideFrontLeftTempGlass.Type = PrimitiveType.Cube;
            sideFrontLeftTempGlass.Color = GetColorFromString("0.5:0.5:1:0.2");


            Primitive sideFrontRightTemp = Primitive.Create(position +  (Vector3.back * 1f) + (Vector3.right * 2.5f) + (Vector3.up * 0.6f), scale: new Vector3(0.1f, 1.2f, 8));
            sideFrontRightTemp.Type = PrimitiveType.Cube;
            sideFrontRightTemp.Color = Color.red;

            Primitive sideFrontRightTempUp = Primitive.Create(position + (Vector3.right * 2.5f) + (Vector3.up * 3f), scale: new Vector3(0.1f, 1f, 10));//Primitive.Create(position + (Vector3.back * 1f) + (Vector3.right * 2.5f) + (Vector3.up * 3f), scale: new Vector3(0.1f, 1f, 8)); ;
            sideFrontRightTempUp.Type = PrimitiveType.Cube;
            sideFrontRightTempUp.Color = Color.red;

            Primitive sideFrontRightTempGlass =  Primitive.Create(position + (Vector3.back * 1f) + (Vector3.right * 2.5f) + (Vector3.up * 2f), scale: new Vector3(0.1f, 2f, 8)); ;
            sideFrontRightTempGlass.Type = PrimitiveType.Cube;
            sideFrontRightTempGlass.Color = GetColorFromString("0.5:0.5:1:0.2");

            Primitive sideFrontRightTemp2 = Primitive.Create(position + (Vector3.forward * 4.5f) + (Vector3.right * 2.5f) + (Vector3.up * 0.6f), scale: new Vector3(0.1f, 1.2f, 1));
            sideFrontRightTemp2.Type = PrimitiveType.Cube;
            sideFrontRightTemp2.Color = Color.red;

           /* Primitive sideFrontRightTempUp2 = Primitive.Create(position + (Vector3.forward * 4.5f) + (Vector3.right * 2.5f) + (Vector3.up * 3f), scale: new Vector3(0.1f, 1f, 1)); ;
            sideFrontRightTempUp2.Type = PrimitiveType.Cube;
            sideFrontRightTempUp2.Color = Color.red;*/

            Primitive sideFrontRightTempGlass2 = Primitive.Create(position + (Vector3.forward * 4.5f) + (Vector3.right * 2.5f) + (Vector3.up * 2f), scale: new Vector3(0.1f, 2f, 1)); ;
            sideFrontRightTempGlass2.Type = PrimitiveType.Cube;
            sideFrontRightTempGlass2.Color = GetColorFromString("0.5:0.5:1:0.2");


            DoorObject = new List<Primitive>();

            Primitive DoorCenter = Primitive.Create(position + (Vector3.forward * 3.5f) + (Vector3.right * 2.5f) + (Vector3.up * 1f), scale: new Vector3(0.1f, 3f, 0.15f));
            DoorCenter.Type = PrimitiveType.Cube;
            DoorCenter.Color = Color.grey;

            Primitive DoorCenterGlass = Primitive.Create(position + (Vector3.forward * 3.5f) + (Vector3.right * 2.5f) + (Vector3.up * 1.5f), scale: new Vector3(0.1f, 3f, 1));
            DoorCenterGlass.Type = PrimitiveType.Cube;
            DoorCenterGlass.Color = GetColorFromString("0.5:0.5:1:0.2");

            /*Primitive DoorUp = Primitive.Create(position + (Vector3.forward * 3.5f) + (Vector3.right * 2.5f) + (Vector3.up * 3f), scale: new Vector3(0.1f, 0.1f, 1));
            DoorUp.Type = PrimitiveType.Cube;
            DoorUp.Color = Color.grey;

            Primitive DoorDown = Primitive.Create(position + (Vector3.forward * 3.5f) + (Vector3.right * 2.5f) + (Vector3.up * 0.6f), scale: new Vector3(0.1f, 0.1f, 1));
            DoorDown.Type = PrimitiveType.Cube;
            DoorDown.Color = Color.grey;*/

            Primitive DoorLeft = Primitive.Create(position + (Vector3.forward * (3 + 0.1f)) + (Vector3.right * 2.5f) + (Vector3.up * 1), scale: new Vector3(0.1f, 3, 0.2f));
            DoorLeft.Type = PrimitiveType.Cube;
            DoorLeft.Color = Color.grey;
            Primitive DoorRight = Primitive.Create(position + (Vector3.forward * (4 - 0.1f)) + (Vector3.right * 2.5f) + (Vector3.up * 1), scale: new Vector3(0.1f,3, 0.2f));
            DoorRight.Type = PrimitiveType.Cube;
            DoorRight.Color = Color.grey;

            DoorObject.Add(DoorCenter);
            //DoorObject.Add(DoorDown);
            //DoorObject.Add(DoorUp);
            DoorObject.Add(DoorLeft);
            DoorObject.Add(DoorRight);
            DoorObject.Add(DoorCenterGlass);

        }

        public void ToggleDoor()
        {
            DoorToggle = !DoorToggle;
            if (DoorToggle)
            {
                foreach (Primitive p in DoorObject)
                {
                    p.Collidable = true;
                    p.Spawn();
                }
            }
            else
            {
                foreach (Primitive p in DoorObject)
                {
                    p.Collidable = false;
                    p.UnSpawn();
                }
            }

        }
        public float steering = 0f;
        public float lastSteer = 0f;
        public System.Random rnd = new System.Random();
        public void SteerLeft()
        {
            // VirtualPosition = VirtualPosition + (Vector3.left * 5);
            steering = steering + 0.1f; //- ((speed / Plugin.Instance.Config.busConfig.maxSpeed) / 10); //- 0.1f;
                                        //VirtualEulerAngles = VirtualEulerAngles + new Vector3(0,-5,0);
            lastSteer = Time.time + Plugin.Instance.Config.busConfig.AFKSwerveTime;

        }
        public void SteerRight()
        {
            //VirtualPosition = VirtualPosition + (Vector3.right * 5);
            steering = steering - 0.1f; //+ ((speed / Plugin.Instance.Config.busConfig.maxSpeed) / 10); //+ 0.1f;
            lastSteer = Time.time +Plugin.Instance.Config.busConfig.AFKSwerveTime;
            //VirtualEulerAngles = VirtualEulerAngles + new Vector3(0, 5, 0);
        }
        public void Accelerate()
        {
            if (speed <= 0)
            {
                lastSteer = (Time.time + Plugin.Instance.Config.busConfig.AFKSwerveTime * 2);
            }
            speed = speed + Plugin.Instance.Config.busConfig.accelerationSpeed;
            if (speed > Plugin.Instance.Config.busConfig.maxSpeed)
                speed = Plugin.Instance.Config.busConfig.maxSpeed;
        }
        public float roadBoundry = 5f;
        public bool isOnSideOfRoad()
        {
            if (VirtualPosition.x < -1 * roadBoundry)
                return true;
            if (VirtualPosition.x > roadBoundry)
                return true;
            return false;
        }
        public void Update()
        {
            if (steering != 0)
            {
                int d = 1;
                if (steering < 0)
                    d = -1;
                steering = steering - (d * 0.005f);
                if (Math.Abs(steering) <= 0.005f)
                {
                    steering = 0;
                }

            }
            if (speed == 0)
                return;
            Distance = Distance + (speed / 3.6f);


            float friction = 1;
            if (isOnSideOfRoad())
                friction = 10f;
            speed = speed - (Plugin.Instance.Config.busConfig.deccelerationSpeed * friction);
            if (speed > Plugin.Instance.Config.busConfig.maxSpeed)
                speed = Plugin.Instance.Config.busConfig.maxSpeed;
            else if (speed < 0)
                speed = 0;

            if (Plugin.Instance.Config.busConfig.AFKSwerve && (Time.time > lastSteer))
            {
                lastSteer = Time.time + (Plugin.Instance.Config.busConfig.AFKSwerveTime * 2f);
                int dir = -1;
                if (rnd.NextDouble() > 0.5f)
                    dir = 1;
                steering = steering + (dir * 0.2f);
            }




            VirtualPosition = VirtualPosition + (Vector3.right * steering * (speed / Plugin.Instance.Config.busConfig.maxSpeed));
            if (VirtualPosition.x < -1 * (roadBoundry + 0.5f))
                VirtualPosition.x = -1 * (roadBoundry + 0.5f);
            if (VirtualPosition.x > roadBoundry + 0.5f)
                VirtualPosition.x = roadBoundry + 0.5f;
            VirtualEulerAngles.y = steering;
            //VirtualEulerAngles = VirtualEulerAngles + new Vector3(0, ((VirtualEulerAngles.y / VirtualEulerAngles.y) * -1), 0);

            road.updatePosition(VirtualPosition, VirtualEulerAngles);
            
        }

        public Bus()
        {
            controlButtons = new BusControls(this);
            road = new Road(this);
        }
    }
}
