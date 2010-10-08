<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SolrCodeCamp.BaseballWeb.Models.BaseballView>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <h2>
        Baseball Stats Navigator</h2>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table>
        <tr>
            <td colspan="2">
                <input type="text" name="txtSearch" id="txtSearch" /><input type="button" id="btnSearch"
                    value="Search" />
            </td>
        </tr>
        <tr>
            <td valign="top" id="facet_list">
                <%
                    Html.RenderPartial("FacetList", this.Model); %>
            </td>
            <td valign="top" id="query_results">
                <% Html.RenderPartial("QueryResults", this.Model); %>
            </td>
        </tr>
    </table>
</asp:Content>
