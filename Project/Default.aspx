<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Project._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="Style.css">
    <div class="jumbotron">
        <span style="font-size: x-large">Stocks Prices</span></h1>
        <p>
            &nbsp;<p style="font-size: medium">Please enter a stock ticker</p>
            <asp:TextBox ID="StockSearch" runat="server" OnTextChanged="TextBox1_TextChanged" Height="17px" Width="172px"></asp:TextBox>
            <asp:Button ID="SubmitButton" runat="server" Height="23px" OnClick="Button1_Click" Text="Submit" Width="59px" style="font-size: x-small" />
        &nbsp;&nbsp;</p>
            
               <div class="text-left">
            
                   <asp:Literal ID="Literal1" runat="server"></asp:Literal>
            
               </div>
            <div class="text-left">
        <asp:Literal ID="Literal2" runat="server"></asp:Literal>
            </div>
             <div class="text-left">
        <asp:Literal ID="Literal3" runat="server"></asp:Literal>
            </div>
              <div class="text-left">
        <asp:Literal ID="Literal4" runat="server"></asp:Literal>
            </div>
            <div class="text-left">
        <asp:Literal ID="Literal5" runat="server"></asp:Literal>
            </div>
            <div class="text-left">
        <asp:Literal ID="Literal6" runat="server"></asp:Literal>
            </div>
            <div class="text-left">
        <asp:Literal ID="Literal7" runat="server"></asp:Literal>
            </div>


        <div class="col-md-4">
            <h2>&nbsp;</h2>
        </div>
    </div>

</asp:Content>
