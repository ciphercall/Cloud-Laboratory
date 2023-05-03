using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using AslLabApi.Models;

namespace AslLabApi.DataAccess
{
    public class mydataservice
    {

        public IEnumerable TopcategoriesListdata(Int64 loggedcompid, string todate, string frdate)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AslLabApiContext"].ToString());

            var query = string.Format("SELECT HMS_TESTMST.TCATNM AS CATNM, SUM(HMS_OPD.AMOUNT) AS VALUE " +
                       " FROM  HMS_TEST INNER JOIN " +
                  " HMS_OPD ON HMS_TEST.COMPID = HMS_OPD.COMPID AND HMS_TEST.TESTID = HMS_OPD.TESTID INNER JOIN " +
                "  HMS_TESTMST ON HMS_TEST.TCATID = HMS_TESTMST.TCATID " +
                " WHERE HMS_OPD.COMPID='" + loggedcompid + "' AND HMS_OPD.TRANSDT  BETWEEN '" + todate + "' AND  '" + frdate + "' " +
                " GROUP BY HMS_TESTMST.TCATNM " +
                " ORDER BY VALUE DESC ");

            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);
            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);

            IList<Outputclass> transList = new List<Outputclass>();
            // var GetList = ds.ToList();

            foreach (DataRow row in ds.Rows)
            {
                transList.Add(new Outputclass()
                {
                    PlanName = row["CATNM"].ToString(),
                    PaymentAmount = Convert.ToInt64(row["VALUE"])

                });
            }
            // var list = con.Query<Outputclass>("Usp_Getdata").AsEnumerable();
            // List of type Outputclass which it will return .
            return transList;
        }



        public IEnumerable TopItemsByQtyListdata(Int64 loggedcompid, string todate, string frdate)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AslLabApiContext"].ToString());

            var query = string.Format("SELECT HMS_TEST.TESTNM AS ITEMNM, COUNT(*) QTY " +
                      " FROM  HMS_TEST INNER JOIN " +
                      " HMS_OPD ON HMS_TEST.COMPID = HMS_OPD.COMPID AND HMS_TEST.TESTID = HMS_OPD.TESTID " +
                      " WHERE HMS_OPD.COMPID='" + loggedcompid + "' AND HMS_OPD.TRANSDT  BETWEEN '" + todate + "' AND  '" + frdate + "' " +
                      " GROUP BY HMS_TEST.TESTNM " +
                      " ORDER BY QTY DESC  ");

            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);
            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);

            IList<Outputclass> transList = new List<Outputclass>();
            // var GetList = ds.ToList();

            foreach (DataRow row in ds.Rows)
            {
                transList.Add(new Outputclass()
                {
                    PlanName = row["ITEMNM"].ToString(),
                    PaymentAmount = Convert.ToInt64(row["QTY"])

                });
            }
            // var list = con.Query<Outputclass>("Usp_Getdata").AsEnumerable();
            // List of type Outputclass which it will return .
            return transList;
        }




        public IEnumerable TopItemsByValueListdata(Int64 loggedcompid, string todate, string frdate)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AslLabApiContext"].ToString());

            var query = string.Format("SELECT HMS_TEST.TESTNM AS ITEMNM, SUM(HMS_OPD.AMOUNT) VALUE  " +
                    " FROM HMS_TEST INNER JOIN " +
                   "   HMS_OPD ON HMS_TEST.COMPID = HMS_OPD.COMPID AND HMS_TEST.TESTID = HMS_OPD.TESTID " +
                    " WHERE HMS_OPD.COMPID='" + loggedcompid + "' AND HMS_OPD.TRANSDT  BETWEEN '" + todate + "' AND  '" + frdate + "'" +
                   "   GROUP BY HMS_TEST.TESTNM " +
                    "  ORDER BY VALUE DESC");

            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);
            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);

            IList<Outputclass> transList = new List<Outputclass>();
            // var GetList = ds.ToList();

            foreach (DataRow row in ds.Rows)
            {
                transList.Add(new Outputclass()
                {
                    PlanName = row["ITEMNM"].ToString(),
                    PaymentAmount = Convert.ToInt64(row["VALUE"])

                });
            }
            // var list = con.Query<Outputclass>("Usp_Getdata").AsEnumerable();
            // List of type Outputclass which it will return .
            return transList;
        }




        public IEnumerable CollectionDataListdata(Int64 loggedcompid, string todate, string frdate)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AslLabApiContext"].ToString());

            var query = string.Format(" SELECT TRANSDT, SUM(COLLECT) COLLECT  FROM (SELECT  CONVERT(NVARCHAR(20),TRANSDT,103) AS TRANSDT, SUM(RCVAMT) COLLECT " +
                " FROM HMS_OPDMST " +
                " WHERE COMPID='" + loggedcompid + "' AND TRANSDT  BETWEEN '" + todate + "' AND  '" + frdate + "'  " +
                " GROUP BY TRANSDT UNION SELECT  CONVERT(NVARCHAR(20),TRANSDT,103) AS TRANSDT, SUM(RCVAMT) COLLECT " +
                " FROM HMS_OPDRCV " +
                " WHERE COMPID='" + loggedcompid + "' AND TRANSDT  BETWEEN '" + todate + "' AND  '" + frdate + "'  " +
                " GROUP BY TRANSDT)A GROUP BY TRANSDT");

            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);
            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);

            IList<Outputclass> transList = new List<Outputclass>();
            // var GetList = ds.ToList();

            foreach (DataRow row in ds.Rows)
            {
                transList.Add(new Outputclass()
                {
                    PlanName = row["TRANSDT"].ToString(),
                    PaymentAmount = Convert.ToInt64(row["COLLECT"])

                });
            }
            // var list = con.Query<Outputclass>("Usp_Getdata").AsEnumerable();
            // List of type Outputclass which it will return .
            return transList;
        }



        public IEnumerable TimeWiseSellDataListdata(Int64 loggedcompid, string todate, string frdate)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AslLabApiContext"].ToString());

            var query = string.Format(" SELECT DISTINCT CONVERT(NVARCHAR(20),dateadd(hour, datediff(hour, 0, dateadd(mi, 30, INSTIME)), 0) ,108) AS INSTIME, SUM(NETAMT) AMOUNT " +
                " FROM HMS_OPDMST " +
                " WHERE  COMPID='" + loggedcompid + "' AND TRANSDT  BETWEEN '" + todate + "' AND  '" + frdate + "'" +
               " GROUP BY CONVERT(NVARCHAR(20),dateadd(hour, datediff(hour, 0, dateadd(mi, 30, INSTIME)), 0) ,108)");

            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);
            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);

            IList<Outputclass> transList = new List<Outputclass>();
            // var GetList = ds.ToList();

            foreach (DataRow row in ds.Rows)
            {
                transList.Add(new Outputclass()
                {
                    PlanName = row["INSTIME"].ToString(),
                    PaymentAmount = Convert.ToInt64(row["AMOUNT"])

                });
            }
            // var list = con.Query<Outputclass>("Usp_Getdata").AsEnumerable();
            // List of type Outputclass which it will return .
            return transList;
        }

    }
}