﻿using Exiled.API.Features.Toys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Desert_Bus_SCP_SL
{
    public class Road
    {
        public Vector3 startPos = Vector3.zero;
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
        public Bus bus;
        public List<Primitive> stationaryPieces = new List<Primitive>();

        public List<Primitive> whiteLines = new List<Primitive>();

        public int roadLength = 50;
        int progress = 0;

        public List<Primitive> randomFlyByObjects = new List<Primitive>();
        public Primitive CurrentFlybyObject = null;
        public float nextFlyByObject = 0;
        public System.Random rnd = new System.Random();
        public void updatePosition(Vector3 VirtalPosition,float deltaTime) //Vector3 VirtualEularAngles, float deltaTime)
        {
            foreach (Primitive p in stationaryPieces)
            {
                //p.Rotation.eulerAngles.Set(VirtualEularAngles.x, VirtualEularAngles.y, VirtualEularAngles.z);
                p.Position = new Vector3(VirtalPosition.x, p.Position.y, p.Position.z); // VirtalPosition;
                //p.Rotation = p.Rotation;
            }
            if (bus.speed <= 0)
            {
                return;
            }
            int x = 0;
            foreach (Primitive p in whiteLines)
            {
                p.Position = new Vector3(VirtalPosition.x, p.Position.y, p.Position.z);// VirtalPosition.x;
                p.Position = p.Position + (Vector3.back * bus.speed *deltaTime);//(bus.speed / Plugin.Instance.Config.busConfig.maxSpeed) * deltaTime * 5);
                if (Vector3.Distance(p.Position, startPos + (Vector3.forward * 25 * x)) > 50)
                {
                    p.Position = startPos + (Vector3.forward * 25 * x);
                    p.Position = new Vector3(VirtalPosition.x, p.Position.y, p.Position.z);
                }
                x++;
            }
            /*if (CurrentFlybyObject != null)
            {
                CurrentFlybyObject.Position = new Vector3(VirtalPosition.x, CurrentFlybyObject.Position.y, CurrentFlybyObject.Position.z);
                CurrentFlybyObject.Position += Vector3.right * -10;
                CurrentFlybyObject.Position = CurrentFlybyObject.Position + (Vector3.back * bus.speed * deltaTime);
                if (Vector3.Distance(CurrentFlybyObject.Position, startPos) > 50)
                {
                    CurrentFlybyObject.Position = Vector3.zero;
                    CurrentFlybyObject = null;
                    nextFlyByObject = rnd.Next(200, 1000);
                }

            }
            else
            {
                nextFlyByObject -= bus.speed * deltaTime;
                if (nextFlyByObject <= 0)
                {
                    CurrentFlybyObject = randomFlyByObjects.RandomItem();
                    CurrentFlybyObject.Position = startPos + Vector3.right * -10;
                }
            }*/
        }
        public void spawnModel(Vector3 Position)
        {

            Vector3 roadScale = (Vector3.forward * 1000f) + (Vector3.up * 0.1f) + (Vector3.right * 10f);

            Primitive stationaryDesert = Primitive.Create(Position, scale: new Vector3(1000, 0.1f, 1000));
            stationaryDesert.Color = GetColorFromString("#ffd561");
            stationaryDesert.Type = PrimitiveType.Cube;

            Primitive stationaryRoad = Primitive.Create(Position + (Vector3.up * 0.1f), scale: roadScale);
            stationaryRoad.Color = Color.gray;
            stationaryRoad.Type = PrimitiveType.Cube;


            Primitive stationaryDesert2 = Primitive.Create(Position, scale: roadScale*1.5f);
            stationaryDesert2.Color = GetColorFromString("#735c1d");
            stationaryDesert2.Type = PrimitiveType.Cube;
            //stationaryPieces.Add(stationaryDesert);
            stationaryPieces.Add(stationaryRoad);
            stationaryPieces.Add(stationaryDesert2);
            //Vector3 whiteScale = (Vector3.forward * 5) + (Vector3.up * 0.15f) + (Vector3.right * 1f);
            Vector3 whiteScale = (Vector3.forward * 5) + (Vector3.up * 0.15f) + (Vector3.right * 1f);
            startPos = Position + (Vector3.up * 0.15f); //+ (Vector3.back * (roadLength/2)) + (Vector3.up * 0.15f);
            for (int x = 0; x < 5; x++)
            {
                Primitive whiteLine = Primitive.Create(startPos + (Vector3.forward * 25 * x), scale: whiteScale);
                whiteLine.Color = Color.yellow;
                whiteLine.Type = PrimitiveType.Cube;
                whiteLines.Add(whiteLine);
            }
           // Primitive rock = Primitive.Create(PrimitiveType.Cube, Vector3.zero, scale: Vector3.one * 0.5f);
           // rock.Color = GetColorFromString("#d9a036");
           // randomFlyByObjects.Add(rock);

        }
        public Road(Bus b)
        {
            bus = b;
        }
    }
}
