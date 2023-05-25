namespace Xamarin.CommunityToolkit.Ports.Effects
{
	public class TouchInteractionStatusChangedEventArgs : EventArgs
	{
		internal TouchInteractionStatusChangedEventArgs(TouchInteractionStatus touchInteractionStatus)
			=> TouchInteractionStatus = touchInteractionStatus;

		public TouchInteractionStatus TouchInteractionStatus { get; }
	}
}