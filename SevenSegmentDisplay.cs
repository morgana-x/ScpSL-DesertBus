using Exiled.API.Features.Toys;
using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Exiled.API.Features;

namespace Desert_Bus_SCP_SL
{
    public class Digit
    {
        public string[] DigitLookUp = new string[10] 
        {
            // 0
            "0000001",
            // 1
            "1111001",
            // 2
            "0100100",
            // 3
            "0110000",
            // 4
            "1011000",
            // 5
            "0010010",
            // 6
            "0000010",
            // 7
            "0111001",
            // 8
            "0000000",
            // 9
            "0010000",
        };
        private static float offsetFactorVertical = 1f;
        private static float offsetFactorHorizontal = 0.25f;
        public List<Vector3> primitivePosLookup = new List<Vector3>()
        {

            // a / 0
            Vector3.up * (offsetFactorVertical + 0.5f),
            // b / 1
            (Vector3.up * (offsetFactorVertical - 0.25f)) + (Vector3.left * 0.75f),
            // c / 2
            (Vector3.down * (offsetFactorVertical - 0.25f)) + (Vector3.left * 0.75f),
            // d / 3
            Vector3.down * (offsetFactorVertical + 0.5f),
            // e / 4
            (Vector3.down * (offsetFactorVertical - 0.25f)) + (Vector3.right * 0.75f),
            // f / 5
            (Vector3.up * (offsetFactorVertical - 0.25f)) + (Vector3.right * 0.75f),
            // g / 6
            Vector3.zero
        };
        public List<Vector3> primitiveScaleLookup = new List<Vector3>()
        {
            // a / 0
            (Vector3.right + (Vector3.up * 0.5f)),
            // b / 1
            (Vector3.up + (Vector3.right * 0.5f)),
            // c / 2
            (Vector3.up + (Vector3.right * 0.5f)),
            // d / 3
            (Vector3.right + (Vector3.up * 0.5f)),
            // e / 4
            (Vector3.up + (Vector3.right * 0.5f)),
            // f / 5
            (Vector3.up + (Vector3.right * 0.5f)),
            // g / 6
           (Vector3.right + (Vector3.up * 0.5f)),
        };
        public char[] visible = new char[7];
        public List<Primitive> primitives = new List<Primitive>();

        public int currentDigit = 0;

        public void setNum(int n)
        {
            if (currentDigit == n)
            {
                return;
            }
            //Exiled.API.Features.Log.Debug("Setting number to " + n);
            currentDigit = n;
            //Log.Debug(n);
            for (int x=6; x >= 0; x--)
            {
                Primitive p = primitives.ElementAt(x);
               // Log.Debug(p);
                if ( DigitLookUp[n][x] == '0')
                {
                    //p.UnSpawn();
                    //p.Color = Color.white;
                    //p.Base.colo
                    if (visible[x] != '0')
                    {
                        p.Color = Color.white;
                       // p.Spawn();
                    }
                    visible[x] = '0';
                    //Exiled.API.Features.Log.Debug("Set color to white!");
                    // p.Base.NetworkMaterialColor = Color.white;
                    //pp.Color = Color.white;
                   // p.Base.MaterialColor = Color.white;
                }
                else
                {
                    //pp.Color = Color.black;
                    //p.Base.MaterialColor = Color.black;
                    //p.Base.NetworkMaterialColor = Color.red;
                    if (visible[x] != '1')
                    {
                        p.Color = Color.black;
                       // p.UnSpawn();
                    }
                    visible[x] = '1';
            
                    //p.Color = Color.black;
                    //p.Spawn();
                }

            }

        }
        public float Scale;
        public void setPos(Vector3 pos)
        {
            for (int x = 0; x < 7; x++)
            {
                Vector3 p = pos + (primitivePosLookup[x] * Scale);
                primitives[x].Position= p;
                primitives[x].Scale = primitiveScaleLookup[x] * Scale;
                //Primitive primitive = Primitive.Create(p, scale: primitiveScaleLookup[x] * Scale);
            }
        }

        public Digit(Vector3 position, float scale = 1f)
        {
            Scale = scale;
            visible = "0000001".ToCharArray();
            for (int x =0; x < 7; x++) 
            {
                Vector3 p = position + (primitivePosLookup[x] * scale);
                Primitive primitive = Primitive.Create( PrimitiveType.Cube, p, scale: (primitiveScaleLookup[x] * scale) + (Vector3.forward * scale * 0.1f));
                if (visible[x] == '0')
                    primitive.Color = Color.white;
                else
                    primitive.Color = Color.black;
                primitives.Insert(x,primitive);
            }
        }
    }
    public class SevenSegmentDisplay // I hate my self!Q
    {
        public List<Digit> Digits = new List<Digit>();
        public SevenSegmentDisplay(Vector3 Position, int digits, float Sscale = 0.1f, bool backGround = true) 
        {
            float width = digits * 3f * Sscale; // 2.5f * Sscale;
            for (int x =0; x < digits; x++) 
            {
                Digit d = new Digit(Position + Vector3.right * ((x * 3f * Sscale) - (width * 0.35f)), scale: Sscale);

                Digits.Add(d);
            }
            if (backGround)
            {
                Primitive bg = Primitive.Create(PrimitiveType.Cube, Position + (Vector3.forward * Sscale * 0.1f), scale: (Vector3.right * width) + (Vector3.forward * Sscale * 0.1f) + (Vector3.up * 3.5f * Sscale));
                bg.Color = Color.black;
            }
        }

        public void setNumber(int n)
        {
            string PaddedResult = n.ToString().PadLeft(3, '0');
            //Exiled.API.Features.Log.Debug(PaddedResult);
            int x = 0;
            foreach (Digit d in Digits)
            {
           
                int digit = Int32.Parse(PaddedResult[x].ToString());
                d.setNum(digit);
                x++;
            }
        }
    }
}
