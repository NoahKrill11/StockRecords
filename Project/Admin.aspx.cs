using System;
using System.Web.UI;
using HtmlAgilityPack;
using System.Text;
using System.Net;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace Project
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private SqlConnectionStringBuilder call()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "stockrecords.database.windows.net";
            builder.UserID = "noahkrill";
            builder.Password = "Uakron2019";
            builder.InitialCatalog = "Stocks";
            return builder;
        }
        String BuyStat(ref String name)
        {
            StringBuilder URL = new StringBuilder();
            URL.Append("https://www.barchart.com/stocks/quotes/");
            URL.Append(name);
            URL.Append("/overview");
            WebClient client2 = new WebClient();
            string content2 = client2.DownloadString(URL.ToString());
            var htmlDocument2 = new HtmlDocument();
            htmlDocument2.LoadHtml(content2);
            try
            {
                var rating = htmlDocument2.DocumentNode.Descendants("a").Where(node => node.GetAttributeValue("class", "").Equals("buy-color")).ToList();
                return rating[0].InnerText;
            }
            catch (Exception)
            {
                var rating = htmlDocument2.DocumentNode.Descendants("a").Where(node => node.GetAttributeValue("class", "").Equals("sell-color")).ToList();
                return rating[0].InnerText;
            }

        }
        double GetPriceFromDB(ref String NameOfCompany)
        {
            SqlConnectionStringBuilder builder = call();
            double num = 0;
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                String sql = "SELECT Cost FROM Stocks_Today WHERE Stock = @Stock ";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@Stock", NameOfCompany);
                    num = (double)cmd.ExecuteScalar();
                }
                connection.Close();
            }
            return num;
        }
        double GetLowPriceFromDB(ref String NameOfCompany)
        {
            SqlConnectionStringBuilder builder = call();
            double num = 0;
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                String sql = "SELECT Low FROM Stocks_Today WHERE Stock = @Stock ";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@Stock", NameOfCompany);
                    num = (double)cmd.ExecuteScalar();
                }
                connection.Close();
            }
            return num;
        }
        double GetHighPriceFromDB(ref String NameOfCompany)
        {
            SqlConnectionStringBuilder builder = call();
            double num = 0;
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                String sql = "SELECT High FROM Stocks_Today WHERE Stock = @Stock ";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@Stock", NameOfCompany);
                    num = (double)cmd.ExecuteScalar();
                }
                connection.Close();
            }
            return num;
        }
       
            void updateTable(ref String name, ref String cost, ref String ticker, ref String daily)
            {
                ticker = BuyStat(ref ticker);
                DateTime today = DateTime.Today;
                double DBPrice = GetPriceFromDB(ref name);
                double Low = GetLowPriceFromDB(ref name);
                double High = GetHighPriceFromDB(ref name);
                SqlConnectionStringBuilder builder = call();
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    if (DBPrice > Convert.ToDouble(cost) && Low > Convert.ToDouble(cost))
                    {
                        String sql = "UPDATE Stocks_Today SET Cost = @CurrentPrice, Low = @Low, Rating=@Rating, DailyVolume=@DailyVolume WHERE Stock = @Stock";
                        using (SqlCommand cmd = new SqlCommand(sql, connection))
                        {
                            cmd.Parameters.AddWithValue("@CurrentPrice", cost);
                            cmd.Parameters.AddWithValue("@Stock", name);
                            cmd.Parameters.AddWithValue("@Low", cost);
                            cmd.Parameters.AddWithValue("@Rating", ticker);
                            cmd.Parameters.AddWithValue("DailyVolume", daily);
                            cmd.ExecuteNonQuery();
                        }
                     
                    }
                    else if (DBPrice < Convert.ToDouble(cost) && High < Convert.ToDouble(cost))
                    {
                        String sql = "UPDATE Stocks_Today SET Cost = @CurrentPrice, High = @High , Rating=@Rating, DailyVolume=@DailyVolume WHERE Stock = @Stock";
                        using (SqlCommand cmd = new SqlCommand(sql, connection))
                        {
                            cmd.Parameters.AddWithValue("@CurrentPrice", cost);
                            cmd.Parameters.AddWithValue("@Stock", name);
                            cmd.Parameters.AddWithValue("@High", cost);
                            cmd.Parameters.AddWithValue("@Rating", ticker);
                            cmd.Parameters.AddWithValue("DailyVolume", daily);

                            cmd.ExecuteNonQuery();

                        }
                    }
                    else
                    {
                        String sql = "UPDATE Stocks_Today SET Cost = @CurrentPrice, Rating=@Rating, DailyVolume=@DailyVolume  WHERE Stock = @Stock";
                        using (SqlCommand cmd = new SqlCommand(sql, connection))
                        {
                            cmd.Parameters.AddWithValue("@CurrentPrice", cost);
                            cmd.Parameters.AddWithValue("@Stock", name);
                            cmd.Parameters.AddWithValue("@Rating", ticker);
                            cmd.Parameters.AddWithValue("DailyVolume", daily);

                            cmd.ExecuteNonQuery();
                        }
                       
                    }
                    connection.Close();
                }


            }
            String volume(ref String name)
        {
            StringBuilder URL = new StringBuilder();
            URL.Append("https://money.cnn.com/quote/quote.html?symb=");
            URL.Append(name);
            WebClient client = new WebClient();
            string content = client.DownloadString(URL.ToString());
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(content);
            var volume = htmlDocument.DocumentNode.Descendants("td").Where(node => node.GetAttributeValue("class", "").Equals("wsod_quoteDataPoint")).ToList();
            return volume[3].InnerText;
        }
        void updateTable2(ref String name)
        {
            StringBuilder URL = new StringBuilder();
            URL.Append("https://finance.yahoo.com/quote/");
            URL.Append(name);
            WebClient client = new WebClient();
            string content = client.DownloadString(URL.ToString());
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(content);
            var price = htmlDocument.DocumentNode.Descendants("span").Where(node => node.GetAttributeValue("class", "").Equals("Trsdu(0.3s) Fw(b) Fz(36px) Mb(-4px) D(ib)")).ToList();
            var findStockName = htmlDocument.DocumentNode.Descendants("h1").Where(node => node.GetAttributeValue("class", "").Equals("D(ib) Fz(18px)")).ToList();
            String vol = volume(ref name);
            if (price.Count > 0 && findStockName.Count > 0)
            {
                var price3 = price[0].InnerText;
                var NameOfCom = findStockName[0].InnerText;
                updateTable(ref NameOfCom, ref price3, ref name, ref vol);

            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            SqlConnectionStringBuilder builder = call();
            String sb = "SELECT Stock FROM Stocks_Today", ticker = "";
            List<String> result = new List<String>();
            int begin = 0, end = 0, pos = 0;
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(sb, connection))
                {
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(reader.GetString(0));
                        }
                        reader.Close();
                    }
                }
                connection.Close();
            }

            foreach (String name in result)
            {
                begin = name.IndexOf("(") + 1;
                pos = name.IndexOf(')');
                end = pos - begin;
                ticker = name.Substring(begin, end);
                updateTable2(ref ticker);
            }
        }
    }
}