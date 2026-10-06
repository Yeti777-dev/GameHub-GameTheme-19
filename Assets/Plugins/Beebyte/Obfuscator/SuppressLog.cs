using System;

namespace Beebyte.Obfuscator
{
	/**
	 * Suppresses certain messages (usually warnings) that the Obfuscator can output.
	 */
	[AttributeUsage(AttributeTargets.Method)]
	public class SuppressLog : System.Attribute
	{
#pragma warning disable 414
		private readonly MessageCode _messageCode;
#pragma warning restore 414

		private SuppressLog()
		{
		}

		public SuppressLog(MessageCode messageCode)
		{
			_messageCode = messageCode;
		}
	}
}
