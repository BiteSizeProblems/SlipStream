using SlipStream.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SlipStream.Structs.Appendeces;

namespace SlipStream.Core.Utils
{
    public static class CarStatusUtils
    {
        /// <summary>
        /// Get how many cars are on each tire
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int GetActiveTireCount(CarStatusData[] data, VisualTireCompounds type)
        {
            // Count how many drivers are on each tire compound.
            int count = 0;

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].m_visualTyreCompound == type)
                {
                    count += 1;
                }
            }
            return count;
        }

        /// <summary>
        /// Get how many cars are on each tire
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Dictionary<VisualTireCompounds, int> GetActiveTireCounts(CarStatusData[] data)
        {
            Dictionary<VisualTireCompounds, int> result = new Dictionary<VisualTireCompounds, int>();

            for (int i = 0;i < data.Length; i++)
            {
                foreach(VisualTireCompounds tirecompound in Enum.GetValues(typeof(VisualTireCompounds)))
                {
                    if(data[i].m_visualTyreCompound == tirecompound)
                    {
                        if (result.ContainsKey(tirecompound))
                        {
                            result[tirecompound] = result[tirecompound] + 1;
                        }
                        else { result.Add(tirecompound, 1); }
                    }
                }
            }
            return result;
        }

    }
}
