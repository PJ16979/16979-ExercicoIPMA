using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public class PrevisaoDia
    {
        public string precipitaProb { get; set; }
        public string tMin { get; set; }
        public string tMax { get; set; }
        public string predWindDir { get; set; }
        public int idWeatherType { get; set; }
        public int classWindSpeed { get; set; }
        public string longitude { get; set; }
        public string forecastDate { get; set; }
        public int classPrecInt { get; set; }
        public string latitude { get; set; }
    }

    public class PrevisaoIPMA
    {
        public string owner { get; set; }
        public string country { get; set; }
        public List<PrevisaoDia> data { get; set; }
        public int globalIdLocal { get; set; }
        public DateTime dataUpdate { get; set; }
        public string Local { get; set; }
    }
}
