using static System.Runtime.InteropServices.JavaScript.JSType;
using System;

namespace CommonHelper
{
    public class CANCrcHelper
    {
        /// <summary>
        /// 通过E2E计算CRC
        /// </summary>
        /// <param name="inputData">输入值</param>
        /// <param name="dataId">校验定义的DataId</param>
        /// <returns></returns>
        public static byte[] CalculateCrc(byte[] inputData, byte[] dataId)
        {
            byte[] bytes2 = new byte[inputData.Length];
            for (int i = 1; i < inputData.Length; i++)
            {
                bytes2[i - 1] = inputData[i];
            }
            if (bytes2[0] <= dataId.Length)
            {
                bytes2[7] = dataId[bytes2[0]];
            }
            byte[] bytes = inputData;

            const byte polynomial = 0x2F; // 给定的多项式
            byte crc = 0xFF; // 初始值

            foreach (byte b in bytes2)
            {
                crc ^= b;
                for (int i = 0; i < 8; i++)
                {
                    if ((crc & 0x80) != 0)
                    {
                        crc = (byte)((crc << 1) ^ polynomial);
                    }
                    else
                    {
                        crc <<= 1;
                    }
                }
            }
            crc ^= 0xFF; // 异或值
            bytes[0] = crc;
            return bytes;
        }
    }
}
