<%@ Page Title="Race weekend | Pit Wall" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RaceWeekend.aspx.cs" Inherits="PitWallTryIt.RaceWeekend" %>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <%-- Provider: Ayomide Oshilaja. Race weekend page: the Assignment 3 TryIt functions, in the new Pit Wall design. --%>

    <section class="hero">
        <div class="eyebrow yellow">Race weekend hub</div>
        <h1>Know the <span class="accent">weekend.</span></h1>
        <p class="lead">Track weather, the season calendar, drivers, race results and the fantasy league, all in one place.</p>
    </section>

    <%-- ===== Track weather ===== --%>
    <section class="section" id="weather">
        <div class="card">
            <span class="badge badge-service">WSDL service</span>
            <h3 style="margin-top:10px;">Track weather</h3>
            <p class="muted">Gets the 5-day weather forecast for a U.S. race track zipcode (Austin 78617, Miami 33054, Las Vegas 89109).</p>
            <div class="form-row">
                <div>
                    <label>Zipcode</label>
                    <asp:TextBox ID="txtZip" runat="server" Text="78617" />
                </div>
                <asp:Button ID="btnWeather" runat="server" Text="Check weather" OnClick="btnWeather_Click" CssClass="btn btn-yellow" />
            </div>
            <div class="output"><asp:Label ID="lblWeather" runat="server" /></div>
        </div>
    </section>

    <%-- ===== Race center ===== --%>
    <section id="race-center">
        <div class="card">
            <span class="badge badge-service">REST service</span>
            <h3 style="margin-top:10px;">Race center</h3>
            <p class="muted">Season calendar, driver lineup and race results from the OpenF1 API.</p>

            <div class="form-row">
                <div>
                    <label>Calendar: every Grand Prix in a season (2023 or later)</label>
                    <asp:TextBox ID="txtYear" runat="server" Text="2025" />
                </div>
                <asp:Button ID="btnCalendar" runat="server" Text="Show calendar" OnClick="btnCalendar_Click" CssClass="btn btn-yellow" />
            </div>

            <div class="form-row">
                <div>
                    <label>Race session key (9693 = Australia 2025)</label>
                    <asp:TextBox ID="txtSession" runat="server" Text="9693" />
                </div>
                <div>
                    <label>How many results</label>
                    <asp:TextBox ID="txtTop" runat="server" Text="10" />
                </div>
                <asp:Button ID="btnDrivers" runat="server" Text="Drivers" OnClick="btnDrivers_Click" CssClass="btn btn-yellow" />
                <asp:Button ID="btnResults" runat="server" Text="Results" OnClick="btnResults_Click" CssClass="btn btn-yellow" />
            </div>
            <div class="output"><asp:Label ID="lblRaces" runat="server" /></div>
        </div>
    </section>

    <%-- ===== Fantasy league ===== --%>
    <section id="fantasy">
        <div class="card">
            <span class="badge badge-service">WSDL service</span>
            <h3 style="margin-top:10px;">Fantasy league</h3>
            <p class="muted">Join the league, pick the podium for a race, then score it with the real results.</p>

            <div class="form-row">
                <div>
                    <label>Your username</label>
                    <asp:TextBox ID="txtUser" runat="server" />
                </div>
                <asp:Button ID="btnJoin" runat="server" Text="Join" OnClick="btnJoin_Click" CssClass="btn btn-yellow" />
                <asp:Button ID="btnMyPicks" runat="server" Text="My picks" OnClick="btnMyPicks_Click" CssClass="btn" />
            </div>

            <div class="form-row">
                <div>
                    <label>Race session key</label>
                    <asp:TextBox ID="txtPickSession" runat="server" Text="9693" />
                </div>
                <div>
                    <label>P1 (car number)</label>
                    <asp:TextBox ID="txtP1" runat="server" placeholder="P1" />
                </div>
                <div>
                    <label>P2</label>
                    <asp:TextBox ID="txtP2" runat="server" placeholder="P2" />
                </div>
                <div>
                    <label>P3</label>
                    <asp:TextBox ID="txtP3" runat="server" placeholder="P3" />
                </div>
            </div>

            <div class="hero-actions" style="margin-top:0;">
                <asp:Button ID="btnLockPicks" runat="server" Text="Lock picks" OnClick="btnLockPicks_Click" CssClass="btn btn-yellow" />
                <asp:Button ID="btnScore" runat="server" Text="Score race" OnClick="btnScore_Click" CssClass="btn" />
                <asp:Button ID="btnLeaderboard" runat="server" Text="Leaderboard" OnClick="btnLeaderboard_Click" CssClass="btn" />
            </div>
            <div class="output"><asp:Label ID="lblFantasy" runat="server" /></div>
        </div>
    </section>

 
</asp:Content>