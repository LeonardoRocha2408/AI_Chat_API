import Message from "./Message";

function ChatHistory({ messages }) {
    return (
        <>
            {messages.map((m) => (
                <Message
                    key={m.id} messages={m} />
            ))}
        </>
    );
}

export default ChatHistory;