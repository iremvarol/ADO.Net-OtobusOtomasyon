using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace OtobüsOtomasyon
{
    public class Baglanti
    {
        public static string sqlBaglanti()
        {
            string conn = "Server=DESKTOP-E7DRJIB;Database=OtobusOtomasyon;Trusted_Connection=True;";
            return conn;
        }


    }
}
