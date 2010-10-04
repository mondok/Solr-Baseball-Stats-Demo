<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SolrCodeCamp.BaseballWeb.Models.BaseballView>" %>
<%@ Import Namespace="SolrCodeCamp.BaseballWeb.Models" %>
<% foreach (var facet in this.Model.Facets)
   { %>
<div>
    <div class="facet_title">
        <%: Global.LookupFacetDisplayName( facet.Key) %>
    </div>
    <div class="facet_values">
        <select name="lst_<%= facet.Key %>" multiple="multiple" size="10" class="facet_box">
            <% foreach (var value in facet.Value)
               {%>
            <option value="<%= facet.Key + "^" + value.Item1 %>">
                <%: value.Item1 %></option>
            <%
                }%>
        </select>
    </div>
</div>
<%} %>