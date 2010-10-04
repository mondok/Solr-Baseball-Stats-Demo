<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<SolrCodeCamp.BaseballWeb.Models.BaseballView>" %>

<% Html.RenderPartial("QueryResults", this.Model); %>
