using System;

namespace Booli.API
{
    public static class Utils
    {
        public static float GetDistanceToCityCentre(double lat, double lon)
        {
            var cityCentreLat = 63.825910;
            var cityCentreLong = 20.263166;

            var cityCentrCoord = (cityCentreLat, cityCentreLong);
            var listingCoord = (lat, lon);

            return (float) CalculateDistance(cityCentrCoord, listingCoord);
        }

        private static double CalculateDistance((double lat, double lon) point1, (double lat, double lon) point2)
        {
            var d1 = point1.lat * (Math.PI / 180.0);
            var num1 = point1.lon * (Math.PI / 180.0);
            var d2 = point2.lat * (Math.PI / 180.0);
            var num2 = point2.lon * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }
    }
}