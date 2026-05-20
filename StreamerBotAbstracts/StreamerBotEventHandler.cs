namespace StreamerBot.Events
{
    public delegate void StreamerBotEventHandler<in TEvent>(TEvent evt)
        where TEvent : StreamerBotEventBase;
}
