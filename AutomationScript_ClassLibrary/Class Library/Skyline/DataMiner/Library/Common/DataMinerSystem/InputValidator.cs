namespace Skyline.DataMiner.Library.Common
{
	using System;
	using System.Linq;
	using System.Text.RegularExpressions;
	using Templates;

	/// <summary>
	/// Contains methods for input validation.
	/// </summary>
	internal static class InputValidator
    {
		/// <summary>
		/// Validates the name of an element, service, redundancy group, template or folder.
		/// </summary>
		/// <param name="name">The element name.</param>
		/// <param name="parameterName">The name of the parameter that is passing the name.</param>
		/// <exception cref="ArgumentNullException">The value of a set operation is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException">The value of a set operation is empty or white space.</exception>
		/// <exception cref="ArgumentException">The value of a set operation exceeds 200 characters.</exception>
		/// <exception cref="ArgumentException">The value of a set operation contains a forbidden character.</exception>
		/// <exception cref="ArgumentException">The value of a set operation contains more than one '%' character.</exception>
		/// <returns><c>true</c> if the name is valid; otherwise, <c>false</c>.</returns>
		/// <remarks>Forbidden characters: '\', '/', ':', '*', '?', '"', '&lt;', '&gt;', '|', '°', ';'.</remarks>
		public static string ValidateName(string name, string parameterName)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (parameterName == null)
            {
                throw new ArgumentNullException("parameterName");
            }

            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("The name must not be null or white space.", parameterName);
            }

            string trimmedName = name.Trim();

            if (trimmedName.Length > 200)
            {
                throw new ArgumentException("The name must not exceed 200 characters.", parameterName);
            }

			// White space is trimmed.
			if (trimmedName[0].Equals('.'))
            {
                throw new ArgumentException("The name must not start with a dot ('.').", parameterName);
            }

            if (trimmedName[trimmedName.Length-1].Equals('.'))
            {
                throw new ArgumentException("The name must not end with a dot ('.').", parameterName);
            }

            if (!Regex.IsMatch(trimmedName, @"^[^/\\:;\*\?<>\|°""]+$"))
            {
                throw new ArgumentException("The name contains a forbidden character.", parameterName);
            }

            if (trimmedName.Count(x => x == '%') > 1)
            {
                throw new ArgumentException("The name must not contain more than one '%' characters.", parameterName);
            }

			return trimmedName;
        }

		/// <summary>
		/// Validates the specified name for a view.
		/// </summary>
		/// <param name="name">The view name.</param>
		/// <param name="parameterName">The name of the parameter to which the view name is passed.</param>
		/// <exception cref="ArgumentNullException">The value of a set operation is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="name"/> is invalid.</exception>
		/// <exception cref="ArgumentException">The value of a set operation exceeds 200 characters.</exception>
		/// <exception cref="ArgumentException">The value of a set operation contains a forbidden character.</exception>
		/// <exception cref="ArgumentException">The value of a set operation contains more than one '%' character.</exception>
		/// <returns>The validated view name.</returns>
		public static string ValidateViewName(string name, string parameterName)
        {
            if (name == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("The name must not be null or white space.", parameterName);
            }

			string trimmedName = name.Trim();

			if (trimmedName.Length > 0)
			{
				if (trimmedName.Length > 200)
				{
					throw new ArgumentException("The name must not exceed 200 characters.", parameterName);
				}

				ValidateViewNameForbiddenCharacters(trimmedName, parameterName);
			}

			return trimmedName;
        }

		/// <summary>
		/// Determines whether the specified template is compatible with the specified protocol.
		/// </summary>
		/// <param name="template">The template.</param>
		/// <param name="protocol">The protocol.</param>
		/// <returns><c>true</c> if the template is compatible with the protocol; otherwise, <c>false</c>.</returns>
		public static bool IsCompatibleTemplate(IDmsTemplate template, IDmsProtocol protocol)
        {
            bool isCompatible = true;

            if (template != null && (!template.Protocol.Name.Equals(protocol.Name, StringComparison.OrdinalIgnoreCase) || !template.Protocol.Version.Equals(protocol.Version, StringComparison.OrdinalIgnoreCase)))
            {
                isCompatible = false;
            }

            return isCompatible;
        }

		/// <summary>
		/// Validates the specified name for a view for forbidden characters.
		/// </summary>
		/// <param name="viewName">The view name.</param>
		/// <param name="parameterName">The name of the parameter to which the view name is passed.</param>
		/// <exception cref="ArgumentException"><paramref name="viewName"/> is invalid.</exception>
		private static void ValidateViewNameForbiddenCharacters(string viewName, string parameterName)
		{
			if (viewName[0].Equals('.'))
			{
				throw new ArgumentException("The name must not start with a dot ('.').", parameterName);
			}

			if (viewName[viewName.Length - 1].Equals('.'))
			{
				throw new ArgumentException("The name must not end with a dot ('.').", parameterName);
			}

			if (viewName.Contains('|'))
			{
				throw new ArgumentException("The name contains a forbidden character. (Forbidden characters: '|')", parameterName);
			}

			if (viewName.Count(x => x == '%') > 1)
			{
				throw new ArgumentException("The name must not contain more than one '%' characters.", parameterName);
			}
		}
	}
}