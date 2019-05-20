using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TABAS_API.DataObjects
{
    public class BaggageDAO
    {
        public int user_id;
        public int color_id;
        public double weight;
        public Decimal cost;

        public BaggageDAO(int u_id, int c_id, double w, Decimal c)
        {
            user_id = u_id;
            color_id = c_id;
            weight = w;
            cost = c;
        }
    }
}