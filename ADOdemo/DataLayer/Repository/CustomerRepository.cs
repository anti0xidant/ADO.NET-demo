using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Config;
using DataLayer.Models;

namespace DataLayer.Repository
{
    public class CustomerRepository
    {
        public void DisplayDistincts(string column)
        {
            using (var cn = new SqlConnection(Settings.ConnectionString))
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "SelectDistinct";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@Column", column);

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    Console.Clear();
                    while (dr.Read())
                    {
                        Console.WriteLine("{0}", dr.GetString(0));
                    }
                }
            }
        }

        public List<Customer> GetByStoredProcedure(string storedProcedure, string parameterValue)
        {
            List<Customer> customers = new List<Customer>();

            using (var cn = new SqlConnection(Settings.ConnectionString))
            {
                var cmd = cn.CreateCommand();
                cmd.CommandText = storedProcedure;
                cmd.CommandType = CommandType.StoredProcedure;

                string spVariable = null;
                switch (storedProcedure)
                {
                    case "CustByID":
                        spVariable = "@CustomerID";
                        break;
                    case "CustByCity":
                        spVariable = "@City";
                        break;
                    case "CustByCountry":
                        spVariable = "@Country";
                        break;
                }
                cmd.Parameters.AddWithValue(spVariable, parameterValue);

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    Customer c = null;
                    while (dr.Read())
                    {
                        c = new Customer();

                        c.CustomerID = dr.GetString(0);
                        c.ContactName = dr.GetString(1);
                        c.City = dr.GetString(2);
                        c.Country = dr.GetString(3);

                        customers.Add(c);
                    }
                }
            }

            return customers;
        }

    }
}
