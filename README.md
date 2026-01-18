<img width="1577" height="939" alt="1" src="https://github.com/user-attachments/assets/226f7c76-5b33-4824-bfe3-37a0673135ac" />
<img width="1441" height="797" alt="2" src="https://github.com/user-attachments/assets/8b52a652-3d6c-4ba6-93d8-4324fafd38f8" />

##Architecture : 
<img width="1875" height="799" alt="archi" src="https://github.com/user-attachments/assets/804181d9-0127-412d-8669-1b7da8a9441d" />



<h1 align="center">Mobilis AI Support Assistant</h1>

<h3 align="center">
Powered by Modern Real Time Communication Technology
</h3>

<hr/>


<p>
This project uses a <strong>long context</strong> to create the best possible AI assistant for
<strong>Mobilis services</strong>.<br/>
(The context is <strong>not included</strong> in the repository and exists only on the
<strong>live server</strong> when i host it).
</p>

<p>
Caching is implemented to improve <strong>performance and loading speed</strong>, as the context
is long and comprehensive, and sending it on every HTTP request to the AI would negatively affect
<strong>ping</strong>.
</p>

<h2>Tech Stack</h2>

<ul>
  <li>.NET Core</li>
  <li>Blazor  (for very minimal front-end)</li>
  <li>External AI API Service</li>
</ul>

<h2>Considerations</h2>

<p>
For future integration, consider updating the front-end, as it is currently written in
Vanilla JavaScript.
</p>

<h2>Setup</h2>

<h3>1. Clone the project</h3>

<h3>2. Restore dependencies</h3>

<pre><code>dotnet restore</code></pre>

<h3>3. Add context</h3>

<ul>
  <li>Create a <code>Data</code> folder</li>
  <li>Inside it, add a <code>ContextResponse.txt</code> file</li>
</ul>

<h3>4. Add API key</h3>

<p>
Add your API key as shown in <code>appsettings.json.example</code>
</p>

<h3>(. Run App</h3>

<pre><code>dotnet run</code></pre>






