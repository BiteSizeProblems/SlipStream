using SlipStream.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SlipStream.Structs.Appendeces;

namespace SlipStream.Core.Utils
{
    public static class LapDataUtils
    {
        /// <summary>
        /// Get how many cars are left in the race
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int GetActiveCarCount(LapData[] data)
        {
            // Count how many active cars there are
            int carCount = 22;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].resultStatus == ResultStatus.Invalid || data[i].resultStatus == ResultStatus.Inactive)
                {
                    carCount -= 1;
                    continue;
                }
            }
            return carCount;
        }

        /// <summary>
        /// Translates the <see cref="LapData"/> array into a int[] with indexs which point to the position in the UDP arrays
        /// <para> Example [2,4,3,0,1] means LapData[2] is in first, Lapdata[1] is in last... etc</para>
        /// </summary>
        /// <param name="data"></param>
        /// <param name="indexToPosition"></param>
        /// <returns></returns>
        public static void UpdatePositionArray(LapData[] data, ref int[] indexToPosition)
        {
            // Sort the cars in the order that they are on track
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].resultStatus == ResultStatus.Invalid || data[i].resultStatus == ResultStatus.Inactive) continue;

                var trueIndex = data[i].carPosition - 1;
                indexToPosition[trueIndex] = i;
            }
        }

        // GET AVERAGE LAP TIME BY TIRE
        public static float GetAverageTireLaptime(LapData[] data, int num)
        {
            // Count Drivers on tire and calculate average lap time by tire type.
            int count = 0;
            float laptime = 0f;

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].resultStatus == ResultStatus.Active)
                {
                    count += 1;
                    laptime += data[i].lastLapTimeInMS;
                }
            }
            var average = laptime / num;
            return average;
        }
    }
}
