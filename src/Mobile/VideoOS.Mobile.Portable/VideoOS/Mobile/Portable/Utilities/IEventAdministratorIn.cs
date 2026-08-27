namespace VideoOS.Mobile.Portable.Utilities;

public interface IEventAdministratorIn<TEvent>
{
	InputEvent<TEvent> OnEvent { get; }
}
