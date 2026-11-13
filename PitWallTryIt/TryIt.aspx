<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TryIt.aspx.cs" Inherits="PitWallTryIt.TryIt" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Pit Wall</title>
    <style>
        /* red bull colors: navy #121F45, blue #223971, red #CC1E4A, yellow #FFC906 */
        body { margin: 0; font-family: Arial, Helvetica, sans-serif; background: #f2f2f2; color: #121F45; }
        a { color: inherit; }

        /* top nav bar */
        .nav { background: #121F45; color: #fff; display: flex; align-items: center; padding: 0 30px; height: 60px; border-bottom: 4px solid #CC1E4A; }
        .logo { font-weight: 900; font-style: italic; font-size: 26px; letter-spacing: 1px; }
        .logo span { color: #FFC906; }
        .nav a { color: #fff; margin-left: 28px; font-size: 14px; font-weight: bold; text-decoration: none; }
        .nav a:hover { color: #FFC906; }

        /* big banner */
        .hero { background: #223971; color: #fff; padding: 40px 30px; }
        .hero h1 { margin: 0; font-size: 46px; font-style: italic; text-transform: uppercase; }
        .hero h1 span { color: #FFC906; }
        .hero p { margin: 8px 0 0; font-size: 16px; }
        .hero small { display: block; margin-top: 6px; color: #b8c4e6; }
        .flag { height: 14px; background: repeating-linear-gradient(90deg, #121F45 0 14px, #fff 14px 28px); }

        /* cards */
        .grid { display: flex; gap: 20px; padding: 26px 30px 40px; flex-wrap: wrap; align-items: flex-start; }
        .card { background: #fff; border-radius: 10px; flex: 1; min-width: 320px; box-shadow: 0 2px 6px rgba(0,0,0,.12); overflow: hidden; }
        .card h2 { margin: 0; background: #CC1E4A; color: #fff; padding: 12px 16px; font-style: italic; text-transform: uppercase; font-size: 19px; }
        .body { padding: 14px 16px 16px; }
        .body p { margin: 0 0 6px; font-size: 14px; }
        .step { margin-top: 14px; }
        .step b { display: block; font-size: 13px; margin-bottom: 4px; }
        input[type=text] { border: 2px solid #223971; border-radius: 6px; padding: 7px; width: 110px; font-size: 14px; }
        input.small { width: 46px; }
        input[type=submit] { background: #FFC906; color: #121F45; border: 0; border-radius: 20px; padding: 8px 16px; font-weight: bold; font-size: 14px; cursor: pointer; margin: 4px 4px 0 0; }
        input[type=submit]:hover { background: #CC1E4A; color: #fff; }
        .out { display: block; background: #121F45; color: #fff; border-radius: 6px; padding: 10px; font-size: 13px; line-height: 1.5; margin-top: 12px; min-height: 40px; }

        /* small service info at the bottom of each card */
        .notes { font-size: 11px; color: #888; border-top: 1px dashed #ccc; padding: 8px 16px; line-height: 1.5; }
        .foot { text-align: center; font-size: 12px; color: #888; padding-bottom: 24px; }
    </style>
</head>
<body>
    <form id="form1" runat="server">


        <div class="hero">
            <h1>Race Weekend <span>Hub</span></h1>
            <small>CSE 445 Assignment 3 TryIt page by Ayomide Oshilaja</small>
        </div>
        <div class="flag"></div>

        <div class="grid">

            <!-- weather card -->
            <div class="card" id="weather">
                <h2>☀️ Track Weather</h2>
                <div class="body">
                    <p>Gets the 5-day weather forecast for a U.S. race track zipcode (Austin 78617, Miami 33054, Las Vegas 89109).</p>
                    <div class="step">
                        <b>Zipcode</b>
                        <asp:TextBox ID="txtZip" runat="server" Text="78617" />
                        <asp:Button ID="btnWeather" runat="server" Text="Check weather" OnClick="btnWeather_Click" />
                    </div>
                    <asp:Label ID="lblWeather" runat="server" CssClass="out" />
                </div>
                <div class="notes">
                    Service: http://localhost:5001/WeatherService.svc<br />
                    string[] Weather5day(string zipcode)
                </div>
            </div>

            <!-- race center card -->
            <div class="card" id="races">
                <h2>🏎️ Race Center</h2>
                <div class="body">
                    <p>Season calendar, driver lineup and race results from the OpenF1 API.</p>
                    <div class="step">
                        <b>Calendar: every Grand Prix in a season (2023 or later)</b>
                        <asp:TextBox ID="txtYear" runat="server" Text="2025" />
                        <asp:Button ID="btnCalendar" runat="server" Text="Show calendar" OnClick="btnCalendar_Click" />
                    </div>
                    <div class="step">
                        <b>Race session key (9693 = Australia 2025) and how many results</b>
                        <asp:TextBox ID="txtSession" runat="server" Text="9693" />
                        <asp:TextBox ID="txtTop" runat="server" Text="10" CssClass="small" />
                        <asp:Button ID="btnDrivers" runat="server" Text="Drivers" OnClick="btnDrivers_Click" />
                        <asp:Button ID="btnResults" runat="server" Text="Results" OnClick="btnResults_Click" />
                    </div>
                    <asp:Label ID="lblRaces" runat="server" CssClass="out" />
                </div>
                <div class="notes">
                    Service: http://localhost:5001/RaceDataService.svc (REST)<br />
                    /races/{year}: List&lt;RaceInfo&gt; GetRaceCalendar(string year)<br />
                    /drivers/{sessionKey}: List&lt;DriverInfo&gt; GetDrivers(string sessionKey)<br />
                    /results/{sessionKey}?top={top}: List&lt;RaceResult&gt; GetRaceResults(string sessionKey, string top)
                </div>
            </div>

            <!-- fantasy league card -->
            <div class="card" id="fantasy">
                <h2>🏆 Fantasy League</h2>
                <div class="body">
                    <p>Join the league, pick the podium for a race, then score it with the real results.</p>
                    <div class="step">
                        <b>Your username</b>
                        <asp:TextBox ID="txtUser" runat="server" />
                        <asp:Button ID="btnJoin" runat="server" Text="Join" OnClick="btnJoin_Click" />
                        <asp:Button ID="btnMyPicks" runat="server" Text="My picks" OnClick="btnMyPicks_Click" />
                    </div>
                    <div class="step">
                        <b>Race session key and your P1, P2, P3 (car numbers)</b>
                        <asp:TextBox ID="txtPickSession" runat="server" Text="9693" />
                        <asp:TextBox ID="txtP1" runat="server" CssClass="small" placeholder="P1" />
                        <asp:TextBox ID="txtP2" runat="server" CssClass="small" placeholder="P2" />
                        <asp:TextBox ID="txtP3" runat="server" CssClass="small" placeholder="P3" />
                        <br />
                        <asp:Button ID="btnLockPicks" runat="server" Text="Lock picks" OnClick="btnLockPicks_Click" />
                        <asp:Button ID="btnScore" runat="server" Text="Score race" OnClick="btnScore_Click" />
                        <asp:Button ID="btnLeaderboard" runat="server" Text="Leaderboard" OnClick="btnLeaderboard_Click" />
                    </div>
                    <asp:Label ID="lblFantasy" runat="server" CssClass="out" />
                </div>
                <div class="notes">
                    Service: http://localhost:5001/FantasyLeagueService.svc<br />
                    Join: string RegisterPlayer(string username)<br />
                    Lock picks: string SubmitPicks(string username, string sessionKey, int p1, int p2, int p3)<br />
                    Score race: string ScoreRace(string sessionKey)<br />
                    Leaderboard: string[] GetLeaderboard()<br />
                    My picks: string[] GetPlayerPicks(string username)
                </div>
            </div>

        </div>

    </form>
</body>
</html>