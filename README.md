<img width="1577" height="939" alt="1" src="https://github.com/user-attachments/assets/226f7c76-5b33-4824-bfe3-37a0673135ac" />
<img width="1441" height="797" alt="2" src="https://github.com/user-attachments/assets/8b52a652-3d6c-4ba6-93d8-4324fafd38f8" />

Mobilis AI Support Assistant
Real-Time Support Powered by Modern Communication Technology
Overview

This project uses a long context to create the best possible AI assistant for Mobilis services.
The context is not included in the repository and exists only on the live server when hosted.

Caching is implemented to improve performance and loading speed, as the context is long and comprehensive, and sending it on every HTTP request to the AI would negatively affect ping.

Tech Stack

.NET Core

Blazor (minimal front-end)

External AI API Service

Considerations

For future integration, consider updating the front-end, as it is currently written in Vanilla JavaScript.

Setup
1. Clone the project
2. Restore dependencies
dotnet restore

3. Add context

Create a Data folder

Inside it, add a ContextResponse.txt file

4. Add API key

Add your API key as shown in appsettings.json.example
