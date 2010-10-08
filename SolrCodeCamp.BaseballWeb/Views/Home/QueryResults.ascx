<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SolrCodeCamp.BaseballWeb.Models.BaseballView>" %>
<%@ Import Namespace="SolrCodeCamp.BaseballWeb.Models" %>
<div class="records_found">
    Showing
    <%: this.Model.GameResults.Count  %>
    of
    <%: this.Model.TotalRecordsFound %>
    records
</div>
<div class="clear">&nbsp;</div>
<table>
    <thead>
        <tr>
            <th>
                <%= Html.TableHeaderLink(this.Model,"bg_homeTeam","Home Team") %>
            </th>
            <th>
                <%= Html.TableHeaderLink(this.Model,"bg_visitingTeam","Visiting Team") %>
            </th>
            <th>
                <%= Html.TableHeaderLink(this.Model, "bg_homeTeamScore", "Home Score ")%>
            </th>
            <th>
                <%= Html.TableHeaderLink(this.Model, "bg_visitingTeamScore", "Visitor Score")%>
            </th>
            <th>
                <%= Html.TableHeaderLink(this.Model, "bg_date", "Date")%>
            </th>
            <th>
                <%= Html.TableHeaderLink(this.Model, "bg_dayOrNight", "Day/Night")%>
            </th>
            <th>
                <%= Html.TableHeaderLink(this.Model, "bg_dayOfTheWeek", "Day of Week")%>
            </th>
            <th>
                <%= Html.TableHeaderLink(this.Model, "bg_homePlateUmpire", "Home Plate Umpire")%>
            </th>
            <th>
                <%= Html.TableHeaderLink(this.Model, "bg_winningPitcher", "Winning Pitcher")%>
            </th>
        </tr>
    </thead>
    <tbody>
        <% foreach (var result in this.Model.GameResults)
           {%>
        <tr>
            <td>
                <%: result.HomeTeam %>
            </td>
            <td>
                <%: result.VisitingTeam %>
            </td>
            <td class="number_cell">
                <%: result.HomeTeamScore %>
            </td>
            <td class="number_cell">
                <%: result.VisitingTeamScore %>
            </td>
            <td>
                <%: result.Date.ToString("MM/dd/yyyy") %>
            </td>
            <td>
                <%: result.DayOrNight %>
            </td>
            <td>
                <%: result.DayOfTheWeek %>
            </td>
            <td>
                <%: result.HomePlateUmpireName %>
            </td>
            <td>
                <%: result.WinningPitcher %>
            </td>
        </tr>
        <%
            }%>
    </tbody>
    <tfoot>
        <tr>
            <td>
            </td>
            <td>
            </td>
            <td class="number_cell_foot">
                <%: Model.GameResults.Sum(t => t.HomeTeamScore) %>
            </td>
            <td class="number_cell_foot">
                <%: Model.GameResults.Sum(t => t.VisitingTeamScore) %>
            </td>
            <td>
            </td>
            <td>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
    </tfoot>
</table>
