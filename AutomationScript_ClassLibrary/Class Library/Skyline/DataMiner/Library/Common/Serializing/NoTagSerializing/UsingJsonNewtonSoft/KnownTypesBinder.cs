namespace Skyline.DataMiner.Library.Common.Serializing.NoTagSerializing.UsingJsonNewtonSoft
{
	using Newtonsoft.Json.Serialization;

	using Skyline.DataMiner.Library.Common.Attributes;
	using Skyline.DataMiner.Library.Common.Reflection;

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	[DllImport("Newtonsoft.Json.dll")]
	internal class KnownTypesBinder : ISerializationBinder
	{
		private readonly List<Assembly> loadedAssemblies = new List<Assembly>();
		private Lazy<string[]> nonUniqueTypeNames;

		public KnownTypesBinder()
		{
			if (loadedAssemblies.Count == 0)
			{
				loadedAssemblies = ReflectionHelper.GetLoadedAssemblies();
			}
		}

		public KnownTypesBinder(IList<Type> knownTypes)
		{
			AddKnownTypes(knownTypes);
		}

		public IList<Type> KnownTypes { get; private set; }

		public void AddKnownTypes(IList<Type> knownTypes)
		{
			if (knownTypes != null)
			{
				KnownTypes = knownTypes;
				nonUniqueTypeNames = new Lazy<string[]>(() => { return KnownTypes.GroupBy(x => x.Name).Where(g => g.Count() > 1).Select(y => y.Key).ToArray(); });
			}
		}

		public void BindToName(Type serializedType, out string assemblyName, out string typeName)
		{
			assemblyName = String.Empty;

			if (serializedType == null)
			{
				throw new ArgumentNullException("serializedType");
			}

			if (KnownTypes != null && KnownTypes.Contains(serializedType) && !nonUniqueTypeNames.Value.Contains(serializedType.Name))
			{
				typeName = serializedType.Name;
			}
			else
			{
				typeName = serializedType.FullName;
			}
		}

#pragma warning disable S3776 // Cognitive Complexity of methods should not be too high

		public Type BindToType(string assemblyName, string typeName)
#pragma warning restore S3776 // Cognitive Complexity of methods should not be too high
		{
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}

			Type foundType = null;
			bool array = false;

			// To Deal with Arrays:
			if (typeName.EndsWith("[]", StringComparison.Ordinal))
			{
#pragma warning disable S1226 // Method parameters, caught exceptions and foreach variables' initial values should not be ignored
#pragma warning disable S3257 // Declarations and initializations should be as concise as possible
				typeName = typeName.TrimEnd(new char[] { '[', ']' });
#pragma warning restore S3257 // Declarations and initializations should be as concise as possible
#pragma warning restore S1226 // Method parameters, caught exceptions and foreach variables' initial values should not be ignored
				array = true;
			}

			if (typeName.StartsWith("System", StringComparison.Ordinal) && foundType == null)
			{
				// MSCORLIB
				var mscorlibAssembly = typeof(Object).Assembly;
				try
				{
					foundType = mscorlibAssembly.GetType(typeName);
				}
				catch
				{
					// Ignore exception in order to see if we find the type. Need to use this for logic unfortunately.
				}

				if (foundType == null)
				{
					// SYSTEM.CORE
					var sysCoreAssembly = typeof(System.Uri).Assembly;
					try
					{
						foundType = sysCoreAssembly.GetType(typeName);
					}
					catch
					{
						// Ignore exception in order to see if we find the type. Need to use this for logic unfortunately.
					}
				}
			}

			if (KnownTypes != null)
			{
				try
				{
					foundType = KnownTypes.SingleOrDefault(t => t.Name == typeName);
				}
				catch (InvalidOperationException ex)
				{
					throw (new IncorrectDataException("Type Name: " + typeName + " was unique on serialization side but not on deserialization side. Please verify the same KnownTypes List is used on both ends of the communication.", ex));
				}
			}

			if (KnownTypes != null && foundType == null)
			{
				foundType = KnownTypes.SingleOrDefault(t => t.FullName == typeName);
			}

			if (foundType == null)
			{
				// Checks the calling assemblies.
				foreach (var ass in loadedAssemblies)
				{
					try
					{
						foundType = ass.GetType(typeName);
					}
					catch
					{
						// Ignore exception in order to see if we find the type. Need to use this for logic unfortunately.
					}

					if (foundType == null)
					{
						foundType = ass.GetTypes().FirstOrDefault(p => p.FullName == typeName);
					}

					if (foundType != null) break;
				}
			}

			if (foundType == null)
			{
				// Checks the current assembly.
				foreach (Type t in typeof(KnownTypesBinder).Assembly.GetTypes())
				{
					if (typeName == t.FullName)
					{
						foundType = t;
						break;
					}
				}
			}

			if (foundType == null)
			{
				DefaultSerializationBinder def = new DefaultSerializationBinder();
				if (String.IsNullOrWhiteSpace(assemblyName))
				{
#pragma warning disable S1226 // Method parameters, caught exceptions and foreach variables' initial values should not be ignored
					assemblyName = typeof(KnownTypesBinder).Assembly.GetName().Name;
#pragma warning restore S1226 // Method parameters, caught exceptions and foreach variables' initial values should not be ignored
				}

				foundType = def.BindToType(assemblyName, typeName);
			}

			if (array)
			{
				foundType = foundType.MakeArrayType();
			}

			return foundType;
		}
	}
}