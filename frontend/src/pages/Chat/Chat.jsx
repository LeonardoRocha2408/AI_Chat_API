import { useState } from 'react'
import ChatHistory from '../.././components/ChatHistory'

function Chat() {
    const [message, setMessage] = useState("");
    const [messages, setMessages] = useState([]);


    const handleChange = (e) => { setMessage(e.target.value) };

    async function sendMessage() {
        setMessages([...messages, {
            id: crypto.randomUUID(),
            role: "user",
            content: message
        }]);
        setMessage("");

        try {
            const response = await fetch("https://localhost:7076/chat", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    content: message
                })
            }
            );
            const data = await response.json();

            setMessages([...messages, {
                id: crypto.randomUUID(),
                role: "assistant",
                content: data.content
            }])
        }
        catch (error) {
            console.log(error);
            alert(error);
        }
    } 

    return (
        <>
            <h1>My Chatbot</h1>

            <ChatHistory
                messages={messages} />

            <input
                type="text"
                placeholder="Ask something"
                value={message}
                onChange={handleChange}></input>

            <button onClick={sendMessage}>Enviar</button>
       </>
  );
}


export default Chat;