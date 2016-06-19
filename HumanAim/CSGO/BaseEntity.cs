﻿using HumanAim.CSGO.Enum;
using System.Linq;
using System;
using HumanAim.CSGO.Structs;
using System.Runtime.InteropServices;

namespace HumanAim.CSGO
{
    internal class BaseEntity
    {
        private byte[] readData;
        private int address;

        public BaseEntity(int address)
        {
            this.address = address;
        }

        public void ClearCache()
        {
            readData = new byte[0];
        }

        public void Update()
        {
            readData = HumanAim.Memory.ReadMemory(address, HumanAim.NetVars.Values.Max() + 12); // Memory.ReadMemory(address, 0x3150);
        }

        public int Address
        {
            get
            {
                return address;
            }
            set
            {
                address = value;
            }
        }

        public int GetHealth()
        {
            return BitConverter.ToInt32(readData, HumanAim.NetVars["m_iHealth"]);
        }

        public Team GetTeam()
        {
            return (Team)BitConverter.ToInt32(readData, HumanAim.NetVars["m_iTeamNum"]);
        }

        public int GetIndex()
        {
            return BitConverter.ToInt32(readData, HumanAim.NetVars["m_dwIndex"]);
        }
        public Vector3D GetPosition()
        {
            byte[] vecData = new byte[12];
            Buffer.BlockCopy(readData, HumanAim.NetVars["m_vecOrigin"], vecData, 0, 12);
            return MemorySystem.MemoryScanner.GetStructure<Vector3D>(vecData);
        }

        public Vector3D GetPunchAngle()
        {
            byte[] vecData = new byte[12];
            Buffer.BlockCopy(readData, HumanAim.NetVars["m_aimPunchAngle"], vecData, 0, 12);
            return MemorySystem.MemoryScanner.GetStructure<Vector3D>(vecData);
        }

        public float GetViewOffset()
        {
            return BitConverter.ToSingle(readData, HumanAim.NetVars["m_vecViewOffset[2]"]);
        }

        public bool IsDormant()
        {
            return BitConverter.ToBoolean(readData, HumanAim.NetVars["m_bDormant"]);
        }

        public int GetBoneMatrix()
        {
            return BitConverter.ToInt32(readData, HumanAim.NetVars["m_dwBoneMatrix"]);
        }

        public Vector3D GetBonesPos(int boneId)
        {
            var size = Marshal.SizeOf(typeof(BaseBone));
            return HumanAim.Memory.Read<BaseBone>(GetBoneMatrix() + (size * boneId)).ToVector3D();
        }

        public Vector3D GetViewAngle()
        {
            return EngineClient.ViewAngle;
        }
    }
}
