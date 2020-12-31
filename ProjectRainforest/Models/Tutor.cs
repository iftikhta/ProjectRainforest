using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectRainforest.Models
{
    public class Tutor
    {
        public int Tutor_id { get; set; }
        public string Tutor_name { get; set; }
        public string Tutor_description { get; set; }
        public string Tutor_Subjects { get; set; }
        public int Tutor_rate { get; set; }
        public string Tutor_img { get; set; }
        public int Tutor_rating  { get; set; }
        public DateTime Tutor_date_joined { get; set; }

    }
}
