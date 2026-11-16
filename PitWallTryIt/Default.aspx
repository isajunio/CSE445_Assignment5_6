<%@ Page Title="Pit Wall F1 | Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PitWallTryIt._Default" %>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <%-- By: Ayomide Oshilaja. Public landing page . --%>

    <section class="hero">
        <div class="eyebrow yellow">Your race weekend starts here</div>
        <h1 class="hero-title">Think like a driver.<br /><span class="accent">Call the race.</span></h1>
        <p class="lead">
            Your front row seat to Formula 1. Check the calendar and track weather, pick your podium.
        </p>
        <%--  a button to the Member page and a button to the Staff page --%>
        <div class="hero-actions">
            <a href="Member.aspx" class="btn btn-yellow">Member page: Call the podium</a>
            <a href="Staff.aspx" class="btn">Staff page</a>
        </div>
    </section>

    <section class="section">
       
        <p class="lead"> Here is what each page does:</p>

        <div class="grid-3" style="margin-top:22px;">
            <div class="card">
                <span class="card-num">01</span>
                <div class="card-icon">1</div>
                <h3>Call the podium</h3>
                <p class="muted">Members pick their top three drivers before lights out, then climb the league standings.</p>
                <a class="link-arrow" href="Member.aspx">Make your picks</a>
            </div>
            <div class="card">
                <span class="card-num">02</span>
                <div class="card-icon">2</div>
                <h3>Race weekend</h3>
                <p class="muted">Season calendar, drivers, race results, a 5-day track forecast and fantasy results in one place.</p>
                <a class="link-arrow" href="RaceWeekend.aspx">Open race weekend</a>
            </div>
            <div class="card">
                <span class="card-num">03</span>
                <div class="card-icon">3</div>
                <h3>Local Component</h3>
                <p class="muted">Test every local component the team built: encryption, cookies, captcha and app state.</p>
                <a class="link-arrow" href="LocalComponent.aspx">Try the components</a>
            </div>
        </div>
    </section>

    <%-- ===== how to sign up directions ===== --%>
    <section class="section section-alt">
        <h2>How to sign up</h2>
        <div class="steps">
            <div>
                <div class="step-num">01</div>
                <div class="step-title">Create an account</div>
                <div class="muted">Choose a username and password, then pass the captcha.</div>
            </div>
            <div>
                <div class="step-num">02</div>
                <div class="step-title">Log in</div>
                <div class="muted">You land on Call the podium (Member page). Change your password any time.</div>
            </div>
            <div>
                <div class="step-num">03</div>
                <div class="step-title">Make your picks</div>
                <div class="muted">Pick P1 to P3 for a race and watch your points on the standings.</div>
            </div>
        </div>
        <a href="CreateAccount.aspx" class="btn btn-yellow">Create your free account</a>

        <%-- ===== how to test ===== --%>

        <div class="card" style="margin-top:28px; margin-bottom:0;">
            <h3>How to test this application</h3>
            <p class="muted">
                Sign up as a member, or use the staff test login. Use these inputs on the TryIt pages
                (links in the Service Directory down below):
            </p>
            <p>
                <span class="chip">zip 78617 (Austin)</span>
                <span class="chip">season 2025</span>
                <span class="chip">session key 9693</span>
                <span class="chip">top 10</span>
                <span class="chip">text "Hello F1"</span>
            </p>
            <p class="muted" style="margin-bottom:0;">
                Staff test login: <span class="chip">TA</span> <span class="chip">Cse445!</span>
                (select <strong>Staff</strong> on the login page).
            </p>
        </div>
    </section>

    <%-- ===== service directory that links to the pages ===== --%>
    <section class="section">
        <div class="eyebrow">Under the bodywork</div>
        <h2>Service directory<span class="accent">.</span></h2>

        <div class="card table-wrap">
            <table class="directory">
                <tr>
                    <th>#</th><th>Component</th><th>Provider</th><th>Type</th><th>Operation</th>
                    <th>Parameters</th><th>Returns</th><th>Description</th><th>TryIt</th>
                </tr>
                <tr>
                    <td class="num">01</td><td><strong>Encryption</strong></td><td>Ayomide Oshilaja</td>
                    <td><span class="badge">DLL</span></td><td>Encrypt</td>
                    <td><span class="chip">string plainText</span></td><td><span class="chip">string</span></td>
                    <td>AES-256 encryption, returns Base64. Used to store passwords in Member.xml and Staff.xml.</td>
                    <td><a class="link-arrow" href="LocalComponent.aspx#encrypt">TryIt</a></td>
                </tr>
                <tr>
                    <td class="num">02</td><td><strong>Decryption</strong></td><td>Gabriela Martinez</td>
                    <td><span class="badge">DLL</span></td><td>Decrypt</td>
                    <td><span class="chip">string cipherText</span></td><td><span class="chip">string</span></td>
                    <td>Restores text encrypted by Encrypt, using the same key.</td>
                    <td><a class="link-arrow" href="LocalComponent.aspx#decrypt">TryIt</a></td>
                </tr>
                <tr>
                    <td class="num">03</td><td><strong>Cookie profile</strong></td><td>Gabriela Martinez</td>
                    <td><span class="badge">Cookie</span></td><td>SaveFavorites</td>
                    <td><span class="chip">string driver</span><span class="chip">string team</span></td><td><span class="chip">void</span></td>
                    <td>Remembers a favorite driver and team for 7 days.</td>
                    <td><a class="link-arrow" href="LocalComponent.aspx#cookies">TryIt</a></td>
                </tr>
                <tr>
                    <td class="num">04</td><td><strong>Captcha</strong></td><td>Izzy Junio</td>
                    <td><span class="badge">User control</span></td><td>Verify</td>
                    <td><span class="chip">string answer</span></td><td><span class="chip">bool</span></td>
                    <td>Image verifier used on sign up.</td>
                    <td><a class="link-arrow" href="LocalComponent.aspx#captcha">TryIt</a></td>
                </tr>
                <tr>
                    <td class="num">05</td><td><strong>App events</strong></td><td>Ayomide Oshilaja</td>
                    <td><span class="badge">Global.asax</span></td><td>Application_Start, Application_AuthenticateRequest, Application_Error</td>
                    <td><span class="chip">none</span></td><td><span class="chip">none</span></td>
                    <td>Starts the visitor counter, creates the XML account files, reads Member/Staff roles, shows the error page.</td>
                    <td><a class="link-arrow" href="LocalComponent.aspx#appstate">TryIt</a></td>
                </tr>
                <tr>
                    <td class="num">06</td><td><strong>Visitor counter</strong></td><td>Izzy Junio</td>
                    <td><span class="badge">Global.asax</span></td><td>Session_Start, Session_End</td>
                    <td><span class="chip">none</span></td><td><span class="chip">int (app state)</span></td>
                    <td>Counts visitors since the app started. Shown in the footer of every page.</td>
                    <td><a class="link-arrow" href="#footer">Footer</a></td>
                </tr>
                <tr>
                    <td class="num">07</td><td><strong>Access control</strong></td><td>Ayomide Oshilaja</td>
                    <td><span class="badge">Class + XML</span></td><td>ValidateMember, ValidateStaff, AddMember, SignIn</td>
                    <td><span class="chip">string user</span><span class="chip">string password</span></td><td><span class="chip">bool</span></td>
                    <td>Forms security with accounts in Member.xml and Staff.xml (encrypted passwords).</td>
                    <td><a class="link-arrow" href="Login.aspx">TryIt</a></td>
                </tr>
                <tr>
                    <td class="num">08</td><td><strong>Track weather</strong></td><td>Ayomide Oshilaja</td>
                    <td><span class="badge badge-service">WSDL service</span></td><td>Weather5day</td>
                    <td><span class="chip">string zipcode</span></td><td><span class="chip">string[]</span></td>
                    <td>5-day forecast for a track zip code (NWS NDFD).</td>
                    <td><a class="link-arrow" href="RaceWeekend.aspx#weather">TryIt</a></td>
                </tr>
                <tr>
                    <td class="num">09</td><td><strong>Race center</strong></td><td>Ayomide Oshilaja</td>
                    <td><span class="badge badge-service">REST service</span></td><td>GetRaceCalendar, GetDrivers, GetRaceResults</td>
                    <td><span class="chip">string year</span><span class="chip">string sessionKey</span><span class="chip">string top</span></td>
                    <td><span class="chip">List&lt;RaceInfo&gt;</span><span class="chip">List&lt;DriverInfo&gt;</span><span class="chip">List&lt;RaceResult&gt;</span></td>
                    <td>Season schedule, driver list and finishing order (OpenF1).</td>
                    <td><a class="link-arrow" href="RaceWeekend.aspx#race-center">TryIt</a></td>
                </tr>
                <tr>
                    <td class="num">10</td><td><strong>Fantasy league</strong></td><td>Ayomide Oshilaja</td>
                    <td><span class="badge badge-service">WSDL service</span></td><td>RegisterPlayer, SubmitPicks, GetPlayerPicks, GetLeaderboard, ScoreRace</td>
                    <td><span class="chip">string username</span><span class="chip">string sessionKey</span><span class="chip">int p1, p2, p3</span></td>
                    <td><span class="chip">string</span><span class="chip">string[]</span></td>
                    <td>Podium picks, race scoring and league rankings.</td>
                    <td><a class="link-arrow" href="RaceWeekend.aspx#fantasy">TryIt</a></td>
                </tr>
                <tr>
                    <td class="num">11</td><td><strong>Captcha service</strong></td><td>Izzy Junio</td>
                    <%-- TODO (Izzy): fill in your service type, operation, parameters, return type and description --%>
                    <td><span class="badge badge-service">[WSDL / REST]</span></td><td>[Operation]</td>
                    <td><span class="chip">[type name]</span></td><td><span class="chip">[type]</span></td>
                    <td>[Description]</td>
                    <td><a class="link-arrow" href="CreateAccount.aspx">TryIt</a></td>
                </tr>
                 <tr>
                    <td class="num">12</td><td><strong>Pit strategy</strong></td><td>Gabriela Martinez</td>
                    <td><span class="badge badge-service">REST service</span></td><td>EstimateRaceTime</td>
                    <td><span class="chip">double averageLapSeconds</span><span class="chip">int totalLaps</span><span class="chip">int plannedPitStops</span><span class="chip">double pitStopLossSeconds</span></td>
                    <td><span class="chip">double</span></td>
                    <td>Estimates total race time in seconds: (average lap × laps) + (pit stops × time lost per stop). Rejects unrealistic inputs with HTTP 400.</td>
                    <td><a class="link-arrow" href="RaceWeekend.aspx#pit-strategy">TryIt</a></td>
                </tr>
            </table>
            <div class="table-foot">
                <span>12 components and services</span>
                <%-- TODO (team): replace with the WebStrar URL before submitting --%>
                <span>Deployed at [WebStrar URL]</span>
            </div>
        </div>
    </section>
</asp:Content>