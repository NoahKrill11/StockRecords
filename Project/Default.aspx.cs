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
    public partial class _Default : Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
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
        private int CheckName(ref String NameOfCompany)
        {
            SqlConnectionStringBuilder builder = call();
            int num;
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                String sql = "SELECT COUNT(Stock) FROM Stocks_Today WHERE Stock = @Stock ";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@Stock", NameOfCompany);
                    num = (int)cmd.ExecuteScalar();
                }
                if (num > 0)
                {
                    return 1;
                }
                return 0;
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
            Literal5.Text = "Barchart says that this stock is a " + ticker.ToString();
            DateTime today = DateTime.Today;
            double DBPrice = GetPriceFromDB(ref name);
            Literal7.Text = "";
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
                    Literal2.Text = "The cost of the stock is " + cost;
                    Literal3.Text = "The low cost of the stock is " + cost;
                    Literal4.Text = "The high cost of the stock is " + High.ToString();
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
                    Literal2.Text = "The cost of the stock is " + cost;
                    Literal3.Text = "The low cost of the stock is " + Low.ToString();
                    Literal4.Text = "The high cost of the stock is " + cost;
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
                    Literal2.Text = "The cost of the stock is " + cost;
                    Literal3.Text = "The low cost of the stock is " + Low.ToString();
                    Literal4.Text = "The high cost of the stock is " + High.ToString();
                }
                connection.Close();
            }

        }
        void addNewStock(ref String Name, ref String Cost, ref String ticker, ref String daily)
        {
            String val = BuyStat(ref ticker);
            Literal5.Text = "Barchart says that this stock is a " + val.ToString();
            DateTime today = DateTime.Today;
            SqlConnectionStringBuilder builder = call();
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                String sql2 = "INSERT into Stocks_Today (Stock, Cost, Low, High, Rating, DailyVolume) VALUES (@Stock, @Cost, @Low, @High, @Rating, @DailyVolume)";
                using (SqlCommand command = new SqlCommand(sql2, connection))
                {
                    command.Parameters.AddWithValue("@Stock", Name);
                    command.Parameters.AddWithValue("@Cost", Cost);
                    command.Parameters.AddWithValue("@Low", Cost);
                    command.Parameters.AddWithValue("@High", Cost);
                    command.Parameters.AddWithValue("@Rating", val);
                    command.Parameters.AddWithValue("@DailyVolume", daily);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            Literal2.Text = "The cost of the stock is " + Cost;
            Literal3.Text = "The low cost of the stock is " + Cost;
            Literal4.Text = "The high cost of the stock is " + Cost;
            Literal7.Text = "This is the first time this stock has been entered into the database";
        }
        String volume()
        {
            StringBuilder URL = new StringBuilder();
            URL.Append("https://money.cnn.com/quote/quote.html?symb=");
            var stockName = StockSearch.Text;
            URL.Append(stockName);
            WebClient client = new WebClient();
            string content = client.DownloadString(URL.ToString());
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(content);
            var volume = htmlDocument.DocumentNode.Descendants("td").Where(node => node.GetAttributeValue("class", "").Equals("wsod_quoteDataPoint")).ToList();
            Literal6.Text = StockSearch.Text.ToUpper() + " has a volume today of " + volume[3].InnerText;
            return volume[3].InnerText;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            StringBuilder URL = new StringBuilder();
            URL.Append("https://finance.yahoo.com/quote/");
            var stockName = StockSearch.Text;
            URL.Append(stockName);
            WebClient client = new WebClient();
            string content = client.DownloadString(URL.ToString());
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(content);
            var price = htmlDocument.DocumentNode.Descendants("span").Where(node => node.GetAttributeValue("class", "").Equals("Trsdu(0.3s) Fw(b) Fz(36px) Mb(-4px) D(ib)")).ToList();
            var findStockName = htmlDocument.DocumentNode.Descendants("h1").Where(node => node.GetAttributeValue("class", "").Equals("D(ib) Fz(18px)")).ToList();
            if (price.Count > 0 && findStockName.Count > 0)
            {
                var price3 = price[0].InnerText;
                var NameOfCom = findStockName[0].InnerText;
                int count = CheckName(ref NameOfCom);
                Literal1.Text = NameOfCom;
                String daily = volume();
                if (price3 != "")
                {
                    if (count == 0)
                    {
                        addNewStock(ref NameOfCom, ref price3, ref stockName, ref daily);
                    }
                    else
                    {
                        updateTable(ref NameOfCom, ref price3, ref stockName, ref daily);
                    }
                }
            }
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
                try
                {
                    var rating = htmlDocument2.DocumentNode.Descendants("a").Where(node => node.GetAttributeValue("class", "").Equals("sell-color")).ToList();
                    return rating[0].InnerText;
                }
                catch (Exception)
                {
                    var rating = htmlDocument2.DocumentNode.Descendants("a").Where(node => node.GetAttributeValue("class", "").Equals("hold-color")).ToList();
                    return rating[0].InnerText;
                }
            }

        }
    }
        
}
       