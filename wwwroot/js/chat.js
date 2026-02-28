const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Information)
    .build();

// Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

// Receive messages from the server
connection.on("ReceiveMessage", (message, response) => {
    const messagesList = document.getElementById("messagesList");

    //user input
    const userInput = document.createElement("li");
    userInput.innerHTML = `<strong></strong> ${escapeHtml(message)}`;
    userInput.style.color = "#166d47";
    userInput.style.userSelect = "none";
    userInput.style.textAlign = "right";
    userInput.style.fontSize = "17px";
    messagesList.appendChild(userInput);

    // Display assistant response
    const responseLi = document.createElement("li");
    responseLi.innerHTML = `<strong>Mobilis:</strong> ${escapeHtml(response)}`;
    responseLi.style.color = "#000000";
    responseLi.style.fontSize = "18px";
    messagesList.appendChild(responseLi);




    // Auto-scroll to bottom
    messagesList.scrollTop = messagesList.scrollHeight;
});

// Start the connection
connection.start()
    .then(() => {
        console.log("Connected to ChatHub successfully!");
        document.getElementById("sendButton").disabled = false;
    })
    .catch(err => {
        console.error("Connection error: ", err.toString());
        alert("Failed to connect to chat server. Please refresh the page.");
    });

// Handle connection state changes
connection.onreconnecting((error) => {
    console.warn("Connection lost. Reconnecting...", error);
    document.getElementById("sendButton").disabled = true;
    const messagesList = document.getElementById("messagesList");
    const statusLi = document.createElement("li");
    statusLi.textContent = "Reconnecting...";
    statusLi.style.color = "#ff9800";
    statusLi.id = "reconnectStatus";
    messagesList.appendChild(statusLi);
});

connection.onreconnected((connectionId) => {
    console.log("Reconnected successfully!", connectionId);
    document.getElementById("sendButton").disabled = false;
    const statusElement = document.getElementById("reconnectStatus");
    if (statusElement) {
        statusElement.remove();
    }
});

connection.onclose((error) => {
    console.error("Connection closed.", error);
    document.getElementById("sendButton").disabled = true;
    alert("Connection to chat server lost. Please refresh the page.");
});

// send message on button click
document.getElementById("sendButton").addEventListener("click", sendMessage);

// send message on Enter key 
document.getElementById("messageInput").addEventListener("keypress", (event) => {
    if (event.key === "Enter") {
        event.preventDefault();
        sendMessage();
    }
});

// Send message function
function sendMessage() {
    const message = document.getElementById("messageInput").value.trim();

    // Validation
    if (!message) {
        alert("Please enter a message");
        document.getElementById("messageInput").focus();
        return;
    }
    if (message.length > 800) {
        alert("Message is too Long")
        messageInput.value = "";
        userInput.value = "";

    }

    // disable button while sending
    const sendButton = document.getElementById("sendButton");
    messageInput.disabled = true;
    sendButton.disabled = true;
    sendButton.value = "Sending...";

    connection.invoke("SendMessage", message)
        .then(() => {
            // clear message input after successful send
            document.getElementById("messageInput").value = "";
            document.getElementById("messageInput").focus();
        })
        .catch(err => {
            console.error("Error sending message: ", err.toString());
            alert("Failed to send message. Please try again.");
        })
        .finally(() => {
            // button for re-enable 
            sendButton.disabled = false;
            sendButton.value = "Send Message";
            messageInput.disabled = false;
        });
}

function escapeHtml(text) {
    const map = {
        '&': '&amp;',
        '<': '&lt;',
        '>': '&gt;',
        '"': '&quot;',
        "'": '&#039;'
    };
    return text.replace(/[&<>"']/g, m => map[m]);
}