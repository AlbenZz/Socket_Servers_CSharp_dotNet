using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationProtocolAndUtilities.Utilities.ByteOrder
{

    public interface IByteOrderConverter
    {

    }

    public class ByteOrderConverterForPrimitiveType
    {
        private const int EIGHT = 8;
        public static unsafe byte[] HostToNetworkOrder(double data)
        {
            byte[] netWorkOrder = new byte[EIGHT];
            if (BitConverter.IsLittleEndian)
            {
                for (int i = 0; i < EIGHT; i++)
                {
                    netWorkOrder[i] = (byte)(*((long*)&data) & 0xFFL);
                    *((long *)&data) >>= 8;
                }
            }
            return netWorkOrder;
        }
    }
}
