using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFramework
{
    public class MathEx
    {
        public static string GetMedian(int[] datas)
        {
            string median;
            if (datas.Length == 0) median = string.Empty;
            else if (datas.Length == 1) median = datas[0].ToString();
            else if (datas.Length % 2 == 0)
            {
                int index = datas.Length / 2;
                decimal value = Convert.ToDecimal((datas[index - 1] + datas[index])) / 2;
                median = string.Format("({0}+{1})/2={2}", datas[index - 1], datas[index], value);
            }
            else
            {
                median = datas[datas.Length / 2].ToString();
            }
            return median;
        }
        public static string GetMedian(decimal[] datas)
        {
            string median;
            if (datas.Length == 0) median = string.Empty;
            else if (datas.Length == 1) median = datas[0].ToString();
            else if (datas.Length % 2 == 0)
            {
                int index = datas.Length / 2;
                decimal value = (datas[index - 1] + datas[index]) / 2;
                median = string.Format("({0}+{1})/2={2}", datas[index - 1], datas[index], value);
            }
            else
            {
                median = datas[datas.Length / 2].ToString();
            }
            return median;
        }
    }
}
