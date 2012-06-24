using System;
using System.Globalization;

namespace Funq
{
	/// <summary>
	/// Exception thrown by the container when a service cannot be resolved.
	/// </summary>
#if !SILVERLIGHT
	[Serializable]
#endif
	public class ResolutionException : Exception
	{
		/// <summary>
		/// Initializes the exception with the service that could not be resolved.
		/// </summary>
		public ResolutionException(Type missingServiceType)
			: base(String.Format(
				CultureInfo.CurrentCulture,
                "Required dependency of type {0} could not be resolved.", 
				missingServiceType.FullName))
		{ }

		/// <summary>
		/// Initializes the exception with the service (and its name) that could not be resolved.
		/// </summary>
		public ResolutionException(Type missingServiceType, string missingServiceName)
			: base(String.Format(
				CultureInfo.CurrentCulture,
				"Required dependency of type {0} could not be resolved.",
				missingServiceType.FullName, missingServiceName))
		{ }
		
		/// <summary>
		/// Initializes the exception with an arbitrary message.
		/// </summary>
		public ResolutionException(string message) : base(message) { }
	}
}
