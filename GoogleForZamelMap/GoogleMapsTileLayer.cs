using System;
using MapControl;

namespace GoogleForZamelMap
{
    public class GoogleMapsTileLayer : TileLayer
    {
        public GoogleMapsTileLayer()
        {
            MinZoomLevel = 1;
            MaxZoomLevel = 21;

            var random = new Random(0);

            MapControl.TileImageLoader.HttpUserAgent =
                string.Format("Mozilla/5.0 (Windows NT 6.1; WOW64; rv:{0}.0) Gecko/{2}{3:00}{4:00} Firefox/{0}.0.{1}",
                    random.Next(3, 14), random.Next(1, 10),
                    random.Next(DateTime.Today.Year - 4, DateTime.Today.Year), random.Next(12),
                    random.Next(30));

            TileSource = new GoogleMapsTileSource();
        }

    }
}