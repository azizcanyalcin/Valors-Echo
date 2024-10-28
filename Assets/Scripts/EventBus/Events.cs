public interface IEvent { }
public class TestEvent: IEvent {}

public class PlayerEvent : IEvent
{
    public int health;
    public int mana;
}
