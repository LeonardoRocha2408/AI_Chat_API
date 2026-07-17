function Message(message) {
    return (
        <div>
            <strong>{message.role}</strong>
            <p>{message.content}</p>
        </div>
    );
}
export default Message;